# Presentation/views/main_window.py
import asyncio
from PyQt5.QtWidgets import QMainWindow, QFileDialog, QGraphicsScene, QListWidget, QListWidgetItem, QProgressBar, QLabel, QMessageBox
from PyQt5.QtGui import QPixmap, QIcon
from PyQt5.QtCore import Qt, QSize
from qasync import asyncSlot
from Frontend.Desktop.Qt.v1.MainwindowSetup import Ui_MainWindow
from Backend.Presentation.Presenters.Desktop.Interfaces.IMainwindowPresenter import IMainwindowPresenter


class MainWindow(QMainWindow):
    def __init__(self, presenter: IMainwindowPresenter):
        super().__init__()
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)
        self.presenter = presenter
        self.presenter.SetView(self)
        self.setWindowIcon(QIcon('images/magnifier.ico'))

        # Создаём виджеты прогресса
        self.progress_bar = QProgressBar()
        self.progress_label = QLabel()
        self.statusBar().addPermanentWidget(self.progress_label)
        self.statusBar().addPermanentWidget(self.progress_bar)
        self.HideProgress()

        self.ConnectSignals()

    def ConnectSignals(self):
        self.ui.loadImage_pushButton.clicked.connect(self.OnLoadImageClicked)
        self.ui.faces_listWidget.itemClicked.connect(self.OnFaceItemClicked)
        self.ui.thresholdProbability_horizontalSlider.valueChanged.connect(self.OnThresholdSliderChanged)
        self.ui.thresholdProbability_doubleSpinBox.valueChanged.connect(self.OnThresholdSpinChanged)
        self.ui.programDescription_action.triggered.connect(self.OnAboutProgram)

    # ---------- Управление прогрессом ----------
    def ShowProgress(self, text: str, value: int = -1):
        self.progress_bar.setVisible(True)
        self.progress_label.setVisible(True)
        self.progress_label.setText(text)
        if value >= 0:
            self.progress_bar.setRange(0, 100)
            self.progress_bar.setValue(value)
        else:
            self.progress_bar.setRange(0, 0)

    def UpdateProgress(self, value: int, text: str = None):
        if value >= 0:
            self.progress_bar.setRange(0, 100)
            self.progress_bar.setValue(value)
        if text is not None:
            self.progress_label.setText(text)

    def HideProgress(self):
        self.progress_bar.setVisible(False)
        self.progress_label.setVisible(False)

    # ---------- Обработчики событий ----------
    @asyncSlot()
    async def OnLoadImageClicked(self):
        file_path, _ = QFileDialog.getOpenFileName(
            self, "Выберите изображение", "",
            "Изображения (*.png *.jpg *.jpeg *.bmp *.tiff)"
        )
        if file_path:
            await self.LoadImage(file_path)

    @asyncSlot(QListWidgetItem)
    async def OnFaceItemClicked(self, item: QListWidgetItem):
        index = self.ui.faces_listWidget.row(item)
        self.ShowProgress("Подготовка...", 0)
        # Устанавливаем изображение для анализа
        self.presenter.SetImage(index)
        self.UpdateProgress(30, "Изображение выбрано")
        # Запускаем анализ
        self.UpdateProgress(60, "Анализ...")
        await self.presenter.Analyze(index)
        self.UpdateProgress(100, "Анализ завершён")
        # Небольшая задержка, чтобы пользователь увидел 100%
        await asyncio.sleep(0.3)
        self.HideProgress()

    def OnThresholdSliderChanged(self, value):
        float_val = value / 100.0
        self.ui.thresholdProbability_doubleSpinBox.setValue(float_val)
        self.presenter.SetThreshold(float_val)

    def OnThresholdSpinChanged(self, value):
        self.ui.thresholdProbability_horizontalSlider.setValue(int(value * 100))
        self.presenter.SetThreshold(value)

    def OnAboutProgram(self):
        """Показывает окно с информацией о программе."""
        QMessageBox.about(
            self,
            "О программе",
            "Автор: Сальников Михаил Алексеевич\n"
            "Группа: ИУ7-74Б\n"
            "Работа: Выпускная квалификационная работа бакалавра\n"
            "Год: 2026\n"
            "\n"
            "© Все права защищены"
        )

    # ---------- Основной процесс загрузки ----------
    async def LoadImage(self, file_path: str):
        self.ShowProgress("Сброс состояния...", 0)
        self.Reset()
        self.UpdateProgress(20, "Загрузка изображения...")
        await self.presenter.LoadImage(file_path)
        self.UpdateProgress(50, "Извлечение лиц...")
        await self.presenter.ExtractFaces()

        if self.presenter.IsNoFaces():
            self.UpdateProgress(70, "Aнализ всего изображения...")
            self.presenter.SetImage(None)
            await self.presenter.Analyze(None)
            self.UpdateProgress(100, "Анализ завершён")
            await asyncio.sleep(0.3)
            self.HideProgress()
        else:
            self.UpdateProgress(100, "Извлечение лиц завершено")
            # Прогресс скрываем, так как следующий шаг – выбор лица пользователем
            await asyncio.sleep(0.3)
            self.HideProgress()

    # ---------- Методы отображения ----------
    def DisplayLoadedImage(self, pixmap: QPixmap):
        scene = QGraphicsScene()
        scene.addPixmap(pixmap)
        self.ui.loadedImage_graphicsView.setScene(scene)
        self.ui.loadedImage_graphicsView.fitInView(scene.itemsBoundingRect(), Qt.KeepAspectRatio)

    def DisplayAnalyzedImage(self, pixmap: QPixmap):
        scene = QGraphicsScene()
        scene.addPixmap(pixmap)
        self.ui.analyzedImage_graphicsView.setScene(scene)
        self.ui.analyzedImage_graphicsView.fitInView(scene.itemsBoundingRect(), Qt.KeepAspectRatio)

    def UpdateFacesList(self, items):
        self.ui.faces_listWidget.clear()
        self.ui.faces_listWidget.setViewMode(QListWidget.IconMode)
        self.ui.faces_listWidget.setIconSize(QSize(100, 100))
        self.ui.faces_listWidget.setResizeMode(QListWidget.Adjust)
        self.ui.faces_listWidget.setGridSize(QSize(120, 140))
        self.ui.faces_listWidget.setWordWrap(True)

        for pixmap, text in items:
            item = QListWidgetItem(text)
            item.setIcon(QIcon(pixmap))
            self.ui.faces_listWidget.addItem(item)

    def ShowResults(self, probability: float, is_fake: bool):
        prob_text = f"Вероятность наличия признаков генерации: {probability:.4f}<br>"
        prefix = "Решение: "
        if is_fake:
            decision = f'<span style="color: red; font-weight: bold;">поддельное изображение</span>'
        else:
            decision = f'<span style="color: green; font-weight: bold;">реальное изображение</span>'
        html = prob_text + prefix + decision
        self.ui.results_textBrowser.setHtml(html)

    def ShowError(self, message: str):
        self.statusBar().showMessage(f"Ошибка: {message}")

    def ShowMessage(self, message: str):
        self.statusBar().showMessage(f"Уведомление: {message}")

    def Reset(self):
        self.presenter.Reset()
        self.statusBar().showMessage("")
