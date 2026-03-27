# Tests/Unit/Repositories/Filesystem/DeepfakeDetectorRepository_Unit_test.py

import pytest
from pytest_mock import MockerFixture
from pathlib import Path
import torch
import pickle
import hashlib
from typing import Dict, Any

from Backend.Core.Repositories.Exceptions.DeepfakeDetectorRepositoryExceptions import (
    DeepfakeDetectorNotFoundError,
    DeepfakeDetectorLoadError,
    DeepfakeDetectorSaveError,
    DeepfakeDetectorAlreadyExistsError,
)
from Backend.DataAccess.Repositories.Filesystem.DeepfakeDetectorRepository import DeepfakeDetectorRepository
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
import timm
from transformers import ViTModel


# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных
# ----------------------------------------------------------------------
class DeepfakeDetectorObjectMother:
    """Предоставляет готовые объекты и моки для тестов репозитория моделей."""

    @staticmethod
    def valid_settings() -> DeepfakeDetectorSettings:
        """Возвращает корректный объект настроек."""
        return DeepfakeDetectorSettings(
            weights={'weight': torch.tensor([1.0])},
            cnn_model_name='efficientnet_b1',
            vit_model_name='google/vit-base-patch16-224-in21k',
            fusion_dim=512,
            dropout_rate=0.3,
            num_classes=1,
            threshold=0.5,
            input_size=(224, 224)
        )

    @staticmethod
    def valid_modules() -> Dict[str, Any]:
        """Возвращает словарь с модулями, имитирующий результат BuildModel."""
        device = torch.device('cpu')
        return {
            'cnn': torch.nn.Linear(10, 10).to(device).eval(),
            'vit': torch.nn.Linear(10, 10).to(device).eval(),
            'cnn_proj': torch.nn.Linear(10, 10).to(device).eval(),
            'vit_proj': torch.nn.Linear(10, 10).to(device).eval(),
            'classifier': torch.nn.Linear(10, 10).to(device).eval(),
            'device': device
        }

    @staticmethod
    def build_mock_modules(mocker: MockerFixture) -> Dict[str, Any]:
        """Создаёт моки для модулей, используемых в репозитории."""
        mock_cnn = mocker.MagicMock(spec=torch.nn.Module)
        mock_vit = mocker.MagicMock(spec=ViTModel)
        mock_cnn_proj = mocker.MagicMock(spec=torch.nn.Module)
        mock_vit_proj = mocker.MagicMock(spec=torch.nn.Module)
        mock_classifier = mocker.MagicMock(spec=torch.nn.Module)
        device = torch.device('cpu')
        return {
            'cnn': mock_cnn,
            'vit': mock_vit,
            'cnn_proj': mock_cnn_proj,
            'vit_proj': mock_vit_proj,
            'classifier': mock_classifier,
            'device': device
        }


