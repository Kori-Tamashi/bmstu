

class DeepfakeDetectorRepositoryError(Exception):
    """Базовое исключение для всех ошибок репозитория детектора дипфейков."""
    pass


class DetectorConfigNotFoundError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее когда файл конфигурации/весов не найден по указанному пути."""
    pass


class DetectorConfigLoadError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее при ошибке загрузки файла конфигурации (повреждённый или некорректный файл)."""
    pass


class InvalidDetectorConfigFormatError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее когда формат файла не поддерживается (не .pth или невалидный чекпоинт)."""
    pass


class DetectorConfigMissingKeysError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее когда в загруженном чекпоинте отсутствуют обязательные ключи (model_config, model_state_dict и т.д.)."""
    pass


class DetectorConfigVersionMismatchError(DeepfakeDetectorRepositoryError):
    """Исключение, возникающее когда версия модели или конфигурации несовместима с текущей версией приложения."""
    pass
