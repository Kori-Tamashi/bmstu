
class DeepfakeDetectorServiceError(Exception):
    """Базовое исключение для всех ошибок сервиса детекции дипфейков."""
    pass


class SettingsLoadError(DeepfakeDetectorServiceError):
    """Исключение при ошибке загрузки настроек детектора."""
    pass


class DetectionError(DeepfakeDetectorServiceError):
    """Исключение при ошибке выполнения инференса модели."""
    pass


class ModelNotInitializedError(DeepfakeDetectorServiceError):
    """Исключение, возникающее если модель не была инициализирована."""
    pass