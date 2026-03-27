
class ImageServiceError(Exception):
    """Базовое исключение для всех ошибок сервиса изображений."""
    pass


class ImageServiceLoadError(ImageServiceError):
    """Исключение, возникающее при ошибке загрузки изображения (обёртка для ошибок репозитория)."""
    pass


class ImageServiceResizeError(ImageServiceError):
    """Исключение, возникающее при ошибке изменения размера изображения."""
    pass


class ImageServiceCropError(ImageServiceError):
    """Исключение, возникающее при ошибке обрезки изображения."""
    pass


class InvalidImageDimensionsError(ImageServiceError):
    """Исключение, возникающее при передаче некорректных размеров (например, отрицательные или нулевые)."""
    pass
