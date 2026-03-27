
import asyncio
import torch
import torch.nn.functional as F
from typing import Dict, Any, Tuple

from Backend.Core.Entities.Image import Image
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Repositories.Interfaces.IDeepfakeDetectorSettingsRepository import IDeepfakeDetectorSettingsRepository
from Backend.Core.Repositories.Interfaces.IDeepfakeDetectorRepository import IDeepfakeDetectorRepository
from Backend.Core.Services.Interfaces.IDeepfakeDetectorService import IDeepfakeDetectorService
from Backend.Core.Services.Exceptions.DeepfakeDetectorServiceExceptions import (
    SettingsLoadError,
    DetectionError,
)
from Backend.Core.Converters.ImageConverter import ImageConverter


class DeepfakeDetectorService(IDeepfakeDetectorService):
    """Реализация сервиса детекции дипфейков с использованием репозитория моделей."""

    def __init__(
        self,
        settings_repository: IDeepfakeDetectorSettingsRepository,
        model_repository: IDeepfakeDetectorRepository
    ):
        self._settings_repository = settings_repository
        self._model_repository = model_repository

    async def GetSettingsAsync(self, path: str) -> DeepfakeDetectorSettings:
        try:
            return await self._settings_repository.GetDeepfakeDetectorSettingsAsync(path)
        except Exception as e:
            raise SettingsLoadError(f"Ошибка загрузки настроек: {e}") from e

    async def DetectAsync(self, image: Image, settings: DeepfakeDetectorSettings) -> Tuple[float, bool]:
        """
        Асинхронно выполняет детекцию дипфейка.
        """

        def PrepareTensor(image: Image, settings: DeepfakeDetectorSettings) -> torch.Tensor:
            tensor = ImageConverter.ImageToTensor(image, normalize=True)
            return tensor.unsqueeze(0)  # добавить batch dimension

        def ForwardCNN(modules: Dict[str, Any], x: torch.Tensor) -> torch.Tensor:
            return modules['cnn'](x)

        def ForwardVit(modules: Dict[str, Any], x: torch.Tensor) -> torch.Tensor:
            out = modules['vit'](pixel_values=x)
            return out.last_hidden_state[:, 0, :]  # [CLS] токен

        def ApplyProjections(modules: Dict[str, Any], cnn_feat: torch.Tensor, vit_feat: torch.Tensor) -> Tuple[torch.Tensor, torch.Tensor]:
            cnn_emb = modules['cnn_proj'](cnn_feat)
            vit_emb = modules['vit_proj'](vit_feat)
            return cnn_emb, vit_emb

        def Classify(modules: Dict[str, Any], cnn_emb: torch.Tensor, vit_emb: torch.Tensor) -> torch.Tensor:
            combined = torch.cat([cnn_emb, vit_emb], dim=1)
            return modules['classifier'](combined)

        def Forward(modules: Dict[str, Any], x: torch.Tensor) -> torch.Tensor:
            device = modules['device']
            x = x.to(device)
            cnn_feat = ForwardCNN(modules, x)
            vit_feat = ForwardVit(modules, x)
            cnn_emb, vit_emb = ApplyProjections(modules, cnn_feat, vit_feat)
            logits = Classify(modules, cnn_emb, vit_emb)
            return logits

        def Predict(modules: Dict[str, Any], tensor: torch.Tensor, threshold: float) -> Tuple[float, bool]:
            with torch.no_grad():
                logits = Forward(modules, tensor)
                if settings.num_classes == 1:
                    prob = torch.sigmoid(logits).item()
                else:
                    probs = F.softmax(logits, dim=1)
                    prob = probs[0, 1].item()  # индекс 1 - Fake
            is_fake = prob >= threshold
            return prob, is_fake

        try:
            # Получаем модель из репозитория
            modules = await self._model_repository.LoadModelAsync(settings)

            # Подготавливаем тензор
            tensor = await asyncio.to_thread(PrepareTensor, image, settings)

            # Выполняем предсказание
            prob, is_fake = await asyncio.to_thread(Predict, modules, tensor, settings.threshold)

            return prob, is_fake
        except Exception as e:
            raise DetectionError(f"Ошибка детекции: {e}") from e