# Tests/Unit/Services/ImageService_Unit_test.py

import pytest
from pytest_mock import MockerFixture
from PIL import Image as PILImage
import numpy as np

from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Repositories.Interfaces.IImageRepository import IImageRepository
from Backend.BusinessLogic.Services.ImageService import ImageService
from Backend.Core.Services.Exceptions.ImageServiceExceptions import (
    ImageServiceLoadError,
    ImageServiceResizeError,
    ImageServiceCropError,
    InvalidImageDimensionsError,
)
from Backend.Core.Converters.ImageConverter import ImageConverter


# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных и моков
# ----------------------------------------------------------------------
class ImageObjectMother:
    """Предоставляет готовые объекты и моки для тестов сервиса."""

    @staticmethod
    def create_core_image(width: int = 100, height: int = 100, channels: int = 3) -> CoreImage:
        """Создаёт доменное изображение с заданными размерами."""
        pixels = np.zeros((height, width, channels), dtype=np.uint8)
        return CoreImage(pixels=pixels)

    @staticmethod
    def create_mock_pil_image(mocker: MockerFixture, size=(100, 100)) -> PILImage.Image:
        """Создаёт мок PIL Image с заданным размером."""
        mock_img = mocker.MagicMock(spec=PILImage.Image)
        mock_img.size = size
        # Мокаем метод resize, чтобы он возвращал другой мок
        mock_img.resize.return_value = mocker.MagicMock(spec=PILImage.Image, size=size)
        # Мокаем метод crop, возвращаем мок
        mock_img.crop.return_value = mocker.MagicMock(spec=PILImage.Image, size=size)
        return mock_img


