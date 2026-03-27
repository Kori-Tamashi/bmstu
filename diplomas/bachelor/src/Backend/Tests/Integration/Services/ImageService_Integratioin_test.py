# Tests/Integration/Services/ImageServiceIntegrationTests.py

import pytest
from pathlib import Path
from PIL import Image, ImageDraw
import numpy as np

from Backend.DataAccess.Repositories.Filesystem.ImageRepository import ImageRepository
from Backend.BusinessLogic.Services.ImageService import ImageService
from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Services.Exceptions.ImageServiceExceptions import (
    ImageServiceLoadError,
    ImageServiceResizeError,
    ImageServiceCropError,
    InvalidImageDimensionsError,
)


@pytest.fixture
def image_repository():
    """Фикстура, возвращающая экземпляр ImageRepository."""
    return ImageRepository()


@pytest.fixture
def image_service(image_repository):
    """Фикстура, возвращающая экземпляр ImageService с реальным репозиторием."""
    return ImageService(repository=image_repository)


@pytest.fixture
def test_images_dir(tmp_path):
    """
    Фикстура создаёт временную директорию с набором тестовых изображений.
    Аналогична той, что используется в интеграционных тестах репозитория.
    """
    images_dir = tmp_path / "images"
    images_dir.mkdir()

    # RGB JPG (100x100, красный)
    rgb_img = Image.new('RGB', (100, 100), color='red')
    rgb_img.save(images_dir / "test_rgb.jpg")

    # Grayscale PNG (120x80, режим L, серый)
    gray_img = Image.new('L', (120, 80), color=128)
    gray_img.save(images_dir / "test_grayscale.png")

    # RGBA PNG (64x64, зелёный с прозрачностью + красный квадрат)
    rgba_img = Image.new('RGBA', (64, 64), color=(0, 255, 0, 128))
    draw = ImageDraw.Draw(rgba_img)
    draw.rectangle([10, 10, 54, 54], fill=(255, 0, 0, 200))
    rgba_img.save(images_dir / "test_rgba.png")

    # CMYK JPEG (150x150, синий)
    rgb_for_cmyk = Image.new('RGB', (150, 150), color='blue')
    cmyk_img = rgb_for_cmyk.convert('CMYK')
    cmyk_img.save(images_dir / "test_cmyk.jpg")

    # Маленькое RGB изображение (10x20, белый)
    small_img = Image.new('RGB', (10, 20), color='white')
    small_img.save(images_dir / "small.jpg")

    # Битый файл (не изображение)
    corrupt_file = images_dir / "corrupt.jpg"
    corrupt_file.write_text("This is not an image", encoding='utf-8')

    return images_dir


