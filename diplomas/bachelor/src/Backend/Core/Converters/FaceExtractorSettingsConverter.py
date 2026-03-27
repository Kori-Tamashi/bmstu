from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings

from typing import Dict, Any, Tuple
import json


class FaceExtractorSettingsConverter:
    """Конвертер между доменным объектом FaceExtractorSettings и JSON-представлением."""

    @staticmethod
    def SettingsToDict(settings: FaceExtractorSettings) -> Dict[str, Any]:
        """
        Преобразует доменный объект в словарь для последующей сериализации в JSON.
        Кортежи преобразуются в списки, так как JSON не поддерживает кортежи.
        """
        return {
            'target_size': list(settings.target_size),
            'margin': settings.margin,
            'min_face_size': list(settings.min_face_size)
        }

    @staticmethod
    def DictToSettings(data: Dict[str, Any]) -> FaceExtractorSettings:
         """
         Преобразует словарь (из JSON) в доменный объект FaceExtractorSettings.
         """
         # Проверка типов
         target_size = data.get('target_size', (224, 224))
         if not isinstance(target_size, (list, tuple)):
             raise TypeError(f"target_size должен быть списком или кортежем, получен {type(target_size).__name__}")
         target_size = tuple(target_size)

         margin = data.get('margin', 0.2)
         if not isinstance(margin, (int, float)):
             raise TypeError(f"margin должен быть числом, получен {type(margin).__name__}")
         margin = float(margin)

         min_face_size = data.get('min_face_size', (30, 30))
         if not isinstance(min_face_size, (list, tuple)):
             raise TypeError(f"min_face_size должен быть списком или кортежем, получен {type(min_face_size).__name__}")
         min_face_size = tuple(min_face_size)

         return FaceExtractorSettings(
             target_size=target_size,
             margin=margin,
             min_face_size=min_face_size
         )

    @staticmethod
    def SettingsToJson(settings: FaceExtractorSettings, ensure_ascii: bool = False, indent: int = 2) -> str:
        """
        Преобразует доменный объект в JSON-строку.
        """
        data = FaceExtractorSettingsConverter.SettingsToDict(settings)
        return json.dumps(data, ensure_ascii=ensure_ascii, indent=indent)

    @staticmethod
    def JsonToSettings(json_str: str) -> FaceExtractorSettings:
        """
        Преобразует JSON-строку в доменный объект.
        """
        data = json.loads(json_str)
        return FaceExtractorSettingsConverter.DictToSettings(data)