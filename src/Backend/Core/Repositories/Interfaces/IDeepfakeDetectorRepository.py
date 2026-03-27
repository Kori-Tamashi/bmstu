# Core/Repositories/Interfaces/IModelRepository.py

from abc import ABC, abstractmethod
from typing import Dict, Any

from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings


class IDeepfakeDetectorRepository(ABC):
    """Репозиторий для управления моделями детекции дипфейков."""

    @abstractmethod
    async def LoadModelAsync(self, settings: DeepfakeDetectorSettings) -> Dict[str, Any]:
        """
        Асинхронно загружает модель на основе настроек.

        Возвращает словарь с компонентами модели и устройством.
        """
        pass

    @abstractmethod
    async def SaveModelAsync(self, model: Dict[str, Any], path: str) -> None:
        """
        Асинхронно сохраняет модель (веса) в файл.
        """
        pass

    @abstractmethod
    async def DeleteModelAsync(self, path: str) -> None:
        """
        Асинхронно удаляет файл модели.
        """
        pass

    @abstractmethod
    async def ModelExistsAsync(self, path: str) -> bool:
        """
        Проверяет существование файла модели.
        """
        pass