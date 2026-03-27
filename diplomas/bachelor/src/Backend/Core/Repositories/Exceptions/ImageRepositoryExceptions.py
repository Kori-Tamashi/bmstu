

class ImageRepositoryError(Exception):
    """Базовое исключение для всех ошибок репозитория изображений."""
    pass


class ImageNotFoundError(ImageRepositoryError):
    """Исключение, возникающее когда файл изображения не найден по указанному пути."""
    pass


class ImageLoadError(ImageRepositoryError):
    """Исключение, возникающее при ошибке загрузки изображения (например, повреждённый или некорректный файл)."""
    pass


class InvalidImageFormatError(ImageRepositoryError):
    """Исключение, возникающее когда формат изображения не поддерживается системой."""
    pass
