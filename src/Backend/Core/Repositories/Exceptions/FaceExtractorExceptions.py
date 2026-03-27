

class FaceExtractorSettingsRepositoryError(Exception):
    """Базовое исключение для всех ошибок репозитория настроек экстрактора лиц."""
    pass


class FaceExtractorSettingsNotFoundError(FaceExtractorSettingsRepositoryError):
    """Исключение, возникающее когда файл настроек не найден по указанному пути."""
    pass


class FaceExtractorSettingsLoadError(FaceExtractorSettingsRepositoryError):
    """Исключение, возникающее при ошибке загрузки файла настроек (например, проблемы с чтением)."""
    pass


class InvalidFaceExtractorSettingsFormatError(FaceExtractorSettingsRepositoryError):
    """Исключение, возникающее когда файл имеет неверный формат (не JSON, повреждён и т.п.)."""
    pass


class FaceExtractorSettingsValidationError(FaceExtractorSettingsRepositoryError):
    """Исключение, возникающее когда значения настроек не проходят валидацию (некорректные поля)."""
    pass
