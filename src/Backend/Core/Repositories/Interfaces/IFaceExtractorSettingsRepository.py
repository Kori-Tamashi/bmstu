from abc import ABC, abstractmethod
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings

class IFaceExtractorSettingsRepository(ABC):
    """репозиторий настройка экстрактора лиц"""

    @abstractmethod
    async def GetFaceExtractorSettingsAsync(self, path: str) -> FaceExtractorSettings:
        """Получает настройки экстрактора лиц"""
        pass
