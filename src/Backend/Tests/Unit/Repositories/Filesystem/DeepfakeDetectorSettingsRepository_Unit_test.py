# Tests/Unit/Repositories/Filesystem/DeepfakeDetectorRepositoryTests.py

import pytest
from pytest_mock import MockerFixture
from pathlib import Path
import torch
import json
from typing import Dict, Any

from Backend.Core.Repositories.Exceptions.DeepfakeDetectorSettingsRepositoryExceptions import (
    DetectorConfigNotFoundError,
    DetectorConfigLoadError,
    InvalidDetectorConfigFormatError,
    DetectorConfigMissingKeysError,
)
from Backend.DataAccess.Repositories.Filesystem.DeepfakeDetectorSettingsRepository import DeepfakeDetectorSettingsRepository
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings


# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных
# ----------------------------------------------------------------------
class DeepfakeDetectorObjectMother:
    """Предоставляет готовые объекты и моки для тестов."""

    @staticmethod
    def valid_checkpoint() -> Dict[str, Any]:
        """Возвращает корректный чекпоинт."""
        return {
            'model_config': {
                'cnn_model_name': 'efficientnet_b1',
                'vit_model_name': 'google/vit-base-patch16-224-in21k',
                'fusion_dim': 512,
                'dropout_rate': 0.3,
                'num_classes': 1,
                'threshold': 0.5,
                'input_size': (224, 224)
            },
            'model_state_dict': {'weight1': torch.tensor([1.0])}
        }

    @staticmethod
    def valid_settings() -> DeepfakeDetectorSettings:
        """Возвращает корректный объект настроек."""
        return DeepfakeDetectorSettings(
            weights={'weight1': torch.tensor([1.0])},
            cnn_model_name='efficientnet_b1',
            vit_model_name='google/vit-base-patch16-224-in21k',
            fusion_dim=512,
            dropout_rate=0.3,
            num_classes=1,
            threshold=0.5,
            input_size=(224, 224)
        )

    @staticmethod
    def checkpoint_with_invalid_threshold() -> Dict[str, Any]:
        """Чекпоинт с некорректным значением threshold."""
        cp = DeepfakeDetectorObjectMother.valid_checkpoint()
        cp['model_config']['threshold'] = 1.5
        return cp

    @staticmethod
    def checkpoint_with_invalid_dropout() -> Dict[str, Any]:
        """Чекпоинт с некорректным dropout_rate."""
        cp = DeepfakeDetectorObjectMother.valid_checkpoint()
        cp['model_config']['dropout_rate'] = -0.1
        return cp

    @staticmethod
    def checkpoint_missing_config() -> Dict[str, Any]:
        """Чекпоинт без ключа 'model_config'."""
        cp = DeepfakeDetectorObjectMother.valid_checkpoint()
        del cp['model_config']
        return cp

    @staticmethod
    def checkpoint_missing_state_dict() -> Dict[str, Any]:
        """Чекпоинт без ключа 'model_state_dict'."""
        cp = DeepfakeDetectorObjectMother.valid_checkpoint()
        del cp['model_state_dict']
        return cp


