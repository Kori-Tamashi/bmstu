"""
Классы эквивалентности, выделенные в тестах ImageRepository.GetImageAsync:

1. Входной путь:
   - Корректный путь к существующему файлу с изображением.
   - Путь к несуществующему файлу.
   - Путь к существующей директории (не файлу).

2. Состояние файла и права доступа:
   - Файл доступен для чтения.
   - Файл существует, но нет прав доступа (например, PermissionError).
   - Файл повреждён или имеет неподдерживаемый формат (вызывает UnidentifiedImageError).

3. Формат (режим) изображения:
   - Изображение в режиме RGB (не требует конвертации).
   - Изображение в режиме L (grayscale) – должно быть сконвертировано в RGB.
   - Изображение в режиме RGBA – должно быть сконвертировано в RGB (альфа-канал отбрасывается).
   - Изображение в неподдерживаемом режиме (например, CMYK) – должно быть сконвертировано в RGB внутри репозитория (до передачи конвертеру).

4. Поведение конвертера ImageConverter.PillowToImage:
   - Конвертер успешно преобразует PIL Image в доменный Image.
   - Конвертер выбрасывает ValueError (например, неподдерживаемый режим) – должно быть преобразовано в InvalidImageFormatError.
   - Конвертер выбрасывает любое другое исключение (RuntimeError) – должно быть преобразовано в ImageLoadError.

5. Асинхронное выполнение:
   - Загрузка в отдельном потоке завершается успешно.
   - Загрузка в потоке вызывает исключение (покрывается через side_effect моков).

Каждый тест проверяет один из этих классов или их комбинацию, обеспечивая полноту покрытия различных сценариев.
"""

import pytest
from pytest_mock import MockerFixture
from pathlib import Path
from unittest.mock import MagicMock, mock_open
import numpy as np

from Backend.Core.Repositories.Exceptions.ImageRepositoryExceptions import (
    ImageNotFoundError,
    ImageLoadError,
    InvalidImageFormatError,
)
from Backend.DataAccess.Repositories.Filesystem.ImageRepository import ImageRepository
from Backend.Core.Converters.ImageConverter import ImageConverter
from Backend.Core.Entities.Image import Image as CoreImage
from PIL import Image as PILImage, UnidentifiedImageError


# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных
# ----------------------------------------------------------------------
class ImageObjectMother:
    """Предоставляет готовые объекты и моки для тестов."""

    @staticmethod
    def valid_numpy_array() -> np.ndarray:
        """Создаёт небольшой массив RGB 2x2."""
        return np.zeros((2, 2, 3), dtype=np.uint8)

    @staticmethod
    def valid_core_image() -> CoreImage:
        """Создаёт доменный Image с корректными данными."""
        return CoreImage(pixels=ImageObjectMother.valid_numpy_array())

    @staticmethod
    def create_mock_pil_image(mode: str = 'RGB') -> MagicMock:
        """
        Создаёт мок PIL Image с заданным режимом.
        Мок поддерживает контекстный менеджер, методы convert и copy.
        """
        mock_img = MagicMock(spec=PILImage.Image)
        mock_img.mode = mode
        mock_img.convert = MagicMock(return_value=mock_img)
        mock_img.copy = MagicMock(return_value=mock_img)
        # Поддержка контекстного менеджера
        mock_img.__enter__ = MagicMock(return_value=mock_img)
        mock_img.__exit__ = MagicMock(return_value=None)
        return mock_img

    @staticmethod
    def valid_mock_pil_image() -> MagicMock:
        """Мок PIL Image с режимом RGB."""
        return ImageObjectMother.create_mock_pil_image('RGB')