@pytest.mark.asyncio
class TestImageServiceIntegration:
    """Интеграционные тесты для ImageService."""

    # ------------------------------------------------------------------
    # Тесты для GetImageAsync
    # ------------------------------------------------------------------
    async def test_get_image_rgb_success(self, image_service, test_images_dir):
        """Успешная загрузка RGB JPG через сервис."""
        path = test_images_dir / "test_rgb.jpg"
        result = await image_service.GetImageAsync(str(path))

        assert isinstance(result, CoreImage)
        assert result.channels == 3
        assert result.width == 100
        assert result.height == 100
        assert result.pixels.dtype == np.uint8

    async def test_get_image_grayscale_converted(self, image_service, test_images_dir):
        """Загрузка grayscale PNG, ожидаем конвертацию в RGB."""
        path = test_images_dir / "test_grayscale.png"
        result = await image_service.GetImageAsync(str(path))

        assert result.channels == 3
        assert result.width == 120
        assert result.height == 80

    async def test_get_image_rgba_converted(self, image_service, test_images_dir):
        """Загрузка RGBA PNG, альфа-канал отбрасывается."""
        path = test_images_dir / "test_rgba.png"
        result = await image_service.GetImageAsync(str(path))

        assert result.channels == 3
        assert result.width == 64
        assert result.height == 64

    async def test_get_image_cmyk_converted(self, image_service, test_images_dir):
        """Загрузка CMYK JPEG, конвертация в RGB."""
        path = test_images_dir / "test_cmyk.jpg"
        result = await image_service.GetImageAsync(str(path))

        assert result.channels == 3
        assert result.width == 150
        assert result.height == 150

    async def test_get_image_not_found(self, image_service, tmp_path):
        """Попытка загрузить несуществующий файл."""
        path = tmp_path / "nonexistent.jpg"
        with pytest.raises(ImageServiceLoadError) as excinfo:
            await image_service.GetImageAsync(str(path))
        assert "Ошибка загрузки изображения" in str(excinfo.value)

    async def test_get_image_corrupt(self, image_service, test_images_dir):
        """Попытка загрузить битый файл (не изображение)."""
        path = test_images_dir / "corrupt.jpg"
        with pytest.raises(ImageServiceLoadError) as excinfo:
            await image_service.GetImageAsync(str(path))
        assert "Ошибка загрузки изображения" in str(excinfo.value)

    # ------------------------------------------------------------------
    # Тесты для ResizeImageAsync
    # ------------------------------------------------------------------
    async def test_resize_image_success(self, image_service, test_images_dir):
        """Успешное изменение размера изображения."""
        # Сначала загрузим изображение
        path = test_images_dir / "test_rgb.jpg"
        original = await image_service.GetImageAsync(str(path))

        new_width, new_height = 50, 75
        resized = await image_service.ResizeImageAsync(original, new_width, new_height)

        assert resized.width == new_width
        assert resized.height == new_height
        assert resized.channels == 3
        assert resized.pixels.dtype == np.uint8

    async def test_resize_image_invalid_dimensions(self, image_service, test_images_dir):
        """Передача некорректных размеров (<=0)."""
        path = test_images_dir / "test_rgb.jpg"
        original = await image_service.GetImageAsync(str(path))

        with pytest.raises(InvalidImageDimensionsError):
            await image_service.ResizeImageAsync(original, 0, 100)
        with pytest.raises(InvalidImageDimensionsError):
            await image_service.ResizeImageAsync(original, 100, -10)

    async def test_resize_image_error_propagation(self, image_service, test_images_dir, mocker):
        """Проверка, что ошибки внутри to_thread оборачиваются в ImageServiceResizeError."""
        path = test_images_dir / "test_rgb.jpg"
        original = await image_service.GetImageAsync(str(path))

        # Подменим ImageConverter.ImageToPillow, чтобы он выбросил исключение
        import Backend.Core.Converters.ImageConverter as conv
        mocker.patch.object(conv.ImageConverter, 'ImageToPillow', side_effect=ValueError("Conversion error"))

        with pytest.raises(ImageServiceResizeError) as excinfo:
            await image_service.ResizeImageAsync(original, 50, 50)
        assert "Ошибка изменения размера" in str(excinfo.value)

    # ------------------------------------------------------------------
    # Тесты для CropCenterSquareAsync
    # ------------------------------------------------------------------
    async def test_crop_center_square_width_gt_height(self, image_service, test_images_dir):
        """Обрезка квадрата из изображения, где ширина больше высоты."""
        path = test_images_dir / "test_rgb.jpg"  # 100x100 квадрат, но возьмём другое
        # Создадим специальное изображение 200x100
        special_dir = test_images_dir.parent / "special"
        special_dir.mkdir()
        img = Image.new('RGB', (200, 100), color='blue')
        img.save(special_dir / "wide.jpg")
        original = await image_service.GetImageAsync(str(special_dir / "wide.jpg"))

        cropped = await image_service.CropCenterSquareAsync(original)

        # Ожидаем квадрат 100x100
        assert cropped.width == 100
        assert cropped.height == 100
        assert cropped.channels == 3

    async def test_crop_center_square_height_gt_width(self, image_service, test_images_dir):
        """Обрезка квадрата из изображения, где высота больше ширины."""
        special_dir = test_images_dir.parent / "special"
        special_dir.mkdir()
        img = Image.new('RGB', (100, 200), color='green')
        img.save(special_dir / "tall.jpg")
        original = await image_service.GetImageAsync(str(special_dir / "tall.jpg"))

        cropped = await image_service.CropCenterSquareAsync(original)

        assert cropped.width == 100
        assert cropped.height == 100

    async def test_crop_center_square_already_square(self, image_service, test_images_dir):
        """Изображение уже квадратное – должно вернуть копию с теми же размерами."""
        path = test_images_dir / "test_rgb.jpg"  # 100x100
        original = await image_service.GetImageAsync(str(path))
        cropped = await image_service.CropCenterSquareAsync(original)

        assert cropped.width == 100
        assert cropped.height == 100
        # Можно проверить, что пиксели совпадают (но не обязательно)

    async def test_crop_center_square_error_propagation(self, image_service, test_images_dir, mocker):
        """Проверка оборачивания ошибок в ImageServiceCropError."""
        path = test_images_dir / "test_rgb.jpg"
        original = await image_service.GetImageAsync(str(path))

        import Backend.Core.Converters.ImageConverter as conv
        mocker.patch.object(conv.ImageConverter, 'ImageToPillow', side_effect=RuntimeError("Crop error"))

        with pytest.raises(ImageServiceCropError) as excinfo:
            await image_service.CropCenterSquareAsync(original)
        assert "Ошибка обрезки изображения" in str(excinfo.value)