from abc import ABC, abstractmethod
from Backend.Core.Entities.Image import Image

class IImageService(ABC):
    """Сервис для операций с изображениями в предметной области.

    Предоставляет методы для загрузки, изменения размера и обрезки изображений,
    работая с доменной сущностью Image.
    """

    @abstractmethod
    async def GetImageAsync(self, path: str) -> Image:
        """
        Асинхронно загружает изображение по указанному пути.

        Args:
            path: Путь к файлу изображения.

        Returns:
            Image: Доменная сущность загруженного изображения.

        Raises:
            ImageNotFoundError: Если файл не существует.
            InvalidImageFormatError: Если формат не поддерживается.
        """
        pass

    @abstractmethod
    async def ResizeImageAsync(self, image: Image, new_width: int, new_height: int) -> Image:
        """
        Асинхронно изменяет размер изображения.

        Args:
            image: Исходное доменное изображение.
            new_width: Новая ширина.
            new_height: Новая высота.

        Returns:
            Image: Новое доменное изображение с изменённым размером.
        """
        pass


    @abstractmethod
    async def CropCenterSquareAsync(self, image: Image) -> Image:
        """
        Асинхронно вырезает квадрат по центру изображения.

        Сторона квадрата равна минимальной из ширины и высоты исходного изображения.

        Args:
            image: Исходное доменное изображение.

        Returns:
            Image: Квадратное изображение, вырезанное из центра.
        """
        pass
