from abc import ABC, abstractmethod
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings

class IDeepfakeDetectorSettingsRepository(ABC):
    """Репозиторий настроек детектора дипфейков"""

    @abstractmethod
    async def GetDeepfakeDetectorSettingsAsync(self, path: str) -> DeepfakeDetectorSettings:
        """Получает настройки детектора дипфейков"""
        pass
