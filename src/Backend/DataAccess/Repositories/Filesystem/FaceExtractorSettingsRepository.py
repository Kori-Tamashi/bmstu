from Backend.Core.Repositories.Exceptions.FaceExtractorExceptions import *
from Backend.Core.Repositories.Interfaces.IFaceExtractorSettingsRepository import IFaceExtractorSettingsRepository
from Backend.Core.Converters.FaceExtractorSettingsConverter import FaceExtractorSettingsConverter
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings

from typing import Any, Dict
from pathlib import Path
import asyncio
import json


class FaceExtractorSettingsRepository(IFaceExtractorSettingsRepository):
    """Репозиторий, загружающий настройки экстрактора лиц из JSON-файла."""

    async def GetFaceExtractorSettingsAsync(self, path: str) -> FaceExtractorSettings:
        """
        Асинхронно загружает настройки из JSON-файла и возвращает объект FaceExtractorSettings.

        Args:
            path: Путь к JSON-файлу с настройками.

        Returns:
            FaceExtractorSettings: Объект с настройками экстрактора лиц.

        Raises:
            FaceExtractorSettingsNotFoundError: Если файл не существует.
            InvalidFaceExtractorSettingsFormatError: Если файл не является валидным JSON или содержит неверные типы.
            FaceExtractorSettingsValidationError: Если значения настроек не проходят валидацию.
            FaceExtractorSettingsLoadError: При других ошибках загрузки.
        """
        def LoadJson(path: Path) -> Dict[str, Any]:
            """Синхронно загружает JSON-файл."""
            with open(path, 'r', encoding='utf-8') as f:
                return json.load(f)

        file_path = Path(path)

        # Проверка существования файла
        if not file_path.exists():
            raise FaceExtractorSettingsNotFoundError(f"Файл настроек не найден: {path}")
        if not file_path.is_file():
            raise FaceExtractorSettingsNotFoundError(f"Путь не является файлом: {path}")

        # Загрузка JSON в отдельном потоке
        try:
            data = await asyncio.to_thread(LoadJson, file_path)
        except FileNotFoundError as e:
            raise FaceExtractorSettingsNotFoundError(f"Файл не найден: {path}") from e
        except json.JSONDecodeError as e:
            raise InvalidFaceExtractorSettingsFormatError(
                f"Файл не является валидным JSON: {path}. Ошибка: {e}"
            ) from e
        except Exception as e:
            raise FaceExtractorSettingsLoadError(
                f"Не удалось загрузить файл настроек: {path}. Ошибка: {e}"
            ) from e

        # Преобразование данных в объект настроек с помощью конвертера
        try:
            settings = FaceExtractorSettingsConverter.DictToSettings(data)
        except ValueError as e:
            raise FaceExtractorSettingsValidationError(
                f"Некорректные значения в файле настроек: {e}"
            ) from e
        except TypeError as e:
            raise InvalidFaceExtractorSettingsFormatError(
                f"Неверный тип данных в файле настроек: {e}"
            ) from e

        return settings



