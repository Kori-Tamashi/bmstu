from dataclasses import dataclass
from typing import Optional, Tuple

@dataclass(frozen=True)
class FaceExtractorSettings:
    """Настройки экстрактора лиц.

    Определяет параметры детекции и выделения лица из изображения.
    Используется как неизменяемый объект-значение в ядре приложения.

    Attributes:
        target_size: Размер (ширина, высота), до которого масштабируется
            вырезанное лицо.
        margin: Отступ вокруг лица в долях от его размера (например, 0.2 = 20%).
        min_face_size: Минимальный размер (ширина, высота) лица, которое
            считается допустимым для детекции.
    """

    target_size: Tuple[int, int] = (224, 224)
    margin: float = 0.2
    min_face_size: Tuple[int, int] = (30, 30)

    def __post_init__(self):
        """Проверяет корректность значений после создания."""
        if not (0 <= self.margin <= 1):
            raise ValueError("margin должен быть в диапазоне [0, 1]")

        if len(self.target_size) != 2 or any(s <= 0 for s in self.target_size):
            raise ValueError("target_size должен быть кортежем из двух положительных чисел")

        if len(self.min_face_size) != 2 or any(s <= 0 for s in self.min_face_size):
            raise ValueError("min_face_size должен быть кортежем из двух положительных чисел")