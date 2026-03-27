from Backend.Core.Repositories.Exceptions.ImageRepositoryExceptions import *
from Backend.Core.Repositories.Interfaces.IImageRepository import IImageRepository
from Backend.Core.Converters.ImageConverter import ImageConverter
from Backend.Core.Entities.Image import Image

from PIL import Image as PILImage, UnidentifiedImageError
from pathlib import Path
import asyncio


class ImageRepository(IImageRepository):
    """Репозиторий изображений"""

    async def GetImageAsync(self, path: str) -> Image:
        """Асинхронно загружает изображение по указанному пути и возвращает доменный объект Image.

        Args:
            path: Путь к файлу изображения.

        Returns:
            Image: Доменная сущность изображения.

        Raises:
            ImageNotFoundError: Если файл не существует.
            ImageLoadError: Если не удалось загрузить изображение (например, файл повреждён).
            InvalidImageFormatError: Если формат изображения не поддерживается.
        """

        def GetPilImage(path: str) -> PILImage.Image:
            """Синхронно загружает изображение с помощью Pillow."""
            with PILImage.open(path) as pil_image:
                if pil_image.mode not in ('RGB', 'L', 'RGBA'):
                    pil_image = pil_image.convert('RGB')
                return pil_image.copy()

        # Проверяем существование файла синхронно (быстрая операция)
        file_path = Path(path)
        if not file_path.exists():
            raise ImageNotFoundError(f"Файл не найден: {path}")
        if not file_path.is_file():
            raise ImageNotFoundError(f"Путь не является файлом: {path}")

        # Загружаем изображение в отдельном потоке, чтобы не блокировать event loop
        try:
            pil_image = await asyncio.to_thread(GetPilImage, path)
        except FileNotFoundError as e:
            raise ImageNotFoundError(f"Файл не найден: {path}") from e
        except UnidentifiedImageError as e:
            raise InvalidImageFormatError(
                f"Невозможно определить формат изображения: {path}") from e
        except Exception as e:
            raise ImageLoadError(f"Ошибка загрузки изображения: {path}") from e

        # Конвертируем PIL Image в доменный Image
        try:
            domain_image = ImageConverter.PillowToImage(pil_image)
        except ValueError as e:
            raise InvalidImageFormatError(f"Неподдерживаемый формат изображения: {e}") from e
        except Exception as e:
            raise ImageLoadError(f"Ошибка преобразования изображения: {path}") from e

        return domain_image