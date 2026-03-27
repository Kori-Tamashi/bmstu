# Tests/Unit/Services/FaceExtractorService_Unit_test.py

import pytest
from pytest_mock import MockerFixture
import numpy as np
from typing import List, Optional

from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings
from Backend.Core.Repositories.Interfaces.IFaceExtractorSettingsRepository import IFaceExtractorSettingsRepository
from Backend.BusinessLogic.Services.FaceExtractorService import FaceExtractorService
from Backend.Core.Services.Exceptions.FaceExtractorServiceExceptions import (
    FaceExtractorSettingsLoadError,
    FaceExtractorExtractionError,
)
from Backend.Core.Converters.ImageConverter import ImageConverter


# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных
# ----------------------------------------------------------------------
class FaceExtractorObjectMother:
    """Предоставляет готовые объекты для тестов сервиса."""

    @staticmethod
    def create_core_image(height: int = 100, width: int = 100, channels: int = 3) -> CoreImage:
        pixels = np.zeros((height, width, channels), dtype=np.uint8)
        return CoreImage(pixels=pixels)

    @staticmethod
    def create_settings(
        target_size=(224, 224),
        margin=0.2,
        min_face_size=(30, 30)
    ) -> FaceExtractorSettings:
        return FaceExtractorSettings(
            target_size=target_size,
            margin=margin,
            min_face_size=min_face_size
        )

    @staticmethod
    def create_face_arrays(count: int, size: tuple = (100, 100)) -> List[np.ndarray]:
        """Создаёт список numpy-массивов, имитирующих лица."""
        return [np.zeros((size[0], size[1], 3), dtype=np.uint8) for _ in range(count)]


# ----------------------------------------------------------------------
# Фикстуры
# ----------------------------------------------------------------------
@pytest.fixture
def mock_settings_repository(mocker: MockerFixture):
    """Мок репозитория настроек."""
    return mocker.MagicMock(spec=IFaceExtractorSettingsRepository)


@pytest.fixture
def mock_cv2(mocker: MockerFixture):
    """Мок модуля cv2."""
    return mocker.patch('Backend.BusinessLogic.Services.FaceExtractorService.cv2')


@pytest.fixture
def mock_converter(mocker: MockerFixture):
    """Мок статических методов ImageConverter."""
    mocker.patch.object(ImageConverter, 'PillowToImage', autospec=True)
    # Можно вернуть сам мок, если нужно проверить вызовы
    return ImageConverter.PillowToImage


@pytest.fixture
def service(mock_settings_repository) -> FaceExtractorService:
    """Сервис с замоканным репозиторием."""
    return FaceExtractorService(settings_repository=mock_settings_repository)


