import numpy as np
import torch
from PIL import Image as PilImage
from Backend.Core.Entities.Image import Image as CoreImage
from torchvision import transforms
from dataclasses import dataclass
from typing import Union


class ImageConverter:
    """Преобразователь между доменным Image, PIL Image, тензором PyTorch и QPixmap."""

    @staticmethod
    def _get_qt_module(framework='qt'):
        """
        Возвращает модуль QtGui для указанного фреймворка.
        framework: 'qt' для PyQt5, 'pyside' для PySide6.
        """
        if framework == 'qt':
            try:
                from PyQt5 import QtGui
                return QtGui
            except ImportError:
                raise ImportError("PyQt5 is not installed")
        elif framework == 'pyside':
            try:
                from PySide6 import QtGui
                return QtGui
            except ImportError:
                raise ImportError("PySide6 is not installed")
        else:
            raise ValueError(f"Unknown framework: {framework}")

    # ------------------------------------------------------------------
    # Методы для Pillow и тензоров (без изменений)
    # ------------------------------------------------------------------
    @staticmethod
    def PillowToImage(pil_image: PilImage) -> 'CoreImage':
        """Преобразует PIL Image в доменный Image."""
        if pil_image.mode == 'RGB':
            rgb = pil_image
        elif pil_image.mode in ('L', 'RGBA'):
            rgb = pil_image.convert('RGB')
        else:
            raise ValueError(
                f"Unsupported PIL image mode: {pil_image.mode}. "
                f"Expected 'RGB', 'L', or 'RGBA'."
            )
        np_array = np.array(rgb, dtype=np.uint8)
        return CoreImage(pixels=np_array)

    @staticmethod
    def ImageToPillow(image: 'Image') -> PilImage:
        """Преобразует доменный Image в PIL Image."""
        pixels = image.pixels
        if pixels.dtype != np.uint8:
            if pixels.dtype in (np.float32, np.float64) and pixels.max() <= 1.0:
                pixels = (pixels * 255).astype(np.uint8)
            else:
                pixels = np.clip(pixels, 0, 255).astype(np.uint8)

        channels = pixels.shape[2]
        if channels == 1:
            mode = 'L'
            pixels_2d = pixels[:, :, 0]
        elif channels == 3:
            mode = 'RGB'
            pixels_2d = pixels
        elif channels == 4:
            mode = 'RGBA'
            pixels_2d = pixels
        else:
            raise ValueError(f"Unsupported number of channels: {channels}. Expected 1, 3, or 4.")
        return PilImage.fromarray(pixels_2d, mode=mode)

    @staticmethod
    def ImageToTensor(
        image: 'Image',
        normalize: bool = True,
        mean: tuple = (0.485, 0.456, 0.406),
        std: tuple = (0.229, 0.224, 0.225)
    ) -> torch.Tensor:
        """
        Преобразует доменный Image в тензор PyTorch с опциональной нормализацией.
        """
        pil_image = ImageConverter.ImageToPillow(image)
        tensor = transforms.ToTensor()(pil_image)  # (C, H, W), значения в [0,1]
        if normalize:
            tensor = transforms.Normalize(mean=mean, std=std)(tensor)
        return tensor

    @staticmethod
    def TensorToImage(
        tensor: torch.Tensor,
        denormalize: bool = True,
        mean: tuple = (0.485, 0.456, 0.406),
        std: tuple = (0.229, 0.224, 0.225)
    ) -> 'Image':
        """
        Преобразует тензор PyTorch обратно в доменный Image.
        """
        if tensor.dim() == 4:
            tensor = tensor.squeeze(0)  # (C, H, W)
        if tensor.dim() != 3:
            raise ValueError(f"Expected 3D or 4D tensor, got {tensor.dim()}D")

        if denormalize:
            mean_t = torch.tensor(mean).view(3, 1, 1)
            std_t = torch.tensor(std).view(3, 1, 1)
            tensor = tensor * std_t + mean_t

        np_array = (tensor.cpu().numpy().clip(0, 1) * 255).astype(np.uint8)  # (C, H, W)
        np_array = np_array.transpose(1, 2, 0)  # (H, W, C)
        return CoreImage(pixels=np_array)

    # ------------------------------------------------------------------
    # Методы для Qt (с поддержкой PyQt5 и PySide6)
    # ------------------------------------------------------------------
    @staticmethod
    def ImageToQPixmap(image: 'CoreImage', framework='qt'):
        """
        Преобразует доменный Image в QPixmap для отображения в Qt.

        Args:
            image: Доменное изображение.
            framework: 'qt' для PyQt5, 'pyside' для PySide6.

        Returns:
            QPixmap: Изображение, готовое к отображению в Qt.

        Raises:
            ValueError: Если число каналов не 1, 3 или 4.
        """
        QtGui = ImageConverter._get_qt_module(framework)

        pixels = image.pixels
        if pixels.dtype != np.uint8:
            if pixels.dtype in (np.float32, np.float64) and pixels.max() <= 1.0:
                pixels = (pixels * 255).astype(np.uint8)
            else:
                pixels = np.clip(pixels, 0, 255).astype(np.uint8)

        height, width, channels = pixels.shape

        if channels == 3:
            fmt = QtGui.QImage.Format_RGB888
            bytes_per_line = 3 * width
        elif channels == 4:
            fmt = QtGui.QImage.Format_RGBA8888
            bytes_per_line = 4 * width
        elif channels == 1:
            # Для отображения grayscale конвертируем в RGB
            pixels = np.repeat(pixels, 3, axis=2)
            fmt = QtGui.QImage.Format_RGB888
            bytes_per_line = 3 * width
        else:
            raise ValueError(f"Unsupported number of channels: {channels}. Expected 1, 3, or 4.")

        # Важно: данные должны быть непрерывными в памяти
        pixels = np.ascontiguousarray(pixels)

        qimage = QtGui.QImage(pixels.data, width, height, bytes_per_line, fmt)
        qimage = qimage.copy()  # копируем, чтобы данные не зависели от numpy
        return QtGui.QPixmap.fromImage(qimage)

    @staticmethod
    def QPixmapToImage(pixmap, framework='qt') -> 'CoreImage':
        """
        Преобразует QPixmap в доменный Image.

        Args:
            pixmap: QPixmap (из выбранного фреймворка).
            framework: 'qt' для PyQt5, 'pyside' для PySide6.

        Returns:
            CoreImage: Доменное изображение.

        Raises:
            ValueError: Если pixmap пуст или не удаётся преобразовать.
        """
        QtGui = ImageConverter._get_qt_module(framework)

        if pixmap.isNull():
            raise ValueError("QPixmap is null")

        qimage = pixmap.toImage()
        if qimage.format() != QtGui.QImage.Format_RGB888:
            qimage = qimage.convertToFormat(QtGui.QImage.Format_RGB888)

        width = qimage.width()
        height = qimage.height()
        ptr = qimage.bits()

        # Универсальное получение байтов из ptr
        try:
            # PySide6: memoryview -> bytes
            data = bytes(ptr)
        except TypeError:
            # PyQt5: sip.voidptr -> asarray
            data = ptr.asarray(height * width * 3).tobytes()

        arr = np.frombuffer(data, dtype=np.uint8).reshape(height, width, 3).copy()
        return CoreImage(pixels=arr)