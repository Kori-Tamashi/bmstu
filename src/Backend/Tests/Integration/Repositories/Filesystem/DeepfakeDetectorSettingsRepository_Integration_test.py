# Tests/Integration/Repositories/Filesystem/DeepfakeDetectorSettingsRepositoryIntegrationTests.py

import pytest
import torch
import copy
from pathlib import Path
from typing import Dict, Any

from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Repositories.Exceptions.DeepfakeDetectorSettingsRepositoryExceptions import (
    DetectorConfigNotFoundError,
    DetectorConfigLoadError,
    InvalidDetectorConfigFormatError,
    DetectorConfigMissingKeysError,
)
from Backend.DataAccess.Repositories.Filesystem.DeepfakeDetectorSettingsRepository import (
    DeepfakeDetectorSettingsRepository,
)


@pytest.fixture
def repo() -> DeepfakeDetectorSettingsRepository:
    """Фикстура, возвращающая экземпляр репозитория."""
    return DeepfakeDetectorSettingsRepository()


@pytest.fixture
def checkpoints_dir(tmp_path) -> Path:
    dir_path = tmp_path / "checkpoints"
    dir_path.mkdir()

    # Базовый корректный чекпоинт
    base_checkpoint = {
        'model_config': {
            'cnn_model_name': 'efficientnet_b1',
            'vit_model_name': 'google/vit-base-patch16-224-in21k',
            'fusion_dim': 512,
            'dropout_rate': 0.3,
            'num_classes': 1,
            'threshold': 0.5,
            'input_size': (224, 224)
        },
        'model_state_dict': {'weight': torch.randn(10, 10)}
    }

    # Валидный
    torch.save(base_checkpoint, dir_path / "valid.pth")

    # Отсутствует model_config
    missing_config = copy.deepcopy(base_checkpoint)
    del missing_config['model_config']
    torch.save(missing_config, dir_path / "missing_config.pth")

    # Отсутствует model_state_dict
    missing_state_dict = copy.deepcopy(base_checkpoint)
    del missing_state_dict['model_state_dict']
    torch.save(missing_state_dict, dir_path / "missing_state_dict.pth")

    # Некорректный threshold
    invalid_threshold = copy.deepcopy(base_checkpoint)
    invalid_threshold['model_config']['threshold'] = 1.5
    torch.save(invalid_threshold, dir_path / "invalid_threshold.pth")

    # Некорректный dropout_rate
    invalid_dropout = copy.deepcopy(base_checkpoint)
    invalid_dropout['model_config']['dropout_rate'] = -0.1
    torch.save(invalid_dropout, dir_path / "invalid_dropout.pth")

    # Невалидный файл (текст)
    (dir_path / "not_a_pth.txt").write_text("This is not a checkpoint", encoding='utf-8')

    # Битый .pth (просто байты)
    (dir_path / "corrupt.pth").write_bytes(b"corrupted data")

    return dir_path


@pytest.mark.asyncio
class TestDeepfakeDetectorSettingsRepositoryIntegration:
    """Интеграционные тесты для DeepfakeDetectorSettingsRepository."""

    async def test_load_valid_checkpoint_success(self, repo, checkpoints_dir):
        """Успешная загрузка корректного чекпоинта."""
        path = checkpoints_dir / "valid.pth"
        settings = await repo.GetDeepfakeDetectorSettingsAsync(str(path))

        assert isinstance(settings, DeepfakeDetectorSettings)
        assert settings.cnn_model_name == 'efficientnet_b1'
        assert settings.vit_model_name == 'google/vit-base-patch16-224-in21k'
        assert settings.fusion_dim == 512
        assert settings.dropout_rate == 0.3
        assert settings.num_classes == 1
        assert settings.threshold == 0.5
        assert settings.input_size == (224, 224)
        assert 'weight' in settings.weights

    async def test_file_not_found(self, repo, checkpoints_dir):
        """Попытка загрузить несуществующий файл."""
        path = checkpoints_dir / "nonexistent.pth"
        with pytest.raises(DetectorConfigNotFoundError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "Файл чекпоинта не найден" in str(excinfo.value)

    async def test_path_is_directory(self, repo, checkpoints_dir):
        """Путь указывает на директорию, а не на файл."""
        with pytest.raises(DetectorConfigNotFoundError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(checkpoints_dir))
        assert "Путь не является файлом" in str(excinfo.value)

    async def test_missing_model_config_key(self, repo, checkpoints_dir):
        """Чекпоинт не содержит ключ model_config."""
        path = checkpoints_dir / "missing_config.pth"
        with pytest.raises(DetectorConfigMissingKeysError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "отсутствуют обязательные ключи: ['model_config']" in str(excinfo.value)

    async def test_missing_state_dict_key(self, repo, checkpoints_dir):
        """Чекпоинт не содержит ключ model_state_dict."""
        path = checkpoints_dir / "missing_state_dict.pth"
        with pytest.raises(DetectorConfigMissingKeysError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "отсутствуют обязательные ключи: ['model_state_dict']" in str(excinfo.value)

    async def test_invalid_threshold_value(self, repo, checkpoints_dir):
        """Значение threshold вне допустимого диапазона."""
        path = checkpoints_dir / "invalid_threshold.pth"
        with pytest.raises(ValueError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "threshold должен быть в диапазоне [0, 1]" in str(excinfo.value)

    async def test_invalid_dropout_value(self, repo, checkpoints_dir):
        """Значение dropout_rate вне допустимого диапазона."""
        path = checkpoints_dir / "invalid_dropout.pth"
        with pytest.raises(ValueError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "dropout_rate должен быть между 0 и 1" in str(excinfo.value)

    async def test_not_a_pth_file(self, repo, checkpoints_dir):
        """Файл не является .pth (простой текст)."""
        path = checkpoints_dir / "not_a_pth.txt"
        with pytest.raises(InvalidDetectorConfigFormatError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "Файл повреждён или имеет неверный формат" in str(excinfo.value)

    async def test_corrupt_pth_file(self, repo, checkpoints_dir):
        """Битый .pth файл."""
        path = checkpoints_dir / "corrupt.pth"
        with pytest.raises(InvalidDetectorConfigFormatError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(str(path))
        assert "Файл повреждён или имеет неверный формат" in str(excinfo.value)
