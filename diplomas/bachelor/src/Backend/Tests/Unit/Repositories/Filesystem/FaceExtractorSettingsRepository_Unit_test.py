"""
Классы эквивалентности, выделенные в тестах FaceExtractorSettingsRepository.GetFaceExtractorSettingsAsync:

1. Входной путь:
   - Корректный путь к существующему файлу с настройками.
   - Путь к несуществующему файлу.
   - Путь к существующей директории (не файлу).

2. Доступность файла:
   - Файл доступен для чтения.
   - Файл существует, но нет прав доступа (например, PermissionError).

3. Формат содержимого файла:
   - Файл содержит валидный JSON.
   - Файл содержит невалидный JSON (вызывает JSONDecodeError).

4. Структура и типы данных в JSON:
   - JSON содержит все необходимые поля с корректными типами (списки для размеров, число для margin).
   - JSON содержит поля с некорректными значениями (margin вне [0,1]).
   - JSON содержит поля с неверными типами (например, строка вместо списка).
   - JSON содержит только часть полей (остальные должны быть заменены значениями по умолчанию).

5. Поведение конвертера FaceExtractorSettingsConverter.DictToSettings:
   - Конвертер успешно преобразует словарь в доменный объект.
   - Конвертер выбрасывает ValueError (например, при недопустимом значении margin) – должно быть преобразовано в FaceExtractorSettingsValidationError.
   - Конвертер выбрасывает TypeError (например, при неверном типе данных) – должно быть преобразовано в InvalidFaceExtractorSettingsFormatError.

Каждый тест покрывает один или несколько из этих классов, обеспечивая проверку всех основных сценариев работы репозитория.
"""

from Backend.Core.Repositories.Exceptions.FaceExtractorExceptions import *
from Backend.DataAccess.Repositories.Filesystem.FaceExtractorSettingsRepository import FaceExtractorSettingsRepository
from Backend.Core.Converters.FaceExtractorSettingsConverter import FaceExtractorSettingsConverter
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings

import json
import pytest
from pytest_mock import MockerFixture
from pathlib import Path
from unittest.mock import mock_open

# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных
# ----------------------------------------------------------------------
class FaceExtractorSettingsObjectMother:
    """Предоставляет готовые объекты и словари для тестов."""

    @staticmethod
    def valid_dict() -> dict:
        return {
            "target_size": [224, 224],
            "margin": 0.2,
            "min_face_size": [30, 30]
        }

    @staticmethod
    def valid_settings() -> FaceExtractorSettings:
        return FaceExtractorSettings(
            target_size=(224, 224),
            margin=0.2,
            min_face_size=(30, 30)
        )

    @staticmethod
    def invalid_dict() -> dict:
        # margin вне допустимого диапазона
        return {
            "target_size": [224, 224],
            "margin": 1.5,
            "min_face_size": [30, 30]
        }

    @staticmethod
    def type_error_dict() -> dict:
        # target_size не является списком чисел
        return {
            "target_size": "wrong",
            "margin": 0.2,
            "min_face_size": [30, 30]
        }

    @staticmethod
    def partial_dict() -> dict:
        # отсутствуют два поля – должны подставиться значения по умолчанию
        return {
            "target_size": [256, 256]
        }


