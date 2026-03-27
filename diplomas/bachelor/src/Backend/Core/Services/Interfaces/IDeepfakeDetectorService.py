from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Entities.Image import Image

from abc import ABC, abstractmethod
from typing import Tuple


class IDeepfakeDetectorService(ABC):
    """Сервис для детекции дипфейков на изображениях."""

    @abstractmethod
    async def GetSettingsAsync(self, path: str) -> DeepfakeDetectorSettings:
        """
        Асинхронно загружает настройки детектора из файла.

        Args:
            path: Путь к .pth файлу чекпоинта.

        Returns:
            DeepfakeDetectorSettings: Объект настроек и весов модели.

        Raises:
            SettingsLoadError: Если не удалось загрузить настройки.
        """
        pass

    @abstractmethod
    async def DetectAsync(
        self,
        image: Image,
        settings: DeepfakeDetectorSettings
    ) -> Tuple[float, bool]:
        """
        Асинхронно выполняет детекцию дипфейка на изображении лица.

        Args:
            image: Доменное изображение лица (предполагается, что лицо уже вырезано).
            settings: Настройки детектора, содержащие веса и параметры.

        Returns:
            Tuple[float, bool]: (вероятность подделки, бинарное решение).
                Бинарное решение определяется порогом settings.threshold.

        Raises:
            DetectionError: Если произошла ошибка при инференсе.
        """
        pass