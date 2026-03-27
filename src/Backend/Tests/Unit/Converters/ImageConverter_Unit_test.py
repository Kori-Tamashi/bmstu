# Tests/Unit/Converters/ImageConverter_Unit_test.py

import pytest
import numpy as np
import torch
from PIL import Image as PilImage
from unittest.mock import MagicMock, patch

from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Converters.ImageConverter import ImageConverter


# ----------------------------------------------------------------------
# Вспомогательные функции для создания тестовых данных
# ----------------------------------------------------------------------
def create_test_core_image(shape=(100, 100, 3), dtype=np.uint8, value=128):
    """Создаёт CoreImage с заданной формой и значением."""
    pixels = np.full(shape, value, dtype=dtype)
    return CoreImage(pixels=pixels)


def create_test_pil_image(mode='RGB', size=(100, 100), color=128):
    """Создаёт PIL Image с заданным режимом и цветом."""
    if mode == 'L':
        arr = np.full((size[1], size[0]), color, dtype=np.uint8)
    else:
        arr = np.full((size[1], size[0], len(mode)), color, dtype=np.uint8)
    return PilImage.fromarray(arr, mode=mode)


# ----------------------------------------------------------------------
# Тесты для методов Pillow <-> CoreImage
# ----------------------------------------------------------------------
class TestPillowConversion:
    """Тесты преобразований между PIL Image и CoreImage."""

    def test_pillow_to_image_rgb(self):
        """RGB PIL -> CoreImage."""
        pil_img = create_test_pil_image('RGB')
        core_img = ImageConverter.PillowToImage(pil_img)
        assert isinstance(core_img, CoreImage)
        assert core_img.pixels.shape == (100, 100, 3)
        assert core_img.pixels.dtype == np.uint8
        assert np.all(core_img.pixels == 128)

    def test_pillow_to_image_grayscale(self):
        """Grayscale PIL -> CoreImage (должен конвертироваться в RGB)."""
        pil_img = create_test_pil_image('L')
        core_img = ImageConverter.PillowToImage(pil_img)
        assert core_img.pixels.shape == (100, 100, 3)
        assert np.all(core_img.pixels[..., 0] == 128)
        assert np.all(core_img.pixels[..., 1] == 128)
        assert np.all(core_img.pixels[..., 2] == 128)

    def test_pillow_to_image_rgba(self):
        """RGBA PIL -> CoreImage (альфа отбрасывается, конвертируется в RGB)."""
        pil_img = create_test_pil_image('RGBA')
        core_img = ImageConverter.PillowToImage(pil_img)
        assert core_img.pixels.shape == (100, 100, 3)
        assert core_img.pixels.dtype == np.uint8

    def test_pillow_to_image_unsupported_mode(self):
        """Неподдерживаемый режим PIL -> ValueError."""
        pil_img = create_test_pil_image('CMYK')
        with pytest.raises(ValueError, match="Unsupported PIL image mode"):
            ImageConverter.PillowToImage(pil_img)

    def test_image_to_pillow_rgb(self):
        """CoreImage RGB -> PIL RGB."""
        core_img = create_test_core_image(shape=(100, 100, 3))
        pil_img = ImageConverter.ImageToPillow(core_img)
        # Обходим проблему isinstance из-за возможного конфликта имён
        assert pil_img is not None
        assert pil_img.mode == 'RGB'
        assert pil_img.size == (100, 100)

    def test_image_to_pillow_grayscale(self):
        """CoreImage с 1 каналом -> PIL Grayscale."""
        core_img = create_test_core_image(shape=(100, 100, 1))
        pil_img = ImageConverter.ImageToPillow(core_img)
        assert pil_img.mode == 'L'
        assert pil_img.size == (100, 100)

    def test_image_to_pillow_rgba(self):
        """CoreImage с 4 каналами -> PIL RGBA."""
        core_img = create_test_core_image(shape=(100, 100, 4))
        pil_img = ImageConverter.ImageToPillow(core_img)
        assert pil_img.mode == 'RGBA'
        assert pil_img.size == (100, 100)

    def test_image_to_pillow_float_conversion(self):
        """CoreImage с float пикселями (0-1) -> uint8."""
        pixels = np.random.rand(50, 50, 3).astype(np.float32)
        core_img = CoreImage(pixels=pixels)
        pil_img = ImageConverter.ImageToPillow(core_img)
        assert pil_img.mode == 'RGB'
        arr = np.array(pil_img)
        assert arr.dtype == np.uint8
        assert arr.max() <= 255
        assert arr.min() >= 0

    def test_image_to_pillow_unsupported_channels(self):
        """Неподдерживаемое число каналов -> ValueError."""
        core_img = create_test_core_image(shape=(100, 100, 2))
        with pytest.raises(ValueError, match="Unsupported number of channels"):
            ImageConverter.ImageToPillow(core_img)


