from dataclasses import dataclass
from typing import Optional, Dict, Any

@dataclass(frozen=True)
class DeepfakeDetectorSettings:
    """Настройки гибридного детектора дипфейков (EfficientNet + ViT).

    Содержит параметры, определяющие архитектуру модели и логику принятия решения.
    Используется как неизменяемый объект-значение в ядре приложения.

    Attributes:
        weights: Веса модели
        cnn_model_name: Название свёрточной модели из библиотеки timm
            (например, 'efficientnet_b1').
        vit_model_name: Название ViT модели из Hugging Face
            (например, 'google/vit-base-patch16-224-in21k').
        fusion_dim: Размерность объединённых признаков после проекции.
        dropout_rate: Вероятность dropout в классификаторе.
        num_classes: Количество выходных классов (1 для бинарной классификации
            с сигмоидой, 2 — с softmax).
        threshold: Порог уверенности для отнесения изображения к классу "Fake"
            (используется, если num_classes=1 или для интерпретации softmax).
        input_size: Размер входного изображения (высота, ширина). По умолчанию (224, 224).
    """
    weights: dict[str, Any]
    cnn_model_name: str = 'efficientnet_b1'
    vit_model_name: str = 'google/vit-base-patch16-224-in21k'
    fusion_dim: int = 512
    dropout_rate: float = 0.3
    num_classes: int = 1
    threshold: float = 0.5
    input_size: tuple = (224, 224)

    def __post_init__(self):
        """Проверяет корректность значений после создания."""
        if not 0 <= self.threshold <= 1:
            raise ValueError('threshold должен быть в диапазоне [0, 1]')
        if self.num_classes not in (1, 2):
            raise ValueError('num_classes должен быть 1 или 2')
        if not 0 < self.dropout_rate < 1:
            raise ValueError('dropout_rate должен быть между 0 и 1')
        if self.fusion_dim <= 0:
            raise ValueError('fusion_dim должен быть положительным')
        if len(self.input_size) != 2 or any(s <= 0 for s in self.input_size):
            raise ValueError('input_size должен быть кортежем из двух положительных чисел')