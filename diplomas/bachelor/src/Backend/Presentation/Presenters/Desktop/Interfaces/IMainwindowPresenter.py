from abc import ABC, abstractmethod
from typing import Optional


class IMainwindowPresenter(ABC):
    """Интерфейс презентера главного окна."""

    @abstractmethod
    def SetView(self, view):
        """Устанавливает ссылку на представление."""
        pass

    @abstractmethod
    async def LoadImage(self, path: str):
        """Загружает изображение и запускает анализ."""
        pass

    @abstractmethod
    async def ExtractFaces(self):
        """Извлечение лиц из изображения"""
        pass

    @abstractmethod
    async def Analyze(self, face_index: Optional[int] = None):
        """Запускает анализ загруженного изображения."""
        pass

    @abstractmethod
    def SetThreshold(self, value: float):
        """Устанавливает порог вероятности."""
        pass

    @abstractmethod
    def SetImage(self, face_index: Optional[int] = None):
        """Устанавливает анализируемое изображение."""
        pass

    @abstractmethod
    def IsNoFaces(self):
        """Проверка наличия лиц"""
        pass

    @abstractmethod
    def Reset(self):
        """Сброс состояния"""
        pass