# Tests/Integration/Repositories/Filesystem/DeepfakeDetectorRepositoryIntegrationTests.py

import pytest
import torch
import torch.nn as nn
from pathlib import Path
from typing import Dict, Any

from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Repositories.Exceptions.DeepfakeDetectorRepositoryExceptions import (
    DeepfakeDetectorNotFoundError,
    DeepfakeDetectorLoadError,
    DeepfakeDetectorSaveError,
    DeepfakeDetectorAlreadyExistsError,
)
from Backend.DataAccess.Repositories.Filesystem.DeepfakeDetectorRepository import DeepfakeDetectorRepository


@pytest.fixture
def repo() -> DeepfakeDetectorRepository:
    """Фикстура, возвращающая экземпляр репозитория."""
    return DeepfakeDetectorRepository()


@pytest.fixture
def temp_dir(tmp_path) -> Path:
    """Фикстура для временной директории."""
    return tmp_path


@pytest.fixture
def dummy_settings() -> DeepfakeDetectorSettings:
    """Создаёт настройки с минимальными весами для тестов."""
    return DeepfakeDetectorSettings(
        weights={'dummy': torch.tensor([1.0])},
        cnn_model_name='efficientnet_b0',
        vit_model_name='google/vit-base-patch16-224-in21k',
        fusion_dim=128,
        dropout_rate=0.2,
        num_classes=1,
        threshold=0.5,
        input_size=(224, 224)
    )


@pytest.fixture
def dummy_modules() -> Dict[str, Any]:
    """Создаёт словарь с фиктивными модулями для тестов сохранения."""
    device = torch.device('cpu')
    return {
        'cnn': nn.Linear(10, 10).to(device).eval(),
        'vit': nn.Linear(10, 10).to(device).eval(),
        'cnn_proj': nn.Linear(10, 10).to(device).eval(),
        'vit_proj': nn.Linear(10, 10).to(device).eval(),
        'classifier': nn.Linear(10, 10).to(device).eval(),
        'device': device
    }


# ----------------------------------------------------------------------
# Тесты для LoadModelAsync
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestDeepfakeDetectorRepositoryIntegration:
    """Интеграционные тесты для DeepfakeDetectorRepository."""

    async def test_load_model_success(self, mocker, repo, dummy_settings, dummy_modules):
        """Успешная загрузка модели (BuildModel мокируется для избежания тяжёлых вычислений)."""
        # Мокаем BuildModel, чтобы он возвращал dummy_modules
        mock_build = mocker.patch.object(repo, 'BuildModel', return_value=dummy_modules)
        # Мокаем ComputeCacheKey, чтобы ключ был предсказуемым
        mock_key = mocker.patch.object(repo, 'ComputeCacheKey', return_value='test_key')

        result = await repo.LoadModelAsync(dummy_settings)

        assert result is dummy_modules
        mock_build.assert_called_once_with(dummy_settings)
        mock_key.assert_called_once_with(dummy_settings)
        # Проверяем, что модель попала в кэш
        assert repo._model_cache['test_key'] is dummy_modules

    async def test_load_model_from_cache(self, mocker, repo, dummy_settings, dummy_modules):
        """Повторная загрузка должна брать модель из кэша."""
        # Предварительно заполняем кэш
        repo._model_cache['test_key'] = dummy_modules
        mock_key = mocker.patch.object(repo, 'ComputeCacheKey', return_value='test_key')
        mock_build = mocker.patch.object(repo, 'BuildModel')

        result = await repo.LoadModelAsync(dummy_settings)

        assert result is dummy_modules
        mock_key.assert_called_once_with(dummy_settings)
        mock_build.assert_not_called()

    async def test_load_model_build_error(self, mocker, repo, dummy_settings):
        """Ошибка при создании модели должна преобразовываться в DeepfakeDetectorLoadError."""
        mock_build = mocker.patch.object(repo, 'BuildModel', side_effect=RuntimeError("Build failed"))
        mock_key = mocker.patch.object(repo, 'ComputeCacheKey', return_value='test_key')

        with pytest.raises(DeepfakeDetectorLoadError) as excinfo:
            await repo.LoadModelAsync(dummy_settings)
        assert "Ошибка загрузки модели" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)

    # ------------------------------------------------------------------
    # Тесты для SaveModelAsync
    # ------------------------------------------------------------------
    async def test_save_model_success(self, repo, dummy_modules, temp_dir):
        """Успешное сохранение модели в файл."""
        file_path = temp_dir / "model.pth"
        await repo.SaveModelAsync(dummy_modules, str(file_path))
        assert file_path.exists()

        # Проверим, что файл можно загрузить через torch.load
        loaded = torch.load(file_path)
        assert isinstance(loaded, dict)
        # Должны быть ключи с префиксами модулей
        assert any(key.startswith('cnn.') for key in loaded.keys())

    async def test_save_model_already_exists(self, repo, dummy_modules, temp_dir):
        """Попытка сохранить в уже существующий файл."""
        file_path = temp_dir / "model.pth"
        # Создаём файл заранее
        file_path.touch()
        with pytest.raises(DeepfakeDetectorAlreadyExistsError) as excinfo:
            await repo.SaveModelAsync(dummy_modules, str(file_path))
        assert "Файл уже существует" in str(excinfo.value)

    async def test_save_model_torch_error(self, mocker, repo, dummy_modules, temp_dir):
        """Ошибка при вызове torch.save."""
        file_path = temp_dir / "model.pth"
        mock_save = mocker.patch('torch.save', side_effect=IOError("Disk full"))
        with pytest.raises(DeepfakeDetectorSaveError) as excinfo:
            await repo.SaveModelAsync(dummy_modules, str(file_path))
        assert "Ошибка сохранения модели" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, IOError)

    # ------------------------------------------------------------------
    # Тесты для DeleteModelAsync
    # ------------------------------------------------------------------
    async def test_delete_model_success(self, repo, temp_dir):
        """Успешное удаление файла."""
        file_path = temp_dir / "model.pth"
        file_path.touch()
        assert file_path.exists()
        await repo.DeleteModelAsync(str(file_path))
        assert not file_path.exists()

    async def test_delete_model_not_found(self, repo, temp_dir):
        """Удаление несуществующего файла."""
        file_path = temp_dir / "missing.pth"
        with pytest.raises(DeepfakeDetectorNotFoundError) as excinfo:
            await repo.DeleteModelAsync(str(file_path))
        assert "Файл не найден" in str(excinfo.value)

    # ------------------------------------------------------------------
    # Тесты для ModelExistsAsync
    # ------------------------------------------------------------------
    async def test_model_exists_true(self, repo, temp_dir):
        """Проверка существования файла (файл есть)."""
        file_path = temp_dir / "model.pth"
        file_path.touch()
        assert await repo.ModelExistsAsync(str(file_path)) is True

    async def test_model_exists_false(self, repo, temp_dir):
        """Проверка существования файла (файла нет)."""
        file_path = temp_dir / "missing.pth"
        assert await repo.ModelExistsAsync(str(file_path)) is False