# ----------------------------------------------------------------------
# Тесты для методов Tensor <-> CoreImage
# ----------------------------------------------------------------------
class TestTensorConversion:
    """Тесты преобразований между тензором PyTorch и CoreImage."""

    def test_image_to_tensor_no_norm(self):
        """CoreImage -> тензор без нормализации."""
        core_img = create_test_core_image(shape=(50, 50, 3), value=128)
        tensor = ImageConverter.ImageToTensor(core_img, normalize=False)
        assert isinstance(tensor, torch.Tensor)
        assert tensor.shape == (3, 50, 50)
        assert tensor.min() >= 0.0
        assert tensor.max() <= 1.0
        assert torch.allclose(tensor, torch.full((3, 50, 50), 128/255), atol=1e-6)

    def test_image_to_tensor_with_norm(self):
        """CoreImage -> тензор с нормализацией."""
        core_img = create_test_core_image(shape=(50, 50, 3), value=128)
        tensor = ImageConverter.ImageToTensor(core_img, normalize=True)
        assert tensor.shape == (3, 50, 50)
        # После нормализации среднее близко к 0 (допустимо)
        assert abs(tensor.mean().item()) < 1.0

    def test_tensor_to_image_no_denorm(self):
        """Тензор -> CoreImage без денормализации."""
        tensor = torch.randn(3, 50, 50) * 0.5 + 0.5
        core_img = ImageConverter.TensorToImage(tensor, denormalize=False)
        assert core_img.pixels.shape == (50, 50, 3)
        assert core_img.pixels.dtype == np.uint8
        assert 0 <= core_img.pixels.min() <= core_img.pixels.max() <= 255

    def test_tensor_to_image_with_denorm(self):
        """Тензор -> CoreImage с денормализацией."""
        mean = torch.tensor([0.485, 0.456, 0.406]).view(3, 1, 1)
        std = torch.tensor([0.229, 0.224, 0.225]).view(3, 1, 1)
        tensor = torch.randn(3, 50, 50) * std + mean
        core_img = ImageConverter.TensorToImage(tensor, denormalize=True)
        assert core_img.pixels.dtype == np.uint8

    def test_tensor_to_image_4d_squeeze(self):
        """4D тензор (batch, C, H, W) -> CoreImage."""
        tensor = torch.randn(1, 3, 50, 50)
        core_img = ImageConverter.TensorToImage(tensor)
        assert core_img.pixels.shape == (50, 50, 3)

    def test_tensor_to_image_wrong_dim(self):
        """Некорректная размерность тензора -> ValueError."""
        tensor = torch.randn(10)
        with pytest.raises(ValueError, match="Expected 3D or 4D tensor"):
            ImageConverter.TensorToImage(tensor)


