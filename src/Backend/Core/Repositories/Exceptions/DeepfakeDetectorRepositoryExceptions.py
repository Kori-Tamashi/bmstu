
class DeepfakeDetectorRepositoryError(Exception):
    """Базовое исключение для всех ошибок репозитория моделей детектора дипфейков."""
    pass


class DeepfakeDetectorNotFoundError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее когда файл модели не найден."""
    pass


class DeepfakeDetectorLoadError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее при ошибке загрузки модели."""
    pass


class DeepfakeDetectorSaveError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее при ошибке сохранения модели."""
    pass


class DeepfakeDetectorAlreadyExistsError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее при попытке сохранить модель, если файл уже существует."""
    pass