# ----------------------------------------------------------------------
# Тесты репозитория
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestDeepfakeDetectorRepository:
    """Модульные тесты для DeepfakeDetectorRepository (лондонский стиль)."""

    async def test_load_model_success_new(self, mocker: MockerFixture):
        """Успешная загрузка модели (первый вызов, создание новой)."""
        # Arrange
        settings = DeepfakeDetectorObjectMother.valid_settings()
        expected_modules = DeepfakeDetectorObjectMother.valid_modules()

        # Мокаем BuildModel, чтобы он возвращал ожидаемые модули
        mock_build = mocker.patch.object(
            DeepfakeDetectorRepository,
            'BuildModel',
            return_value=expected_modules
        )
        # Мокаем ComputeCacheKey, чтобы он возвращал фиксированный ключ
        mock_key = mocker.patch.object(
            DeepfakeDetectorRepository,
            'ComputeCacheKey',
            return_value='test_cache_key'
        )

        repo = DeepfakeDetectorRepository()

        # Act
        result = await repo.LoadModelAsync(settings)

        # Assert
        assert result is expected_modules
        mock_key.assert_called_once_with(settings)
        mock_build.assert_called_once_with(settings)
        # Проверяем, что модель сохранилась в кэше
        assert repo._model_cache['test_cache_key'] is expected_modules

    async def test_load_model_from_cache(self, mocker: MockerFixture):
        """Загрузка модели из кэша при повторном вызове."""
        # Arrange
        settings = DeepfakeDetectorObjectMother.valid_settings()
        expected_modules = DeepfakeDetectorObjectMother.valid_modules()

        # Предварительно заполняем кэш
        repo = DeepfakeDetectorRepository()
        cache_key = 'test_cache_key'
        repo._model_cache[cache_key] = expected_modules

        # Мокаем ComputeCacheKey, чтобы он вернул тот же ключ
        mock_key = mocker.patch.object(
            DeepfakeDetectorRepository,
            'ComputeCacheKey',
            return_value=cache_key
        )
        # Мокаем BuildModel, чтобы убедиться, что он не вызывается
        mock_build = mocker.patch.object(DeepfakeDetectorRepository, 'BuildModel')

        # Act
        result = await repo.LoadModelAsync(settings)

        # Assert
        assert result is expected_modules
        mock_key.assert_called_once_with(settings)
        mock_build.assert_not_called()

    async def test_load_model_build_error(self, mocker: MockerFixture):
        """Ошибка при создании модели (исключение в BuildModel)."""
        # Arrange
        settings = DeepfakeDetectorObjectMother.valid_settings()

        # Мокаем BuildModel, чтобы он выбросил исключение
        mock_build = mocker.patch.object(
            DeepfakeDetectorRepository,
            'BuildModel',
            side_effect=RuntimeError("Build failed")
        )
        mock_key = mocker.patch.object(
            DeepfakeDetectorRepository,
            'ComputeCacheKey',
            return_value='test_key'
        )

        repo = DeepfakeDetectorRepository()

        # Act & Assert
        with pytest.raises(DeepfakeDetectorLoadError) as excinfo:
            await repo.LoadModelAsync(settings)
        assert "Ошибка загрузки модели" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)
        mock_key.assert_called_once_with(settings)
        mock_build.assert_called_once_with(settings)

    async def test_save_model_success(self, mocker: MockerFixture):
        """Успешное сохранение модели."""
        # Arrange
        path = "model.pth"
        modules = DeepfakeDetectorObjectMother.valid_modules()
        # Мокаем state_dict для каждого модуля
        for name, module in modules.items():
            if name != 'device' and hasattr(module, 'state_dict'):
                mocker.patch.object(module, 'state_dict', return_value={f"{name}_weight": torch.tensor([1.0])})

        # Мокаем Path.exists, чтобы файла не было
        mocker.patch.object(Path, 'exists', return_value=False)
        # Мокаем torch.save
        mock_torch_save = mocker.patch('torch.save')

        repo = DeepfakeDetectorRepository()

        # Act
        await repo.SaveModelAsync(modules, path)

        # Assert
        mock_torch_save.assert_called_once()
        args, kwargs = mock_torch_save.call_args
        assert isinstance(args[0], dict)  # state_dict
        assert args[1] == Path(path)

    async def test_save_model_already_exists(self, mocker: MockerFixture):
        """Попытка сохранить модель, когда файл уже существует."""
        # Arrange
        path = "model.pth"
        modules = DeepfakeDetectorObjectMother.valid_modules()

        # Мокаем Path.exists, чтобы файл существовал
        mocker.patch.object(Path, 'exists', return_value=True)

        repo = DeepfakeDetectorRepository()

        # Act & Assert
        with pytest.raises(DeepfakeDetectorAlreadyExistsError) as excinfo:
            await repo.SaveModelAsync(modules, path)
        assert "Файл уже существует" in str(excinfo.value)

    async def test_save_model_torch_error(self, mocker: MockerFixture):
        """Ошибка при сохранении через torch.save."""
        # Arrange
        path = "model.pth"
        modules = DeepfakeDetectorObjectMother.valid_modules()
        for name, module in modules.items():
            if name != 'device' and hasattr(module, 'state_dict'):
                mocker.patch.object(module, 'state_dict', return_value={})

        mocker.patch.object(Path, 'exists', return_value=False)
        mock_torch_save = mocker.patch('torch.save', side_effect=IOError("Disk full"))

        repo = DeepfakeDetectorRepository()

        # Act & Assert
        with pytest.raises(DeepfakeDetectorSaveError) as excinfo:
            await repo.SaveModelAsync(modules, path)
        assert "Ошибка сохранения модели" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, IOError)

    async def test_model_exists_true(self, mocker: MockerFixture):
        """Проверка существования файла (файл есть)."""
        # Arrange
        path = "model.pth"
        mocker.patch.object(Path, 'exists', return_value=True)

        repo = DeepfakeDetectorRepository()

        # Act
        result = await repo.ModelExistsAsync(path)

        # Assert
        assert result is True

    async def test_model_exists_false(self, mocker: MockerFixture):
        """Проверка существования файла (файла нет)."""
        # Arrange
        path = "missing.pth"
        mocker.patch.object(Path, 'exists', return_value=False)

        repo = DeepfakeDetectorRepository()

        # Act
        result = await repo.ModelExistsAsync(path)

        # Assert
        assert result is False