# ----------------------------------------------------------------------
# Тесты репозитория
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestFaceExtractorSettingsRepository:
    """Модульные тесты для FaceExtractorSettingsRepository (лондонский стиль)."""

    async def test_get_settings_success(self, mocker: MockerFixture):
        """Успешная загрузка корректного JSON-файла."""
        # Arrange
        file_path = "test_config.json"
        expected_dict = FaceExtractorSettingsObjectMother.valid_dict()
        expected_settings = FaceExtractorSettingsObjectMother.valid_settings()

        # Мокаем проверки файловой системы
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем чтение файла и загрузку JSON
        mock_open_func = mock_open(read_data=json.dumps(expected_dict))
        mocker.patch('builtins.open', mock_open_func)
        mocker.patch('json.load', return_value=expected_dict)

        # Мокаем конвертер
        mock_converter = mocker.patch.object(
            FaceExtractorSettingsConverter,
            'DictToSettings',
            return_value=expected_settings
        )

        repo = FaceExtractorSettingsRepository()

        # Act
        result = await repo.GetFaceExtractorSettingsAsync(file_path)

        # Assert
        assert result == expected_settings
        mock_converter.assert_called_once_with(expected_dict)
        mock_open_func.assert_called_once_with(Path(file_path), 'r', encoding='utf-8')

    async def test_get_settings_file_not_found(self, mocker: MockerFixture):
        """Файл не существует."""
        # Arrange
        file_path = "nonexistent.json"
        mocker.patch.object(Path, 'exists', return_value=False)

        repo = FaceExtractorSettingsRepository()

        # Act & Assert
        with pytest.raises(FaceExtractorSettingsNotFoundError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(file_path)
        assert "Файл настроек не найден" in str(excinfo.value)

    async def test_get_settings_path_is_directory(self, mocker: MockerFixture):
        """Путь указывает на директорию, а не на файл."""
        # Arrange
        file_path = "config_dir"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=False)

        repo = FaceExtractorSettingsRepository()

        # Act & Assert
        with pytest.raises(FaceExtractorSettingsNotFoundError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(file_path)
        assert "Путь не является файлом" in str(excinfo.value)

    async def test_get_settings_json_decode_error(self, mocker: MockerFixture):
        """Файл содержит невалидный JSON."""
        # Arrange
        file_path = "invalid.json"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем открытие, но json.load бросает исключение
        mock_open_func = mock_open(read_data="invalid json")
        mocker.patch('builtins.open', mock_open_func)
        mocker.patch('json.load', side_effect=json.JSONDecodeError("Expecting value", "doc", 0))

        repo = FaceExtractorSettingsRepository()

        # Act & Assert
        with pytest.raises(InvalidFaceExtractorSettingsFormatError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(file_path)
        assert "Файл не является валидным JSON" in str(excinfo.value)

    async def test_get_settings_other_load_error(self, mocker: MockerFixture):
        """Возникает ошибка ввода-вывода (например, PermissionError)."""
        # Arrange
        file_path = "test.json"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # open бросает PermissionError
        mocker.patch('builtins.open', side_effect=PermissionError("Access denied"))

        repo = FaceExtractorSettingsRepository()

        # Act & Assert
        with pytest.raises(FaceExtractorSettingsLoadError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(file_path)
        assert "Не удалось загрузить файл настроек" in str(excinfo.value)

    async def test_get_settings_validation_error(self, mocker: MockerFixture):
        """Конвертер выбрасывает ValueError (некорректные значения)."""
        # Arrange
        file_path = "test.json"
        invalid_dict = FaceExtractorSettingsObjectMother.invalid_dict()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('builtins.open', mock_open(read_data=json.dumps(invalid_dict)))
        mocker.patch('json.load', return_value=invalid_dict)

        mock_converter = mocker.patch.object(FaceExtractorSettingsConverter, 'DictToSettings')
        mock_converter.side_effect = ValueError("margin must be between 0 and 1")

        repo = FaceExtractorSettingsRepository()

        # Act & Assert
        with pytest.raises(FaceExtractorSettingsValidationError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(file_path)
        assert "Некорректные значения" in str(excinfo.value)
        assert "margin must be between 0 and 1" in str(excinfo.value)

    async def test_get_settings_type_error(self, mocker: MockerFixture):
        """Конвертер выбрасывает TypeError (неверный тип данных)."""
        # Arrange
        file_path = "test.json"
        type_error_dict = FaceExtractorSettingsObjectMother.type_error_dict()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('builtins.open', mock_open(read_data=json.dumps(type_error_dict)))
        mocker.patch('json.load', return_value=type_error_dict)

        mock_converter = mocker.patch.object(FaceExtractorSettingsConverter, 'DictToSettings')
        mock_converter.side_effect = TypeError("expected tuple or list")

        repo = FaceExtractorSettingsRepository()

        # Act & Assert
        with pytest.raises(InvalidFaceExtractorSettingsFormatError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(file_path)
        assert "Неверный тип данных" in str(excinfo.value)

    async def test_get_settings_with_defaults(self, mocker: MockerFixture):
        """В JSON отсутствуют некоторые поля – используются значения по умолчанию."""
        # Arrange
        file_path = "test.json"
        partial_dict = FaceExtractorSettingsObjectMother.partial_dict()
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch('builtins.open', mock_open(read_data=json.dumps(partial_dict)))
        mocker.patch('json.load', return_value=partial_dict)

        # Ожидаемый объект с подставленными дефолтными значениями
        expected_settings = FaceExtractorSettings(
            target_size=(256, 256),
            margin=0.2,
            min_face_size=(30, 30)
        )

        mock_converter = mocker.patch.object(
            FaceExtractorSettingsConverter,
            'DictToSettings',
            return_value=expected_settings
        )

        repo = FaceExtractorSettingsRepository()

        # Act
        result = await repo.GetFaceExtractorSettingsAsync(file_path)

        # Assert
        assert result == expected_settings
        mock_converter.assert_called_once_with(partial_dict)