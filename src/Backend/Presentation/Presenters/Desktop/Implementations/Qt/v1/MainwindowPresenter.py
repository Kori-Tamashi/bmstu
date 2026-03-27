from typing import Optional

from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Entities.FaceExtractorSettings import FaceExtractorSettings
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Converters.ImageConverter import ImageConverter
from Backend.Presentation.Presenters.Desktop.Interfaces.IMainwindowPresenter import IMainwindowPresenter
from Backend.BusinessLogic.Services.ImageService import ImageService
from Backend.BusinessLogic.Services.FaceExtractorService import FaceExtractorService
from Backend.BusinessLogic.Services.DeepfakeDetectorService import DeepfakeDetectorService
from Frontend.Desktop.Qt.v1.Mainwindow import MainWindow


class MainwindowPresenter(IMainwindowPresenter):
    def __init__(
        self,
        image_service: ImageService,
        face_service: FaceExtractorService,
        detector_service: DeepfakeDetectorService,
        face_settings_path: str,
        detector_settings_path: str
    ):
        self.image_service = image_service
        self.face_service = face_service
        self.detector_service = detector_service
        self.face_settings_path = face_settings_path
        self.detector_settings_path = detector_settings_path

        self.view: Optional[MainWindow] = None
        self.current_image: Optional[CoreImage] = None
        self.selected_image: Optional[CoreImage] = None

        self.faces: list[CoreImage] = []
        self.threshold: float = 0.5
        self.face_settings: FaceExtractorSettings | None = None
        self.detector_settings: DeepfakeDetectorSettings | None = None

    def SetView(self, view: MainWindow):
        self.view = view

    async def LoadImage(self, path: str):
        """Загружает изображение и инициирует анализ."""
        try:
            # Загрузка изображения
            core_image = await self.image_service.GetImageAsync(path)
            self.current_image = core_image

            # Конвертация изображения
            pixmap = ImageConverter.ImageToQPixmap(core_image, framework='qt')
            self.view.DisplayLoadedImage(pixmap)
        except Exception as e:
            self.view.ShowError(f"Ошибка загрузки изображения: {e}")

    async def ExtractFaces(self):
        """Извлечение лиц из изображения"""
        try:
            if self.current_image is None:
                return

            # Загрузка настроек извлечения лиц
            if self.face_settings is None:
                self.face_settings = await self.face_service.GetSettingsAsync(self.face_settings_path)

            # Извлечение лиц
            self.faces = await self.face_service.ExtractAllFacesAsync(self.current_image, self.face_settings)

            if self.faces:
                items = []
                for i, face in enumerate(self.faces):
                    pixmap = ImageConverter.ImageToQPixmap(face, framework='qt')
                    text = f"Лицо {i + 1}"
                    items.append((pixmap, text))
                    self.view.UpdateFacesList(items)
            else:
                self.view.ShowMessage("На изображении лиц не было обнаружено")
        except Exception as e:
            self.view.ShowError(f"Ошибка извлечения лиц: {e}")

    async def Analyze(self, face_index: Optional[int] = None):
        """Проверка выбранного изображения"""
        try:
            if self.current_image is None and self.faces == []:
                return

            # Загрузка настроек детектора
            if self.detector_settings is None:
                self.detector_settings = await self.detector_service.GetSettingsAsync(self.detector_settings_path)

            if face_index is not None and 0 <= face_index < len(self.faces):
                image_to_analyze = self.faces[face_index]
            else:
                image_to_analyze = self.current_image

            # Изменение размеров изображения
            image_to_analyze = await self.image_service.ResizeImageAsync(
                image_to_analyze,
                self.detector_settings.input_size[0],
                self.detector_settings.input_size[1]
            )

            # Анализ изображения
            prob, is_fake = await self.detector_service.DetectAsync(image_to_analyze, self.detector_settings)
            self.view.ShowResults(prob, is_fake)
        except Exception as e:
            self.view.ShowError(f"Ошибка анализа: {e}")

    def SetThreshold(self, value: float):
        try:
            self.threshold = value
            if self.current_image is not None:
                updated_settings = DeepfakeDetectorSettings(
                    weights=self.detector_settings.weights,
                    cnn_model_name=self.detector_settings.cnn_model_name,
                    vit_model_name=self.detector_settings.vit_model_name,
                    fusion_dim=self.detector_settings.fusion_dim,
                    dropout_rate=self.detector_settings.dropout_rate,
                    num_classes=self.detector_settings.num_classes,
                    threshold=value,
                    input_size=self.detector_settings.input_size,
                )
                self.detector_settings = updated_settings
        except Exception as e:
            self.view.ShowError(f"Ошибка установки пороговой вероятности: {e}")

    def SetImage(self, face_index: Optional[int] = None):
        try:
            if face_index is not None and 0 <= face_index < len(self.faces):
                image = self.faces[face_index]
            else:
                image = self.current_image
            image_pixmap = ImageConverter.ImageToQPixmap(image, framework='qt')
            self.view.DisplayAnalyzedImage(image_pixmap)
        except Exception as e:
            self.view.ShowError(f"Ошибка установки анализируемого изображения: {e}")

    def IsNoFaces(self):
        return self.faces == []

    def Reset(self):
        self.faces = []
        self.current_image = None
        self.view.UpdateFacesList([])