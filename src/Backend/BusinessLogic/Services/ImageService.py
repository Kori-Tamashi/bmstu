
import asyncio
from PIL import Image as PILImage

from Backend.Core.Entities.Image import Image
from Backend.Core.Repositories.Interfaces.IImageRepository import IImageRepository
from Backend.Core.Services.Interfaces.IImageService import IImageService
from Backend.Core.Services.Exceptions.ImageServiceExceptions import *
from Backend.Core.Converters.ImageConverter import ImageConverter


class ImageService(IImageService):
    """Реализация сервиса для работы с изображениями."""

    def __init__(self, repository: IImageRepository):
        """
        Инициализирует сервис с указанным репозиторием изображений.

        Args:
            repository: Репозиторий для загрузки изображений.
        """
        self._repository = repository

    async def GetImageAsync(self, path: str) -> Image:
        """
        Асинхронно загружает изображение по указанному пути.

        Args:
            path: Путь к файлу изображения.

        Returns:
            Image: Доменная сущность загруженного изображения.

        Raises:
            ImageServiceLoadError: Если произошла ошибка при загрузке изображения.
        """
        try:
            return await self._repository.GetImageAsync(path)
        except Exception as e:
            raise ImageServiceLoadError(f"Ошибка загрузки изображения: {e}") from e

    async def ResizeImageAsync(self, image: Image, new_width: int, new_height: int) -> Image:
        """
        Асинхронно изменяет размер изображения.

        Args:
            image: Исходное доменное изображение.
            new_width: Новая ширина.
            new_height: Новая высота.

        Returns:
            Image: Новое доменное изображение с изменённым размером.

        Raises:
            InvalidImageDimensionsError: Если new_width или new_height <= 0.
            ImageServiceResizeError: Если произошла ошибка при изменении размера.
        """
        def ResizeImage(image: Image, new_width: int, new_height: int) -> Image:
            """Синхронно изменяет размер изображения с помощью Pillow."""
            pil_image = ImageConverter.ImageToPillow(image)
            resized_pil = pil_image.resize((new_width, new_height), PILImage.Resampling.LANCZOS)
            return ImageConverter.PillowToImage(resized_pil)

        if new_width <= 0 or new_height <= 0:
            raise InvalidImageDimensionsError(
                f"Размеры должны быть положительными: {new_width}x{new_height}"
            )

        try:
            resized_image = await asyncio.to_thread(
                ResizeImage,
                image,
                new_width,
                new_height
            )
        except Exception as e:
            raise ImageServiceResizeError(f"Ошибка изменения размера: {e}") from e

        return resized_image


    async def CropCenterSquareAsync(self, image: Image) -> Image:
        """
        Асинхронно вырезает квадрат по центру изображения.

        Сторона квадрата равна минимальной из ширины и высоты исходного изображения.

        Args:
            image: Исходное доменное изображение.

        Returns:
            Image: Квадратное изображение, вырезанное из центра.

        Raises:
            ImageServiceCropError: Если произошла ошибка при обрезке.
        """
        def CropCentreSquare(image: Image) -> Image:
            """Синхронно вырезает квадрат по центру с помощью Pillow."""
            pil_image = ImageConverter.ImageToPillow(image)
            width, height = pil_image.size
            size = min(width, height)
            left = (width - size) // 2
            top = (height - size) // 2
            right = left + size
            bottom = top + size
            cropped_pil = pil_image.crop((left, top, right, bottom))
            return ImageConverter.PillowToImage(cropped_pil)

        try:
            cropped_image = await asyncio.to_thread(CropCentreSquare, image)
        except Exception as e:
            raise ImageServiceCropError(f"Ошибка обрезки изображения: {e}") from e

        return cropped_image
