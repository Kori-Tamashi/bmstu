# Tests/Integration/Services/FaceExtractorServiceIntegrationTests.py

import pytest
import json
import numpy as np
from pathlib import Path
from PIL import Image, ImageDraw

from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings
from Backend.DataAccess.Repositories.Filesystem.FaceExtractorSettingsRepository import FaceExtractorSettingsRepository
from Backend.BusinessLogic.Services.FaceExtractorService import FaceExtractorService
from Backend.Core.Services.Exceptions.FaceExtractorServiceExceptions import (
    FaceExtractorSettingsLoadError,
    FaceExtractorExtractionError,
)


# ----------------------------------------------------------------------
# Фикстуры
# ----------------------------------------------------------------------
@pytest.fixture
def settings_repo():
    """Реальный репозиторий настроек (работает с файловой системой)."""
    return FaceExtractorSettingsRepository()


@pytest.fixture
def service(settings_repo):
    """Сервис с реальным репозиторием."""
    return FaceExtractorService(settings_repository=settings_repo)


@pytest.fixture
def test_data_dir(tmp_path):
    """
    Создаёт временную директорию с тестовыми данными:
    - файл настроек (valid_settings.json)
    - изображение с лицом (face.jpg) – синтетическое, созданное через Pillow
    - изображение без лица (no_face.jpg)
    """
    data_dir = tmp_path / "test_data"
    data_dir.mkdir()

    # Создаём валидный JSON с настройками
    settings_dict = {
        "target_size": [224, 224],
        "margin": 0.2,
        "min_face_size": [30, 30]
    }
    settings_file = data_dir / "valid_settings.json"
    settings_file.write_text(json.dumps(settings_dict), encoding='utf-8')

    # Создаём синтетическое изображение с лицом через Pillow
    face_img = Image.new('RGB', (300, 300), color='gray')
    draw = ImageDraw.Draw(face_img)
    # Рисуем овал (лицо)
    draw.ellipse((100, 80, 200, 220), fill='white')
    # Глаза
    draw.ellipse((120, 120, 140, 140), fill='black')
    draw.ellipse((160, 120, 180, 140), fill='black')
    # Рот
    draw.arc((130, 160, 170, 190), start=0, end=180, fill='black', width=2)
    face_img.save(data_dir / "face.jpg")

    # Создаём изображение без лица (сплошной серый фон)
    no_face_img = Image.new('RGB', (200, 200), color='gray')
    no_face_img.save(data_dir / "no_face.jpg")

    # Проверяем, что файлы действительно созданы и читаются
    assert (data_dir / "face.jpg").exists(), "face.jpg не создан"
    assert (data_dir / "no_face.jpg").exists(), "no_face.jpg не создан"

    return data_dir


# ----------------------------------------------------------------------
# Вспомогательная функция для загрузки CoreImage (использует PIL вместо OpenCV)
# ----------------------------------------------------------------------
def load_core_image_from_path(path: Path) -> CoreImage:
    """Загружает изображение из файла и возвращает CoreImage (через PIL)."""
    if not path.exists():
        raise RuntimeError(f"Файл не существует: {path}")
    pil_image = Image.open(path)
    if pil_image.mode != 'RGB':
        pil_image = pil_image.convert('RGB')
    np_array = np.array(pil_image)
    return CoreImage(pixels=np_array)


