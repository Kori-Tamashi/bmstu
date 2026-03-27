
import asyncio
import cv2
import numpy as np
from typing import List, Optional
from PIL import Image as PILImage

from Backend.Core.Entities.Image import Image
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings
from Backend.Core.Repositories.Interfaces.IFaceExtractorSettingsRepository import IFaceExtractorSettingsRepository
from Backend.Core.Services.Interfaces.IFaceExtractorService import IFaceExtractorService
from Backend.Core.Services.Exceptions.FaceExtractorServiceExceptions import *
from Backend.Core.Converters.ImageConverter import ImageConverter


class FaceExtractorService(IFaceExtractorService):
    """Реализация сервиса для извлечения лиц с использованием OpenCV."""

    def __init__(self, settings_repository: IFaceExtractorSettingsRepository):
        """
        Инициализирует сервис с репозиторием настроек.

        Args:
            settings_repository: Репозиторий для загрузки настроек.
        """
        self._settings_repository = settings_repository

    async def GetSettingsAsync(self, path: str) -> FaceExtractorSettings:
        """
        Асинхронно загружает настройки экстрактора лиц.

        Args:
            path: Путь к файлу настроек.

        Returns:
            FaceExtractorSettings: Объект настроек.

        Raises:
            FaceExtractorSettingsLoadError: Если не удалось загрузить настройки.
        """
        try:
            return await self._settings_repository.GetFaceExtractorSettingsAsync(path)
        except Exception as e:
            raise FaceExtractorSettingsLoadError(f"Ошибка загрузки настроек: {e}") from e

    async def ExtractFaceAsync(self, image: Image, settings: FaceExtractorSettings) -> Optional[Image]:
        """
        Асинхронно извлекает одно лицо (самое большое) из изображения.

        Args:
            image: Исходное доменное изображение.
            settings: Настройки экстрактора.

        Returns:
            Optional[Image]: Изображение лица или None, если лицо не найдено.

        Raises:
            FaceExtractorExtractionError: При ошибке обработки.
        """
        try:
            faces = await asyncio.to_thread(self.DetectFaces, image, settings)
        except Exception as e:
            raise FaceExtractorExtractionError(f"Ошибка детекции лиц: {e}") from e

        if not faces:
            return None

        largest_face = max(faces, key=lambda face: face.shape[0] * face.shape[1])
        largest_face_pil = PILImage.fromarray(largest_face)
        return ImageConverter.PillowToImage(largest_face_pil)


    async def ExtractAllFacesAsync(self, image: Image, settings: FaceExtractorSettings) -> List[Image]:
        """
        Асинхронно извлекает все лица из изображения.

        Args:
            image: Исходное доменное изображение.
            settings: Настройки экстрактора.

        Returns:
            List[Image]: Список изображений лиц (может быть пустым).

        Raises:
            FaceExtractorExtractionError: При ошибке обработки.
        """
        try:
            faces = await asyncio.to_thread(self.DetectFaces, image, settings)
        except Exception as e:
            raise FaceExtractorExtractionError(f"Ошибка детекции лиц: {e}") from e

        return [ImageConverter.PillowToImage(PILImage.fromarray(face)) for face in faces]

    def DetectFaces(self, image: Image, settings: FaceExtractorSettings) -> List[np.ndarray]:
        """
        Синхронно детектирует лица на изображении и возвращает список вырезанных лиц,
        приведённых к target_size.

        Args:
            image: Исходное доменное изображение.
            settings: Настройки экстрактора.

        Returns:
            List[np.ndarray]: Список массивов лиц (RGB, uint8) размера target_size.
        """
        # Конвертируем доменное изображение в numpy (уже есть)
        img_np = image.pixels
        gray = cv2.cvtColor(img_np, cv2.COLOR_RGB2GRAY)

        # Загружаем каскад (всегда используем стандартный)
        cascade_path = cv2.data.haarcascades + 'haarcascade_frontalface_default.xml'
        face_cascade = cv2.CascadeClassifier(cascade_path)

        faces = face_cascade.detectMultiScale(
            gray,
            scaleFactor=1.15,
            minNeighbors=8,
            minSize=settings.min_face_size
        )

        result = []
        target_w, target_h = settings.target_size

        for (x, y, w, h) in faces:
            # Добавляем отступ
            margin_w = int(w * settings.margin)
            margin_h = int(h * settings.margin)
            x1 = max(0, x - margin_w)
            y1 = max(0, y - margin_h)
            x2 = min(img_np.shape[1], x + w + margin_w)
            y2 = min(img_np.shape[0], y + h + margin_h)

            face_roi = img_np[y1:y2, x1:x2]
            face_resized = cv2.resize(face_roi, (target_w, target_h),
                                      interpolation=cv2.INTER_LINEAR)
            result.append(face_resized)

        return result
