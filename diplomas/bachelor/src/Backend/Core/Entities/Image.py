from dataclasses import dataclass
import numpy as np

@dataclass(frozen=True)
class Image:
    """Представляет цифровое изображение в предметной области.

    Содержит пиксельные данные и метаинформацию. Используется как
    неизменяемый объект-значение (value object) в ядре приложения.

    Attributes:
        pixels: Тензор изображения. Трёхмерный массив NumPy формы (height, width, channels)
            со значениями пикселей. Обычно в диапазоне 0–255 (uint8)
            или после нормализации float.
    """

    pixels: np.ndarray

    @property
    def height(self) -> int:
        return self.pixels.shape[0]

    @property
    def width(self) -> int:
        return self.pixels.shape[1]

    @property
    def channels(self) -> int:
        return self.pixels.shape[2]