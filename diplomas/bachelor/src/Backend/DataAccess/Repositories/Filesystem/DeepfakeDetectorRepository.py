# Infrastructure/Repositories/Filesystem/DeepfakeDetectorRepository.py

import asyncio
import torch
import torch.nn as nn
from pathlib import Path
from typing import Dict, Any, Optional
import hashlib
import pickle

from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Repositories.Interfaces.IDeepfakeDetectorRepository import IDeepfakeDetectorRepository
from Backend.Core.Repositories.Exceptions.DeepfakeDetectorRepositoryExceptions import (
    DeepfakeDetectorNotFoundError,
    DeepfakeDetectorLoadError,
    DeepfakeDetectorSaveError,
    DeepfakeDetectorAlreadyExistsError,
)
import timm
from transformers import ViTModel


class DeepfakeDetectorRepository(IDeepfakeDetectorRepository):
    """Реализация репозитория моделей, работающего с файловой системой и PyTorch."""

    def __init__(self):
        self._model_cache: Dict[str, Dict[str, Any]] = {}

    async def LoadModelAsync(self, settings: DeepfakeDetectorSettings) -> Dict[str, Any]:
        """
        Загружает модель из кэша или создаёт новую на основе настроек.
        """
        key = self.ComputeCacheKey(settings)

        if key in self._model_cache:
            return self._model_cache[key]

        try:
            model = await asyncio.to_thread(self.BuildModel, settings)
            self._model_cache[key] = model
            return model
        except Exception as e:
            raise DeepfakeDetectorLoadError(f"Ошибка загрузки модели: {e}") from e

    async def SaveModelAsync(self, model: Dict[str, Any], path: str) -> None:
        """
        Сохраняет веса модели в файл.
        Ожидается, что model содержит компоненты с весами.
        """
        file_path = Path(path)
        if file_path.exists():
            raise DeepfakeDetectorAlreadyExistsError(f"Файл уже существует: {path}")

        state_dict = {}
        for name, module in model.items():
            if isinstance(module, nn.Module):
                for key, value in module.state_dict().items():
                    state_dict[f"{name}.{key}"] = value

        try:
            await asyncio.to_thread(torch.save, state_dict, file_path)
        except Exception as e:
            raise DeepfakeDetectorSaveError(f"Ошибка сохранения модели: {e}") from e

    async def DeleteModelAsync(self, path: str) -> None:
        file_path = Path(path)
        if not file_path.exists():
            raise DeepfakeDetectorNotFoundError(f"Файл не найден: {path}")
        try:
            file_path.unlink()
        except Exception as e:
            raise DeepfakeDetectorSaveError(f"Ошибка удаления файла: {e}") from e

    async def ModelExistsAsync(self, path: str) -> bool:
        return Path(path).exists()

    def ComputeCacheKey(self, settings: DeepfakeDetectorSettings) -> str:
        arch_part = (
            settings.cnn_model_name,
            settings.vit_model_name,
            settings.fusion_dim,
            settings.dropout_rate,
            settings.num_classes,
            settings.input_size,
        )
        weights_hash = hashlib.sha256(pickle.dumps(settings.weights)).hexdigest()[:16]
        return f"{arch_part}_{weights_hash}"

    def BuildModel(self, settings: DeepfakeDetectorSettings) -> Dict[str, Any]:
        """Создаёт компоненты модели и загружает веса."""
        device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')

        # Создаём модули
        cnn = timm.create_model(settings.cnn_model_name, pretrained=True, num_classes=0)
        vit = ViTModel.from_pretrained(settings.vit_model_name)

        # Определяем размер выхода CNN
        with torch.no_grad():
            test_input = torch.randn(1, 3, 224, 224)
            cnn_dim = cnn(test_input).shape[1]

        cnn_proj = nn.Sequential(
            nn.Linear(cnn_dim, settings.fusion_dim),
            nn.BatchNorm1d(settings.fusion_dim),
            nn.GELU(),
            nn.Dropout(settings.dropout_rate)
        )

        vit_proj = nn.Sequential(
            nn.Linear(768, settings.fusion_dim),
            nn.BatchNorm1d(settings.fusion_dim),
            nn.GELU(),
            nn.Dropout(settings.dropout_rate)
        )

        classifier = nn.Sequential(
            nn.Linear(settings.fusion_dim * 2, 256),
            nn.GELU(),
            nn.Dropout(settings.dropout_rate),
            nn.Linear(256, settings.num_classes)
        )

        # Загружаем веса
        weights = settings.weights
        self.LoadSubStateDict(cnn, weights, 'cnn')
        self.LoadSubStateDict(vit, weights, 'vit')
        self.LoadSubStateDict(cnn_proj, weights, 'cnn_proj')
        self.LoadSubStateDict(vit_proj, weights, 'vit_proj')
        self.LoadSubStateDict(classifier, weights, 'classifier')

        # Перемещаем на устройство и eval
        modules = {
            'cnn': cnn.to(device).eval(),
            'vit': vit.to(device).eval(),
            'cnn_proj': cnn_proj.to(device).eval(),
            'vit_proj': vit_proj.to(device).eval(),
            'classifier': classifier.to(device).eval(),
            'device': device
        }
        return modules

    def LoadSubStateDict(self, module: nn.Module, full_dict: Dict[str, Any], prefix: str) -> None:
        sub_dict = {k.replace(prefix + '.', ''): v for k, v in full_dict.items() if k.startswith(prefix + '.')}
        module.load_state_dict(sub_dict, strict=False)