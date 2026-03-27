# Tests/Integration/Repositories/Filesystem/ImageRepositoryIntegrationTests.py

import pytest
from pathlib import Path
from PIL import Image, ImageDraw
import numpy as np

from Backend.DataAccess.Repositories.Filesystem.ImageRepository import ImageRepository
from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Repositories.Exceptions.ImageRepositoryExceptions import (
    ImageNotFoundError,
    InvalidImageFormatError
)


@pytest.fixture
def image_repository():
    """Фикстура, возвращающая экземпляр ImageRepository."""
    return ImageRepository()


@pytest.fixture
def test_images_dir(tmp_path):
    """
    Фикстура создаёт временную директорию с набором тестовых изображений
    разных форматов и цветовых режимов. Возвращает путь к этой директории.
    """
    images_dir = tmp_path / "images"
    images_dir.mkdir()

    # 1. RGB JPG (100x100, красный)
    rgb_img = Image.new('RGB', (100, 100), color='red')
    rgb_img.save(images_dir / "test_rgb.jpg")

    # 2. Grayscale PNG (120x80, режим L, серый)
    gray_img = Image.new('L', (120, 80), color=128)
    gray_img.save(images_dir / "test_grayscale.png")

    # 3. RGBA PNG (64x64, зелёный с прозрачностью + красный квадрат)
    rgba_img = Image.new('RGBA', (64, 64), color=(0, 255, 0, 128))
    draw = ImageDraw.Draw(rgba_img)
    draw.rectangle([10, 10, 54, 54], fill=(255, 0, 0, 200))
    rgba_img.save(images_dir / "test_rgba.png")

    # 4. CMYK JPEG (150x150, синий)
    rgb_for_cmyk = Image.new('RGB', (150, 150), color='blue')
    cmyk_img = rgb_for_cmyk.convert('CMYK')
    cmyk_img.save(images_dir / "test_cmyk.jpg")

    # 5. Маленькое RGB изображение (10x20, белый)
    small_img = Image.new('RGB', (10, 20), color='white')
    small_img.save(images_dir / "small.jpg")

    # 6. Битый файл (не изображение)
    corrupt_file = images_dir / "corrupt.jpg"
    corrupt_file.write_text("This is not an image", encoding='utf-8')

    return images_dir


@pytest.mark.asyncio
class TestImageRepositoryIntegration:
    """Интеграционные тесты для ImageRepository с реальными файлами."""

    async def test_load_rgb_jpg(self, image_repository, test_images_dir):
        """Загрузка RGB JPG изображения."""
        path = test_images_dir / "test_rgb.jpg"
        result = await image_repository.GetImageAsync(str(path))

        assert isinstance(result, CoreImage)
        assert result.channels == 3
        assert result.pixels.dtype == np.uint8
        assert result.width == 100
        assert result.height == 100

    async def test_load_grayscale_png(self, image_repository, test_images_dir):
        """Загрузка grayscale PNG (должен конвертироваться в RGB)."""
        path = test_images_dir / "test_grayscale.png"
        result = await image_repository.GetImageAsync(str(path))

        assert result.channels == 3
        assert result.width == 120
        assert result.height == 80
        assert result.pixels.shape[2] == 3

    async def test_load_rgba_png(self, image_repository, test_images_dir):
        """Загрузка RGBA PNG (альфа-канал должен быть отброшен)."""
        path = test_images_dir / "test_rgba.png"
        result = await image_repository.GetImageAsync(str(path))

        assert result.channels == 3
        assert result.width == 64
        assert result.height == 64
        assert result.pixels.shape[2] == 3

    async def test_load_cmyk_jpg(self, image_repository, test_images_dir):
        """Загрузка CMYK JPEG (должен конвертироваться в RGB)."""
        path = test_images_dir / "test_cmyk.jpg"
        result = await image_repository.GetImageAsync(str(path))

        assert result.channels == 3
        assert result.width == 150
        assert result.height == 150
        assert result.pixels.shape[2] == 3

    async def test_load_small_image(self, image_repository, test_images_dir):
        """Загрузка маленького изображения для проверки размеров."""
        path = test_images_dir / "small.jpg"
        result = await image_repository.GetImageAsync(str(path))

        assert result.width == 10
        assert result.height == 20
        assert result.channels == 3

    async def test_file_not_found(self, image_repository, tmp_path):
        """Попытка загрузить несуществующий файл."""
        with pytest.raises(ImageNotFoundError) as exc_info:
            await image_repository.GetImageAsync(str(tmp_path / "nonexistent.jpg"))
        assert "Файл не найден" in str(exc_info.value)

    async def test_corrupt_file(self, image_repository, test_images_dir):
        """Попытка загрузить битый файл (не изображение)."""
        path = test_images_dir / "corrupt.jpg"
        with pytest.raises(InvalidImageFormatError) as exc_info:
            await image_repository.GetImageAsync(str(path))
        assert "Невозможно определить формат изображения" in str(exc_info.value)