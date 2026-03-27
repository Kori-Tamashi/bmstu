# Tests/Integration/Repositories/Filesystem/FaceExtractorSettingsRepositoryIntegrationTests.py

import json
import pytest
from pathlib import Path

from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings
from Backend.Core.Repositories.Exceptions.FaceExtractorExceptions import (
    FaceExtractorSettingsNotFoundError,
    FaceExtractorSettingsValidationError,
    InvalidFaceExtractorSettingsFormatError,
    FaceExtractorSettingsLoadError,
)
from Backend.DataAccess.Repositories.Filesystem.FaceExtractorSettingsRepository import (
    FaceExtractorSettingsRepository,
)


@pytest.fixture
def repo() -> FaceExtractorSettingsRepository:
    """Фикстура, возвращающая экземпляр репозитория."""
    return FaceExtractorSettingsRepository()


@pytest.fixture
def settings_dir(tmp_path) -> Path:
    """
    Фикстура создаёт временную директорию с набором JSON-файлов настроек.
    Возвращает путь к этой директории.
    """
    dir_path = tmp_path / "settings"
    dir_path.mkdir()

    # 1. Полный корректный JSON
    full_config = {
        "target_size": [300, 300],
        "margin": 0.25,
        "min_face_size": [40, 40],
    }
    (dir_path / "full.json").write_text(json.dumps(full_config), encoding="utf-8")

    # 2. JSON только с target_size (остальное по умолчанию)
    partial_config = {
        "target_size": [256, 256]
    }
    (dir_path / "partial.json").write_text(json.dumps(partial_config), encoding="utf-8")

    # 3. JSON с недопустимым margin (вне [0,1])
    invalid_margin_config = {
        "target_size": [224, 224],
        "margin": 1.5,
        "min_face_size": [30, 30],
    }
    (dir_path / "invalid_margin.json").write_text(json.dumps(invalid_margin_config), encoding="utf-8")

    # 4. JSON с неверным типом для target_size (строка вместо списка)
    type_error_config = {
        "target_size": "wrong",
        "margin": 0.2,
        "min_face_size": [30, 30],
    }
    (dir_path / "type_error.json").write_text(json.dumps(type_error_config), encoding="utf-8")

    # 5. Невалидный JSON (синтаксическая ошибка)
    (dir_path / "invalid.json").write_text('{"target_size": [224, 224], "margin": 0.2, "min_face_size": [30, 30]', encoding="utf-8")  # отсутствует закрывающая скобка

    # 6. Пустая директория (для теста с несуществующим файлом) – не создаём файл
    return dir_path


@pytest.mark.asyncio
class TestFaceExtractorSettingsRepositoryIntegration:
    """Интеграционные тесты для FaceExtractorSettingsRepository."""

    async def test_load_full_config_success(self, repo, settings_dir):
        """Успешная загрузка полного корректного JSON."""
        path = settings_dir / "full.json"
        settings = await repo.GetFaceExtractorSettingsAsync(str(path))

        assert isinstance(settings, FaceExtractorSettings)
        assert settings.target_size == (300, 300)
        assert settings.margin == 0.25
        assert settings.min_face_size == (40, 40)

    async def test_load_partial_config_defaults(self, repo, settings_dir):
        """Загрузка JSON с отсутствующими полями – подставляются значения по умолчанию."""
        path = settings_dir / "partial.json"
        settings = await repo.GetFaceExtractorSettingsAsync(str(path))

        assert settings.target_size == (256, 256)
        assert settings.margin == 0.2          # значение по умолчанию
        assert settings.min_face_size == (30, 30)  # значение по умолчанию

    async def test_file_not_found(self, repo, settings_dir):
        """Попытка загрузить несуществующий файл."""
        path = settings_dir / "nonexistent.json"
        with pytest.raises(FaceExtractorSettingsNotFoundError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(str(path))
        assert "Файл настроек не найден" in str(excinfo.value)

    async def test_path_is_directory(self, repo, settings_dir):
        """Путь указывает на директорию, а не на файл."""
        with pytest.raises(FaceExtractorSettingsNotFoundError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(str(settings_dir))
        assert "Путь не является файлом" in str(excinfo.value)

    async def test_invalid_json_syntax(self, repo, settings_dir):
        """Файл содержит невалидный JSON (синтаксическая ошибка)."""
        path = settings_dir / "invalid.json"
        with pytest.raises(InvalidFaceExtractorSettingsFormatError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(str(path))
        assert "Файл не является валидным JSON" in str(excinfo.value)

    async def test_invalid_margin_value(self, repo, settings_dir):
        """JSON содержит значение margin вне допустимого диапазона."""
        path = settings_dir / "invalid_margin.json"
        with pytest.raises(FaceExtractorSettingsValidationError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(str(path))
        assert "margin должен быть в диапазоне [0, 1]" in str(excinfo.value)

    async def test_type_error_in_config(self, repo, settings_dir):
        """JSON содержит поле с неверным типом (строка вместо списка)."""
        path = settings_dir / "type_error.json"
        with pytest.raises(InvalidFaceExtractorSettingsFormatError) as excinfo:
            await repo.GetFaceExtractorSettingsAsync(str(path))
        # Ожидаем, что конвертер выбросит TypeError, который будет преобразован
        assert "Неверный тип данных" in str(excinfo.value)
