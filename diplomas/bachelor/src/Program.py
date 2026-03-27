import sys
import torch
import asyncio
import argparse
from PyQt5.QtWidgets import QApplication as QtApp
from PySide6.QtWidgets import QApplication as PySideApp
from qasync import QEventLoop
from dependency_injector import containers, providers
from dependency_injector.providers import Configuration

from Backend.DataAccess.Repositories.Filesystem.ImageRepository import ImageRepository
from Backend.DataAccess.Repositories.Filesystem.FaceExtractorSettingsRepository import FaceExtractorSettingsRepository
from Backend.DataAccess.Repositories.Filesystem.DeepfakeDetectorSettingsRepository import DeepfakeDetectorSettingsRepository
from Backend.DataAccess.Repositories.Filesystem.DeepfakeDetectorRepository import DeepfakeDetectorRepository

from Backend.BusinessLogic.Services.ImageService import ImageService
from Backend.BusinessLogic.Services.FaceExtractorService import FaceExtractorService
from Backend.BusinessLogic.Services.DeepfakeDetectorService import DeepfakeDetectorService


class ApplicationContainer(containers.DeclarativeContainer):
    config = Configuration()

    image_repository = providers.Factory(ImageRepository)
    face_settings_repository = providers.Factory(FaceExtractorSettingsRepository)
    detector_settings_repository = providers.Factory(DeepfakeDetectorSettingsRepository)
    detector_model_repository = providers.Factory(DeepfakeDetectorRepository)

    image_service = providers.Factory(ImageService, repository=image_repository)
    face_service = providers.Factory(FaceExtractorService, settings_repository=face_settings_repository)
    detector_service = providers.Factory(
        DeepfakeDetectorService,
        settings_repository=detector_settings_repository,
        model_repository=detector_model_repository,
    )


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--gui', choices=['qt', 'pysite'], default='qt', help='Графический интерфейс')
    parser.add_argument('--version', choices=['v1'], default='v1', help='Версия интерфейса')
    args = parser.parse_args()

    container = ApplicationContainer()
    container.config.face_settings_path.from_env(
        "FACE_SETTINGS_PATH",
        required=False,
        default="config/face_extractor.json"
    )
    container.config.detector_settings_path.from_env(
        "DETECTOR_SETTINGS_PATH",
        required=False,
        default="config/data.pth"
    )

    # Создаём зависимости
    image_service = container.image_service()
    face_service = container.face_service()
    detector_service = container.detector_service()

    # Выбираем презентер и окно в зависимости от аргументов
    if args.gui == 'qt' and args.version == 'v1':
        from Backend.Presentation.Presenters.Desktop.Implementations.Qt.v1.MainwindowPresenter import MainwindowPresenter
        from Frontend.Desktop.Qt.v1.Mainwindow import MainWindow
        AppClass = QtApp
    elif args.gui == 'pysite' and args.version == 'v1':
        from Backend.Presentation.Presenters.Desktop.Implementations.PySite.v1.MainwindowPresenter import MainwindowPresenter
        from Frontend.Desktop.PySite.v1.Mainwindow import MainWindow
        AppClass = PySideApp
    else:
        raise ValueError(f"Неподдерживаемая комбинация: gui={args.gui}, version={args.version}")

    # Создаём презентер
    presenter = MainwindowPresenter(
        image_service=image_service,
        face_service=face_service,
        detector_service=detector_service,
        face_settings_path=container.config.face_settings_path(),
        detector_settings_path=container.config.detector_settings_path()
    )

    # Инициализируем приложение и цикл событий
    app = AppClass(sys.argv)
    loop = QEventLoop(app)
    asyncio.set_event_loop(loop)

    window = MainWindow(presenter)
    window.show()

    with loop:
        loop.run_forever()


if __name__ == "__main__":
    main()