# ----------------------------------------------------------------------
# Тесты репозитория
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestImageRepository:
    """Модульные тесты для ImageRepository (лондонский стиль)."""

    async def test_get_image_success(self, mocker: MockerFixture):
        """Успешная загрузка корректного изображения."""
        # Arrange
        file_path = "test.jpg"
        mock_pil_image = ImageObjectMother.valid_mock_pil_image()
        expected_core_image = ImageObjectMother.valid_core_image()

        # Мокаем проверки файловой системы
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем открытие файла (PIL.Image.open)
        mock_open = mocker.patch.object(PILImage, 'open', return_value=mock_pil_image)

        # Мокаем конвертер
        mock_converter = mocker.patch.object(
            ImageConverter,
            'PillowToImage',
            return_value=expected_core_image
        )

        repo = ImageRepository()

        # Act
        result = await repo.GetImageAsync(file_path)

        # Assert
        assert result is expected_core_image
        mock_converter.assert_called_once_with(mock_pil_image)
        # Ожидаем вызов open с исходной строкой пути
        mock_open.assert_called_once_with(file_path)

    async def test_get_image_file_not_found(self, mocker: MockerFixture):
        """Файл не существует."""
        # Arrange
        file_path = "missing.jpg"
        mocker.patch.object(Path, 'exists', return_value=False)

        repo = ImageRepository()

        # Act & Assert
        with pytest.raises(ImageNotFoundError) as excinfo:
            await repo.GetImageAsync(file_path)
        assert "Файл не найден" in str(excinfo.value)

    async def test_get_image_path_is_directory(self, mocker: MockerFixture):
        """Путь является директорией, а не файлом."""
        # Arrange
        file_path = "some_dir"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=False)

        repo = ImageRepository()

        # Act & Assert
        with pytest.raises(ImageNotFoundError) as excinfo:
            await repo.GetImageAsync(file_path)
        assert "Путь не является файлом" in str(excinfo.value)

    async def test_get_image_open_os_error(self, mocker: MockerFixture):
        """Ошибка операционной системы при открытии файла (например, PermissionError)."""
        # Arrange
        file_path = "test.jpg"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        # Мокаем PIL.Image.open так, чтобы он выбросил PermissionError
        mocker.patch.object(PILImage, 'open', side_effect=PermissionError("Access denied"))

        repo = ImageRepository()

        # Act & Assert
        with pytest.raises(ImageLoadError) as excinfo:
            await repo.GetImageAsync(file_path)
        assert "Ошибка загрузки изображения" in str(excinfo.value)

    async def test_get_image_unidentified_image_error(self, mocker: MockerFixture):
        """Файл повреждён или имеет неподдерживаемый формат (PIL выбросит UnidentifiedImageError)."""
        # Arrange
        file_path = "corrupt.jpg"
        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)

        mocker.patch.object(
            PILImage,
            'open',
            side_effect=UnidentifiedImageError("cannot identify image file")
        )

        repo = ImageRepository()

        # Act & Assert
        with pytest.raises(InvalidImageFormatError) as excinfo:
            await repo.GetImageAsync(file_path)
        assert "Невозможно определить формат изображения" in str(excinfo.value)

    async def test_get_image_converter_value_error(self, mocker: MockerFixture):
        """Конвертер выбрасывает ValueError (неподдерживаемый режим PIL)."""
        # Arrange
        file_path = "test.jpg"
        mock_pil_image = ImageObjectMother.valid_mock_pil_image()

        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch.object(PILImage, 'open', return_value=mock_pil_image)

        # Мокаем конвертер так, чтобы он выбросил ValueError при вызове
        mock_converter = mocker.patch.object(ImageConverter, 'PillowToImage')
        mock_converter.side_effect = ValueError("Unsupported PIL image mode: CMYK")

        repo = ImageRepository()

        # Act & Assert
        with pytest.raises(InvalidImageFormatError) as excinfo:
            await repo.GetImageAsync(file_path)
        assert "Неподдерживаемый формат изображения" in str(excinfo.value)
        assert "Unsupported PIL image mode: CMYK" in str(excinfo.value)

    async def test_get_image_converter_other_exception(self, mocker: MockerFixture):
        """Конвертер выбрасывает непредвиденное исключение (должно быть обёрнуто в ImageLoadError)."""
        # Arrange
        file_path = "test.jpg"
        mock_pil_image = ImageObjectMother.valid_mock_pil_image()

        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch.object(PILImage, 'open', return_value=mock_pil_image)

        # Мокаем конвертер, чтобы он выбросил RuntimeError
        mock_converter = mocker.patch.object(ImageConverter, 'PillowToImage')
        mock_converter.side_effect = RuntimeError("Unexpected error")

        repo = ImageRepository()

        # Act & Assert
        with pytest.raises(ImageLoadError) as excinfo:
            await repo.GetImageAsync(file_path)
        assert "Ошибка преобразования изображения" in str(excinfo.value)
        # Проверяем, что исходное исключение было завёрнуто как причина
        assert isinstance(excinfo.value.__cause__, RuntimeError)

    async def test_get_image_pil_convert_called_for_non_standard_mode(self, mocker: MockerFixture):
        """Если PIL Image имеет нестандартный режим (например, CMYK), вызывается convert('RGB')."""
        # Arrange
        file_path = "test.cmyk"
        # Создаём мок с режимом CMYK (не входит в разрешённые)
        mock_pil_image = ImageObjectMother.create_mock_pil_image('CMYK')
        expected_core_image = ImageObjectMother.valid_core_image()

        mocker.patch.object(Path, 'exists', return_value=True)
        mocker.patch.object(Path, 'is_file', return_value=True)
        mocker.patch.object(PILImage, 'open', return_value=mock_pil_image)

        mock_converter = mocker.patch.object(
            ImageConverter,
            'PillowToImage',
            return_value=expected_core_image
        )

        repo = ImageRepository()

        # Act
        result = await repo.GetImageAsync(file_path)

        # Assert
        assert result is expected_core_image
        # Для CMYK должен быть вызван convert('RGB')
        mock_pil_image.convert.assert_called_once_with('RGB')
        mock_converter.assert_called_once_with(mock_pil_image)