# ----------------------------------------------------------------------
# Тесты репозитория
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestDeepfakeDetectorRepository:
    """Модульные тесты для DeepfakeDetectorSettingsRepository (лондонский стиль)."""

    async def test_get_settings_success(self, mocker: MockerFixture):
        """Успешная загрузка корректного чекпоинта."""
        # Arrange
        file_path = "model.pth"
        expected_checkpoint = DeepfakeDetectorObjectMother.valid_checkpoint()
        expected_settings = DeepfakeDetectorObjectMother.valid_settings()

        # Мокаем проверки файловой системы
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем torch.load
        mock_torch_load = mocker.patch('torch.load', return_value=expected_checkpoint)

        repo = DeepfakeDetectorSettingsRepository()

        # Act
        result = await repo.GetDeepfakeDetectorSettingsAsync(file_path)

        # Assert
        assert result == expected_settings
        mock_torch_load.assert_called_once_with(Path(file_path), map_location='cpu')

    async def test_get_settings_file_not_found(self, mocker: MockerFixture):
        """Файл не существует."""
        # Arrange
        file_path = "missing.pth"
        mocker.patch.object(Path, 'exists', return_value=False)

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(DetectorConfigNotFoundError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "Файл чекпоинта не найден" in str(excinfo.value)

    async def test_get_settings_path_is_directory(self, mocker: MockerFixture):
        """Путь является директорией, а не файлом."""
        # Arrange
        file_path = "models_dir"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=False)

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(DetectorConfigNotFoundError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "Путь не является файлом" in str(excinfo.value)

    async def test_get_settings_torch_load_os_error(self, mocker: MockerFixture):
        """Ошибка операционной системы при загрузке файла (например, PermissionError)."""
        # Arrange
        file_path = "model.pth"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем torch.load, чтобы он выбросил PermissionError
        mocker.patch('torch.load', side_effect=PermissionError("Access denied"))

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(DetectorConfigLoadError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "Не удалось загрузить чекпоинт" in str(excinfo.value)
        # Проверяем, что исходное исключение было завёрнуто
        assert isinstance(excinfo.value.__cause__, PermissionError)

    async def test_get_settings_invalid_format_error(self, mocker: MockerFixture):
        """torch.load выбрасывает исключение, указывающее на неверный формат (например, RuntimeError)."""
        # Arrange
        file_path = "corrupt.pth"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем torch.load, чтобы он выбросил RuntimeError (например, "Not a valid checkpoint")
        mocker.patch('torch.load', side_effect=RuntimeError("Not a valid checkpoint"))

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(InvalidDetectorConfigFormatError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "Файл повреждён или имеет неверный формат" in str(excinfo.value)

    async def test_get_settings_missing_model_config_key(self, mocker: MockerFixture):
        """Чекпоинт не содержит ключ 'model_config'."""
        # Arrange
        file_path = "model.pth"
        checkpoint = DeepfakeDetectorObjectMother.checkpoint_missing_config()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('torch.load', return_value=checkpoint)

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(DetectorConfigMissingKeysError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "отсутствуют обязательные ключи: ['model_config']" in str(excinfo.value)

    async def test_get_settings_missing_state_dict_key(self, mocker: MockerFixture):
        """Чекпоинт не содержит ключ 'model_state_dict'."""
        # Arrange
        file_path = "model.pth"
        checkpoint = DeepfakeDetectorObjectMother.checkpoint_missing_state_dict()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('torch.load', return_value=checkpoint)

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(DetectorConfigMissingKeysError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "отсутствуют обязательные ключи: ['model_state_dict']" in str(excinfo.value)

    async def test_get_settings_validation_error_threshold(self, mocker: MockerFixture):
        """Значение threshold в чекпоинте недопустимо (вызывает ValueError при создании настроек)."""
        # Arrange
        file_path = "model.pth"
        checkpoint = DeepfakeDetectorObjectMother.checkpoint_with_invalid_threshold()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('torch.load', return_value=checkpoint)

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(ValueError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "threshold должен быть в диапазоне [0, 1]" in str(excinfo.value)

    async def test_get_settings_validation_error_dropout(self, mocker: MockerFixture):
        """Значение dropout_rate в чекпоинте недопустимо."""
        # Arrange
        file_path = "model.pth"
        checkpoint = DeepfakeDetectorObjectMother.checkpoint_with_invalid_dropout()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('torch.load', return_value=checkpoint)

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(ValueError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "dropout_rate должен быть между 0 и 1" in str(excinfo.value)

    async def test_get_settings_unexpected_exception(self, mocker: MockerFixture):
        """
        torch.load выбрасывает неожиданное исключение (не входящее в список обрабатываемых).
        Должно быть преобразовано в DetectorConfigLoadError.
        """
        # Arrange
        file_path = "model.pth"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Например, KeyError – не в списке (EOFError, RuntimeError, ValueError, KeyError) – но KeyError входит!
        # Возьмём TypeError, который не входит.
        mocker.patch('torch.load', side_effect=TypeError("unsupported type"))

        repo = DeepfakeDetectorSettingsRepository()

        # Act & Assert
        with pytest.raises(DetectorConfigLoadError) as excinfo:
            await repo.GetDeepfakeDetectorSettingsAsync(file_path)
        assert "Не удалось загрузить чекпоинт" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, TypeError)