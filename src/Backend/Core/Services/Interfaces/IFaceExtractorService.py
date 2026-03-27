# Core/Services/Interfaces/IFaceExtractorService.py

from abc import ABC, abstractmethod
from typing import List, Optional

from Backend.Core.Entities.Image import Image
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings


class IFaceExtractorService(ABC):
    """Сервис для извлечения лиц из изображений."""

    @abstractmethod
    async def GetSettingsAsync(self, path: str) -> FaceExtractorSettings:
        """
        Асинхронно загружает настройки экстрактора лиц из файла.

        Args:
            path: Путь к файлу настроек (JSON).

        Returns:
            FaceExtractorSettings: Объект настроек.

        Raises:
            FaceExtractorSettingsLoadError: Если не удалось загрузить настройки.
        """
        pass

    @abstractmethod
    async def ExtractFaceAsync(self, image: Image, settings: FaceExtractorSettings) -> Optional[Image]:
        """
        Асинхронно извлекает одно лицо из изображения.

        Args:
            image: Исходное доменное изображение.
            settings: Настройки экстрактора.

        Returns:
            Optional[Image]: Изображение с вырезанным лицом, приведённое к target_size.
                Если лицо не найдено, возвращает None.

        Raises:
            FaceExtractorExtractionError: Если произошла ошибка при обработке.
        """
        pass

    @abstractmethod
    async def ExtractAllFacesAsync(self, image: Image, settings: FaceExtractorSettings) -> List[Image]:
        """
        Асинхронно извлекает все лица из изображения.

        Args:
            image: Исходное доменное изображение.
            settings: Настройки экстрактора.

        Returns:
            List[Image]: Список изображений лиц (каждое приведено к target_size).
                Если лица не найдены, возвращает пустой список.

        Raises:
            FaceExtractorExtractionError: Если произошла ошибка при обработке.
        """
        pass