# ----------------------------------------------------------------------
# Тесты сервиса
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestImageService:
    """Модульные тесты для ImageService (лондонский стиль)."""

    @pytest.fixture
    def mock_repository(self, mocker: MockerFixture):
        """Фикстура для мока репозитория."""
        return mocker.MagicMock(spec=IImageRepository)

    @pytest.fixture
    def service(self, mock_repository):
        """Фикстура для сервиса с замоканным репозиторием."""
        return ImageService(repository=mock_repository)

    @pytest.fixture(autouse=True)
    def mock_converter(self, mocker: MockerFixture):
        """Фикстура для мока статических методов ImageConverter."""
        mocker.patch.object(ImageConverter, 'ImageToPillow', autospec=True)
        mocker.patch.object(ImageConverter, 'PillowToImage', autospec=True)

    # ------------------------------------------------------------------
    # Тесты для GetImageAsync
    # ------------------------------------------------------------------
    async def test_get_image_success(self, service, mock_repository, mocker: MockerFixture):
        """Успешное получение изображения через репозиторий."""
        # Arrange
        path = "test.jpg"
        expected_image = ImageObjectMother.create_core_image()
        mock_repository.GetImageAsync.return_value = expected_image

        # Act
        result = await service.GetImageAsync(path)

        # Assert
        assert result is expected_image
        mock_repository.GetImageAsync.assert_called_once_with(path)

    async def test_get_image_repository_error(self, service, mock_repository, mocker: MockerFixture):
        """Ошибка репозитория при загрузке."""
        # Arrange
        path = "test.jpg"
        mock_repository.GetImageAsync.side_effect = Exception("Repo error")

        # Act & Assert
        with pytest.raises(ImageServiceLoadError) as excinfo:
            await service.GetImageAsync(path)
        assert "Ошибка загрузки изображения" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, Exception)

    # ------------------------------------------------------------------
    # Тесты для ResizeImageAsync
    # ------------------------------------------------------------------
    async def test_resize_success(self, service, mocker: MockerFixture):
        """Успешное изменение размера."""
        # Arrange
        input_image = ImageObjectMother.create_core_image(200, 200)
        new_width, new_height = 100, 150

        # Мокаем конвертацию: ImageToPillow -> PIL, PillowToImage -> новое CoreImage
        mock_pil_input = ImageObjectMother.create_mock_pil_image(mocker, size=(200, 200))
        mock_pil_resized = ImageObjectMother.create_mock_pil_image(mocker, size=(new_width, new_height))
        expected_output = ImageObjectMother.create_core_image(new_width, new_height)

        ImageConverter.ImageToPillow.return_value = mock_pil_input
        mock_pil_input.resize.return_value = mock_pil_resized
        ImageConverter.PillowToImage.return_value = expected_output

        # Act
        result = await service.ResizeImageAsync(input_image, new_width, new_height)

        # Assert
        assert result is expected_output
        ImageConverter.ImageToPillow.assert_called_once_with(input_image)
        mock_pil_input.resize.assert_called_once_with((new_width, new_height), PILImage.Resampling.LANCZOS)
        ImageConverter.PillowToImage.assert_called_once_with(mock_pil_resized)

    async def test_resize_invalid_dimensions(self, service, mocker: MockerFixture):
        """Переданы некорректные размеры (<=0)."""
        input_image = ImageObjectMother.create_core_image()

        with pytest.raises(InvalidImageDimensionsError) as excinfo:
            await service.ResizeImageAsync(input_image, -10, 100)
        assert "Размеры должны быть положительными" in str(excinfo.value)

        with pytest.raises(InvalidImageDimensionsError) as excinfo:
            await service.ResizeImageAsync(input_image, 100, 0)
        assert "Размеры должны быть положительными" in str(excinfo.value)

    async def test_resize_conversion_error(self, service, mocker: MockerFixture):
        """Ошибка при конвертации (например, ImageToPillow бросает исключение)."""
        input_image = ImageObjectMother.create_core_image()
        ImageConverter.ImageToPillow.side_effect = ValueError("Conversion error")

        with pytest.raises(ImageServiceResizeError) as excinfo:
            await service.ResizeImageAsync(input_image, 100, 100)
        assert "Ошибка изменения размера" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, ValueError)

    # ------------------------------------------------------------------
    # Тесты для CropCenterSquareAsync
    # ------------------------------------------------------------------
    async def test_crop_center_square_success(self, service, mocker: MockerFixture):
        """Успешная обрезка квадрата по центру."""
        # Arrange
        input_image = ImageObjectMother.create_core_image(300, 200)  # ширина больше высоты
        expected_output = ImageObjectMother.create_core_image(200, 200)  # квадрат 200x200

        mock_pil_input = ImageObjectMother.create_mock_pil_image(mocker, size=(300, 200))
        mock_pil_cropped = ImageObjectMother.create_mock_pil_image(mocker, size=(200, 200))

        ImageConverter.ImageToPillow.return_value = mock_pil_input
        mock_pil_input.crop.return_value = mock_pil_cropped
        ImageConverter.PillowToImage.return_value = expected_output

        # Act
        result = await service.CropCenterSquareAsync(input_image)

        # Assert
        assert result is expected_output
        ImageConverter.ImageToPillow.assert_called_once_with(input_image)
        # Проверим, что crop вызван с правильными координатами
        # size = 200, left = (300-200)//2 = 50, top = 0, right = 250, bottom = 200
        mock_pil_input.crop.assert_called_once_with((50, 0, 250, 200))
        ImageConverter.PillowToImage.assert_called_once_with(mock_pil_cropped)

    async def test_crop_center_square_width_less_than_height(self, service, mocker: MockerFixture):
        """Случай, когда ширина меньше высоты."""
        input_image = ImageObjectMother.create_core_image(200, 300)
        expected_output = ImageObjectMother.create_core_image(200, 200)

        mock_pil_input = ImageObjectMother.create_mock_pil_image(mocker, size=(200, 300))
        mock_pil_cropped = ImageObjectMother.create_mock_pil_image(mocker, size=(200, 200))

        ImageConverter.ImageToPillow.return_value = mock_pil_input
        mock_pil_input.crop.return_value = mock_pil_cropped
        ImageConverter.PillowToImage.return_value = expected_output

        await service.CropCenterSquareAsync(input_image)

        # left = 0, top = (300-200)//2 = 50, right = 200, bottom = 250
        mock_pil_input.crop.assert_called_once_with((0, 50, 200, 250))

    async def test_crop_center_square_square_image(self, service, mocker: MockerFixture):
        """Изображение уже квадратное."""
        input_image = ImageObjectMother.create_core_image(200, 200)
        expected_output = ImageObjectMother.create_core_image(200, 200)

        mock_pil_input = ImageObjectMother.create_mock_pil_image(mocker, size=(200, 200))
        mock_pil_cropped = ImageObjectMother.create_mock_pil_image(mocker, size=(200, 200))

        ImageConverter.ImageToPillow.return_value = mock_pil_input
        mock_pil_input.crop.return_value = mock_pil_cropped
        ImageConverter.PillowToImage.return_value = expected_output

        await service.CropCenterSquareAsync(input_image)

        # left = 0, top = 0, right = 200, bottom = 200
        mock_pil_input.crop.assert_called_once_with((0, 0, 200, 200))

    async def test_crop_conversion_error(self, service, mocker: MockerFixture):
        """Ошибка при конвертации во время обрезки."""
        input_image = ImageObjectMother.create_core_image()
        ImageConverter.ImageToPillow.side_effect = RuntimeError("Conversion error")

        with pytest.raises(ImageServiceCropError) as excinfo:
            await service.CropCenterSquareAsync(input_image)
        assert "Ошибка обрезки изображения" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)