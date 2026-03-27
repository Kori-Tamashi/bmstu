
class FaceExtractorServiceError(Exception):
    """Базовое исключение для всех ошибок сервиса извлечения лиц."""
    pass


class FaceExtractorSettingsLoadError(FaceExtractorServiceError):
    """Исключение при ошибке загрузки настроек экстрактора."""
    pass


class FaceExtractorExtractionError(FaceExtractorServiceError):
    """Исключение при ошибке извлечения лица (например, проблемы с OpenCV)."""
    pass


class NoFaceDetectedError(FaceExtractorServiceError):
    """Исключение, возникающее когда лицо не найдено, но ожидалось хотя бы одно."""
    pass