# ----------------------------------------------------------------------
# Тесты
# ----------------------------------------------------------------------
@pytest.mark.asyncio
class TestFaceExtractorServiceIntegration:
    """Интеграционные тесты для FaceExtractorService."""

    # ------------------------------------------------------------------
    # GetSettingsAsync
    # ------------------------------------------------------------------
    async def test_get_settings_success(self, service, test_data_dir):
        """Успешная загрузка настроек из файла."""
        settings_path = test_data_dir / "valid_settings.json"
        settings = await service.GetSettingsAsync(str(settings_path))

        assert isinstance(settings, FaceExtractorSettings)
        assert settings.target_size == (224, 224)
        assert settings.margin == 0.2
        assert settings.min_face_size == (30, 30)

    async def test_get_settings_file_not_found(self, service, test_data_dir):
        """Файл настроек не существует."""
        fake_path = test_data_dir / "missing.json"
        with pytest.raises(FaceExtractorSettingsLoadError) as excinfo:
            await service.GetSettingsAsync(str(fake_path))
        assert "Ошибка загрузки настроек" in str(excinfo.value)

    async def test_get_settings_invalid_json(self, service, test_data_dir):
        """Файл содержит невалидный JSON."""
        invalid_file = test_data_dir / "invalid.json"
        invalid_file.write_text("{invalid", encoding='utf-8')
        with pytest.raises(FaceExtractorSettingsLoadError) as excinfo:
            await service.GetSettingsAsync(str(invalid_file))
        assert "Ошибка загрузки настроек" in str(excinfo.value)

    # ------------------------------------------------------------------
    # ExtractFaceAsync (одно лицо)
    # ------------------------------------------------------------------
    async def test_extract_face_with_face_found(self, service, test_data_dir):
        """Изображение содержит лицо – должно вернуть одно лицо."""
        settings_path = test_data_dir / "valid_settings.json"
        settings = await service.GetSettingsAsync(str(settings_path))

        core_image = load_core_image_from_path(test_data_dir / "face.jpg")
        result = await service.ExtractFaceAsync(core_image, settings)

        assert result is not None
        assert isinstance(result, CoreImage)
        assert result.width == settings.target_size[0]
        assert result.height == settings.target_size[1]
        assert result.channels == 3

    async def test_extract_face_no_face(self, service, test_data_dir):
        """Изображение без лица – возвращается None."""
        settings_path = test_data_dir / "valid_settings.json"
        settings = await service.GetSettingsAsync(str(settings_path))

        core_image = load_core_image_from_path(test_data_dir / "no_face.jpg")
        result = await service.ExtractFaceAsync(core_image, settings)

        assert result is None

    # ------------------------------------------------------------------
    # ExtractAllFacesAsync (все лица)
    # ------------------------------------------------------------------
    async def test_extract_all_faces_with_face(self, service, test_data_dir):
        """Изображение с лицом – возвращает список с одним лицом."""
        settings_path = test_data_dir / "valid_settings.json"
        settings = await service.GetSettingsAsync(str(settings_path))

        core_image = load_core_image_from_path(test_data_dir / "face.jpg")
        result = await service.ExtractAllFacesAsync(core_image, settings)

        assert isinstance(result, list)
        assert len(result) > 0
        for face in result:
            assert isinstance(face, CoreImage)
            assert face.width == settings.target_size[0]
            assert face.height == settings.target_size[1]
            assert face.channels == 3

    async def test_extract_all_faces_no_face(self, service, test_data_dir):
        """Изображение без лица – пустой список."""
        settings_path = test_data_dir / "valid_settings.json"
        settings = await service.GetSettingsAsync(str(settings_path))

        core_image = load_core_image_from_path(test_data_dir / "no_face.jpg")
        result = await service.ExtractAllFacesAsync(core_image, settings)

        assert result == []

    # ------------------------------------------------------------------
    # Обработка ошибок OpenCV (например, неподдерживаемое изображение)
    # ------------------------------------------------------------------
    async def test_extract_face_with_corrupt_image(self, service, test_data_dir):
        """Попытка обработать битое изображение (невалидные пиксели)."""
        settings_path = test_data_dir / "valid_settings.json"
        settings = await service.GetSettingsAsync(str(settings_path))

        empty_pixels = np.zeros((0, 0, 3), dtype=np.uint8)
        core_image = CoreImage(pixels=empty_pixels)

        with pytest.raises(FaceExtractorExtractionError) as excinfo:
            await service.ExtractFaceAsync(core_image, settings)
        assert "Ошибка детекции лиц" in str(excinfo.value)