@pytest.mark.asyncio
class TestFaceExtractorService:
    """Модульные тесты для FaceExtractorService (лондонский стиль)."""

    # ------------------------------------------------------------------
    # Тесты для GetSettingsAsync
    # ------------------------------------------------------------------
    async def test_get_settings_success(
        self,
        service: FaceExtractorService,
        mock_settings_repository: IFaceExtractorSettingsRepository,
    ):
        """Успешное получение настроек."""
        # Arrange
        path = "settings.json"
        expected_settings = FaceExtractorObjectMother.create_settings()
        mock_settings_repository.GetFaceExtractorSettingsAsync.return_value = expected_settings

        # Act
        result = await service.GetSettingsAsync(path)

        # Assert
        assert result is expected_settings
        mock_settings_repository.GetFaceExtractorSettingsAsync.assert_called_once_with(path)

    async def test_get_settings_repository_error(
        self,
        service: FaceExtractorService,
        mock_settings_repository: IFaceExtractorSettingsRepository,
    ):
        """Ошибка репозитория при загрузке настроек."""
        # Arrange
        path = "settings.json"
        mock_settings_repository.GetFaceExtractorSettingsAsync.side_effect = Exception("Repo error")

        # Act & Assert
        with pytest.raises(FaceExtractorSettingsLoadError) as excinfo:
            await service.GetSettingsAsync(path)
        assert "Ошибка загрузки настроек" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, Exception)

    # ------------------------------------------------------------------
    # Тесты для ExtractFaceAsync
    # ------------------------------------------------------------------
    async def test_extract_face_success_one_face(
        self,
        service: FaceExtractorService,
        mock_cv2,
        mock_converter,
    ):
        """Успешное извлечение одного лица (самое большое)."""
        # Arrange
        input_image = FaceExtractorObjectMother.create_core_image(200, 200)
        settings = FaceExtractorObjectMother.create_settings()

        # Мокаем OpenCV
        mock_gray = np.zeros((200, 200), dtype=np.uint8)
        mock_cv2.cvtColor.return_value = mock_gray

        # Мокаем CascadeClassifier
        mock_cascade = mock_cv2.CascadeClassifier.return_value
        # Возвращаем координаты двух лиц: (50,50,80,80) и (20,20,120,120) (второе больше)
        mock_cascade.detectMultiScale.return_value = np.array([
            [50, 50, 80, 80],
            [20, 20, 120, 120]
        ])

        # Мокаем cv2.resize для каждого лица – возвращаем разные массивы
        def mock_resize(face_roi, dsize, **kwargs):
            # Создаём массив с уникальным содержимым, чтобы различать лица
            h, w = dsize
            return np.full((w, h, 3), np.random.randint(0, 255), dtype=np.uint8)
        mock_cv2.resize.side_effect = mock_resize

        # Мокаем конвертер – будем возвращать CoreImage для каждого лица
        # Но нас интересует, что будет возвращено из метода ExtractFaceAsync.
        # В методе вызывается ImageConverter.PillowToImage для largest_face.
        # Мы можем проверить, что конвертер вызывается один раз с правильным массивом (для самого большого лица).
        # Для простоты подставим ожидаемый результат.
        expected_core_image = FaceExtractorObjectMother.create_core_image(
            settings.target_size[1], settings.target_size[0]
        )
        mock_converter.return_value = expected_core_image

        # Act
        result = await service.ExtractFaceAsync(input_image, settings)

        # Assert
        assert result is expected_core_image

        # Проверяем вызовы OpenCV
        mock_cv2.cvtColor.assert_called_once_with(input_image.pixels, mock_cv2.COLOR_RGB2GRAY)
        mock_cv2.CascadeClassifier.assert_called_once_with(
            mock_cv2.data.haarcascades + 'haarcascade_frontalface_default.xml'
        )
        mock_cascade.detectMultiScale.assert_called_once_with(
            mock_gray,
            scaleFactor=1.15,
            minNeighbors=8,
            minSize=settings.min_face_size
        )
        # Должно быть два вызова resize (по одному на каждое лицо), но конвертер вызывается только для largest
        assert mock_cv2.resize.call_count == 2
        # Проверяем, что конвертер вызван один раз (для самого большого лица)
        mock_converter.assert_called_once()
        # Можно проверить, что переданный в конвертер массив соответствует размеру target_size
        args, _ = mock_converter.call_args
        assert args[0].size == (settings.target_size[1], settings.target_size[0])

    async def test_extract_face_no_faces(
        self,
        service: FaceExtractorService,
        mock_cv2,
        mock_converter,
    ):
        """Когда лица не найдены, возвращается None."""
        # Arrange
        input_image = FaceExtractorObjectMother.create_core_image()
        settings = FaceExtractorObjectMother.create_settings()

        mock_gray = np.zeros((100, 100), dtype=np.uint8)
        mock_cv2.cvtColor.return_value = mock_gray
        mock_cascade = mock_cv2.CascadeClassifier.return_value
        mock_cascade.detectMultiScale.return_value = np.array([])  # пустой список

        # Act
        result = await service.ExtractFaceAsync(input_image, settings)

        # Assert
        assert result is None
        mock_converter.assert_not_called()

    async def test_extract_face_detection_error(
        self,
        service: FaceExtractorService,
        mock_cv2,
    ):
        """Ошибка в процессе детекции (например, OpenCV бросает исключение)."""
        # Arrange
        input_image = FaceExtractorObjectMother.create_core_image()
        settings = FaceExtractorObjectMother.create_settings()

        # Симулируем ошибку при вызове cvtColor
        mock_cv2.cvtColor.side_effect = RuntimeError("OpenCV error")

        # Act & Assert
        with pytest.raises(FaceExtractorExtractionError) as excinfo:
            await service.ExtractFaceAsync(input_image, settings)
        assert "Ошибка детекции лиц" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)

    # ------------------------------------------------------------------
    # Тесты для ExtractAllFacesAsync
    # ------------------------------------------------------------------
    async def test_extract_all_faces_success_multiple_faces(
        self,
        service: FaceExtractorService,
        mock_cv2,
        mock_converter,
    ):
        """Успешное извлечение нескольких лиц."""
        # Arrange
        input_image = FaceExtractorObjectMother.create_core_image(200, 200)
        settings = FaceExtractorObjectMother.create_settings()
        face_count = 3

        mock_gray = np.zeros((200, 200), dtype=np.uint8)
        mock_cv2.cvtColor.return_value = mock_gray
        mock_cascade = mock_cv2.CascadeClassifier.return_value
        # Создаём массив координат лиц (произвольные)
        faces_coords = np.array([[10, 10, 40, 40], [50, 50, 60, 60], [100, 100, 80, 80]])
        mock_cascade.detectMultiScale.return_value = faces_coords

        # Мокаем resize, чтобы возвращать разные массивы для каждого лица
        def mock_resize(face_roi, dsize, **kwargs):
            return np.full((dsize[1], dsize[0], 3), np.random.randint(0, 255), dtype=np.uint8)
        mock_cv2.resize.side_effect = mock_resize

        # Мокаем конвертер, чтобы он возвращал CoreImage для каждого лица
        expected_images = [
            FaceExtractorObjectMother.create_core_image(
                settings.target_size[1], settings.target_size[0]
            ) for _ in range(face_count)
        ]
        mock_converter.side_effect = expected_images

        # Act
        result = await service.ExtractAllFacesAsync(input_image, settings)

        # Assert
        assert len(result) == face_count
        assert result == expected_images
        assert mock_converter.call_count == face_count
        # Проверяем, что resize вызывался для каждого лица
        assert mock_cv2.resize.call_count == face_count

    async def test_extract_all_faces_no_faces(
        self,
        service: FaceExtractorService,
        mock_cv2,
        mock_converter,
    ):
        """Когда лица не найдены, возвращается пустой список."""
        # Arrange
        input_image = FaceExtractorObjectMother.create_core_image()
        settings = FaceExtractorObjectMother.create_settings()

        mock_gray = np.zeros((100, 100), dtype=np.uint8)
        mock_cv2.cvtColor.return_value = mock_gray
        mock_cascade = mock_cv2.CascadeClassifier.return_value
        mock_cascade.detectMultiScale.return_value = np.array([])

        # Act
        result = await service.ExtractAllFacesAsync(input_image, settings)

        # Assert
        assert result == []
        mock_converter.assert_not_called()

    async def test_extract_all_faces_detection_error(
        self,
        service: FaceExtractorService,
        mock_cv2,
    ):
        """Ошибка в процессе детекции при извлечении всех лиц."""
        # Arrange
        input_image = FaceExtractorObjectMother.create_core_image()
        settings = FaceExtractorObjectMother.create_settings()

        # Симулируем ошибку
        mock_cv2.cvtColor.side_effect = RuntimeError("OpenCV error")

        # Act & Assert
        with pytest.raises(FaceExtractorExtractionError) as excinfo:
            await service.ExtractAllFacesAsync(input_image, settings)
        assert "Ошибка детекции лиц" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)