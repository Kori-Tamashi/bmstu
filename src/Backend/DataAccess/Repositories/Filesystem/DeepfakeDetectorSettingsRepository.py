from Backend.Core.Repositories.Exceptions.DeepfakeDetectorSettingsRepositoryExceptions import *
from Backend.Core.Repositories.Interfaces.IDeepfakeDetectorSettingsRepository import IDeepfakeDetectorSettingsRepository
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings

from typing import Dict, Any
from pathlib import Path
import asyncio
import pickle
import torch


class DeepfakeDetectorSettingsRepository(IDeepfakeDetectorSettingsRepository):
    """Репозиторий, загружающий настройки и веса детектора дипфейков из .pth файла."""

    async def GetDeepfakeDetectorSettingsAsync(self, path: str) -> DeepfakeDetectorSettings:
        """
        Асинхронно загружает чекпоинт из файла и возвращает объект DeepfakeDetectorSettings.

        Args:
            path: Путь к .pth файлу чекпоинта.

        Returns:
            DeepfakeDetectorSettings: Объект с настройками и весами модели.

        Raises:
            DetectorConfigNotFoundError: Если файл не существует.
            InvalidDetectorConfigFormatError: Если файл не является валидным .pth.
            DetectorConfigMissingKeysError: Если в чекпоинте отсутствуют обязательные ключи.
            DetectorConfigLoadError: При других ошибках загрузки.
        """
        def LoadCheckpoint(path: Path) -> Dict[str, Any]:
            """
            Синхронно загружает чекпоинт с помощью torch.load.
            Отдельный метод для вызова в потоке.
            """
            return torch.load(path, map_location='cpu')

        file_path = Path(path)

        # Проверка существования файла
        if not file_path.exists():
            raise DetectorConfigNotFoundError(f"Файл чекпоинта не найден: {path}")
        if not file_path.is_file():
            raise DetectorConfigNotFoundError(f"Путь не является файлом: {path}")

        # Загрузка чекпоинта в отдельном потоке
        try:
            checkpoint = await asyncio.to_thread(LoadCheckpoint, file_path)
        except FileNotFoundError as e:
            raise DetectorConfigNotFoundError(f"Файл не найден: {path}") from e
        except (EOFError, RuntimeError, ValueError, KeyError, pickle.UnpicklingError) as e:
            raise InvalidDetectorConfigFormatError(
                f"Файл повреждён или имеет неверный формат: {path}. Ошибка: {e}"
            ) from e
        except Exception as e:
            raise DetectorConfigLoadError(
                f"Не удалось загрузить чекпоинт: {path}. Ошибка: {e}"
            ) from e

        # Проверка наличия обязательных ключей
        required_keys = ['model_config', 'model_state_dict']
        missing_keys = [key for key in required_keys if key not in checkpoint]
        if missing_keys:
            raise DetectorConfigMissingKeysError(
                f"В чекпоинте отсутствуют обязательные ключи: {missing_keys}"
            )

        model_config = checkpoint['model_config']
        state_dict = checkpoint['model_state_dict']

        settings = DeepfakeDetectorSettings(
            weights=state_dict,
            cnn_model_name=model_config.get('cnn_model_name', 'efficientnet_b1'),
            vit_model_name=model_config.get('vit_model_name', 'google/vit-base-patch16-224-in21k'),
            fusion_dim=model_config.get('fusion_dim', 512),
            dropout_rate=model_config.get('dropout_rate', 0.3),
            num_classes=model_config.get('num_classes', 1),
            threshold=model_config.get('threshold', 0.5),
            input_size=model_config.get('input_size', (224, 224))
        )

        return settings