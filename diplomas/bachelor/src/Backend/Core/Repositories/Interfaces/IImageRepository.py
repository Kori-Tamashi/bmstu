from abc import ABC, abstractmethod
from Backend.Core.Entities.Image import Image

class IImageRepository(ABC):
    """Репозиторий изображений"""

    @abstractmethod
    async def GetImageAsync(self, path: str) -> Image:
        """Возвращает изображение по пути"""
        pass