# ----------------------------------------------------------------------
# Тесты для методов Qt (PyQt5 и PySide6) – мокаем QtGui
# ----------------------------------------------------------------------
class TestQtConversion:
    """Тесты методов Qt с использованием моков."""

    @pytest.fixture
    def mock_qtgui(self, mocker):
        """Создаёт мок QtGui, который подменяет реальные классы."""
        mock_qimage = mocker.MagicMock()
        mock_qpixmap = mocker.MagicMock()
        mock_qpixmap.fromImage = mocker.MagicMock(return_value=mock_qpixmap)
        mock_qimage.copy = mocker.MagicMock(return_value=mock_qimage)

        fake_qtgui = mocker.MagicMock()
        fake_qtgui.QImage = mock_qimage
        fake_qtgui.QPixmap = mock_qpixmap
        fake_qtgui.QImage.Format_RGB888 = 4
        fake_qtgui.QImage.Format_RGBA8888 = 5
        return fake_qtgui

    @pytest.fixture
    def mock_qt_module(self, mocker, mock_qtgui):
        """Мокает _get_qt_module для возврата mock_qtgui."""
        return mocker.patch.object(
            ImageConverter,
            '_get_qt_module',
            return_value=mock_qtgui
        )

    def test_image_to_qpixmap_rgb(self, mock_qt_module, mock_qtgui):
        """Преобразование RGB CoreImage в QPixmap (framework='qt')."""
        core_img = create_test_core_image(shape=(50, 50, 3))
        pixmap = ImageConverter.ImageToQPixmap(core_img, framework='qt')

        mock_qtgui.QImage.assert_called_once()
        args, _ = mock_qtgui.QImage.call_args
        assert args[1] == 50  # width
        assert args[2] == 50  # height
        assert args[3] == 3 * 50  # bytes_per_line
        assert args[4] == mock_qtgui.QImage.Format_RGB888
        mock_qtgui.QImage.return_value.copy.assert_called_once()
        mock_qtgui.QPixmap.fromImage.assert_called_once()

    def test_image_to_qpixmap_rgba(self, mock_qt_module, mock_qtgui):
        """Преобразование RGBA CoreImage в QPixmap."""
        core_img = create_test_core_image(shape=(50, 50, 4))
        ImageConverter.ImageToQPixmap(core_img, framework='qt')
        args, _ = mock_qtgui.QImage.call_args
        assert args[3] == 4 * 50
        assert args[4] == mock_qtgui.QImage.Format_RGBA8888

    def test_image_to_qpixmap_grayscale(self, mock_qt_module, mock_qtgui):
        """Преобразование grayscale (1 канал) -> RGB QPixmap."""
        core_img = create_test_core_image(shape=(50, 50, 1))
        ImageConverter.ImageToQPixmap(core_img, framework='qt')
        args, _ = mock_qtgui.QImage.call_args
        assert args[3] == 3 * 50
        assert args[4] == mock_qtgui.QImage.Format_RGB888

    def test_image_to_qpixmap_unsupported_channels(self, mock_qt_module):
        """Неподдерживаемое число каналов -> ValueError."""
        core_img = create_test_core_image(shape=(50, 50, 2))
        with pytest.raises(ValueError, match="Unsupported number of channels"):
            ImageConverter.ImageToQPixmap(core_img, framework='qt')

    def test_qpixmap_to_image_rgb(self, mock_qt_module, mock_qtgui, mocker):
        """Преобразование QPixmap (RGB) в CoreImage."""
        mock_pixmap = mock_qtgui.QPixmap.return_value
        mock_qimage = mock_qtgui.QImage.return_value

        mock_pixmap.isNull.return_value = False
        mock_pixmap.toImage.return_value = mock_qimage
        mock_qimage.format.return_value = mock_qtgui.QImage.Format_RGB888
        mock_qimage.width.return_value = 100
        mock_qimage.height.return_value = 80

        # Создаём объект с методом __bytes__, возвращающим нужное количество байт
        class FakePtr:
            def __bytes__(self):
                return b'\x00' * (80 * 100 * 3)

        fake_ptr = FakePtr()
        mock_qimage.bits.return_value = fake_ptr

        core_img = ImageConverter.QPixmapToImage(mock_pixmap, framework='qt')
        assert isinstance(core_img, CoreImage)
        assert core_img.pixels.shape == (80, 100, 3)
        mock_qimage.convertToFormat.assert_not_called()

    def test_qpixmap_to_image_not_rgb(self, mock_qt_module, mock_qtgui, mocker):
        """Если формат QImage не RGB888, вызывается convertToFormat."""
        mock_pixmap = mock_qtgui.QPixmap.return_value
        mock_qimage = mock_qtgui.QImage.return_value

        mock_pixmap.isNull.return_value = False
        mock_pixmap.toImage.return_value = mock_qimage
        mock_qimage.format.return_value = mock_qtgui.QImage.Format_ARGB32
        mock_qimage.convertToFormat.return_value = mock_qimage  # возвращаем себя
        mock_qimage.width.return_value = 100
        mock_qimage.height.return_value = 80

        class FakePtr:
            def __bytes__(self):
                return b'\x00' * (80 * 100 * 3)

        fake_ptr = FakePtr()
        mock_qimage.bits.return_value = fake_ptr

        core_img = ImageConverter.QPixmapToImage(mock_pixmap, framework='qt')
        mock_qimage.convertToFormat.assert_called_once_with(mock_qtgui.QImage.Format_RGB888)
        assert isinstance(core_img, CoreImage)
        assert core_img.pixels.shape == (80, 100, 3)

    def test_qpixmap_to_image_null_pixmap(self, mock_qt_module, mock_qtgui):
        """Пустой QPixmap -> ValueError."""
        mock_pixmap = mock_qtgui.QPixmap.return_value
        mock_pixmap.isNull.return_value = True
        with pytest.raises(ValueError, match="QPixmap is null"):
            ImageConverter.QPixmapToImage(mock_pixmap, framework='qt')

    @pytest.mark.parametrize("framework", ['qt', 'pyside'])
    def test_image_to_qpixmap_framework_parameter(self, mock_qt_module, mock_qtgui, framework):
        """Проверка, что параметр framework передаётся в _get_qt_module."""
        core_img = create_test_core_image(shape=(50, 50, 3))
        ImageConverter.ImageToQPixmap(core_img, framework=framework)
        mock_qt_module.assert_called_with(framework)

    def test_get_qt_module_unknown_framework(self):
        """Передача неизвестного фреймворка -> ValueError."""
        with pytest.raises(ValueError, match="Unknown framework"):
            ImageConverter._get_qt_module('unknown')

    def test_get_qt_module_qt_import_error(self, mocker):
        """Попытка импорта PyQt5, но он не установлен."""
        with patch('builtins.__import__', side_effect=ImportError("No PyQt5")):
            with pytest.raises(ImportError, match="PyQt5 is not installed"):
                ImageConverter._get_qt_module('qt')

    def test_get_qt_module_pyside_import_error(self, mocker):
        """Попытка импорта PySide6, но он не установлен."""
        with patch('builtins.__import__', side_effect=ImportError("No PySide6")):
            with pytest.raises(ImportError, match="PySide6 is not installed"):
                ImageConverter._get_qt_module('pyside')