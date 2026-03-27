# Tests/Unit/Services/DeepfakeDetectorService_Unit_test.py

import pytest
from pytest_mock import MockerFixture
import torch
import numpy as np
from typing import Dict, Any, Tuple

from Backend.Core.Entities.Image import Image as CoreImage
from Backend.Core.Entities.DeepfakeDetectorSettings import DeepfakeDetectorSettings
from Backend.Core.Repositories.Interfaces.IDeepfakeDetectorSettingsRepository import IDeepfakeDetectorSettingsRepository
from Backend.Core.Repositories.Interfaces.IDeepfakeDetectorRepository import IDeepfakeDetectorRepository
from Backend.BusinessLogic.Services.DeepfakeDetectorService import DeepfakeDetectorService
from Backend.Core.Services.Exceptions.DeepfakeDetectorServiceExceptions import (
    SettingsLoadError,
    DetectionError,
)
from Backend.Core.Converters.ImageConverter import ImageConverter


# ----------------------------------------------------------------------
# Object Mother – фабрика тестовых данных
# ----------------------------------------------------------------------
class DeepfakeDetectorObjectMother:
    """Предоставляет готовые объекты для тестов сервиса."""

    @staticmethod
    def create_core_image(height: int = 224, width: int = 224, channels: int = 3) -> CoreImage:
        pixels = np.zeros((height, width, channels), dtype=np.uint8)
        return CoreImage(pixels=pixels)

    @staticmethod
    def create_settings(num_classes: int = 1, threshold: float = 0.5) -> DeepfakeDetectorSettings:
        return DeepfakeDetectorSettings(
            weights={'some.weight': torch.tensor([1.0])},
            cnn_model_name='efficientnet_b0',
            vit_model_name='google/vit-base-patch16-224-in21k',
            fusion_dim=128,
            dropout_rate=0.2,
            num_classes=num_classes,
            threshold=threshold,
            input_size=(224, 224)
        )

    @staticmethod
    def create_mock_modules(mocker: MockerFixture) -> Dict[str, Any]:
        """Создаёт словарь с моками модулей для возврата из репозитория."""
        mock_cnn = mocker.MagicMock()
        mock_vit = mocker.MagicMock()
        mock_cnn_proj = mocker.MagicMock()
        mock_vit_proj = mocker.MagicMock()
        mock_classifier = mocker.MagicMock()
        # Мокаем поведение vit: он возвращает объект с last_hidden_state
        mock_vit_out = mocker.MagicMock()
        mock_vit_out.last_hidden_state = torch.randn(1, 197, 768)  # [CLS] на позиции 0
        mock_vit.return_value = mock_vit_out
        return {
            'cnn': mock_cnn,
            'vit': mock_vit,
            'cnn_proj': mock_cnn_proj,
            'vit_proj': mock_vit_proj,
            'classifier': mock_classifier,
            'device': torch.device('cpu')
        }


# ----------------------------------------------------------------------
# Фикстуры
# ----------------------------------------------------------------------
@pytest.fixture
def mock_settings_repo(mocker: MockerFixture):
    """Мок репозитория настроек."""
    return mocker.MagicMock(spec=IDeepfakeDetectorSettingsRepository)


@pytest.fixture
def mock_model_repo(mocker: MockerFixture):
    """Мок репозитория моделей."""
    return mocker.MagicMock(spec=IDeepfakeDetectorRepository)


@pytest.fixture
def mock_converter(mocker: MockerFixture):
    """Мок статических методов ImageConverter."""
    mocker.patch.object(ImageConverter, 'ImageToTensor', autospec=True)
    return ImageConverter.ImageToTensor


@pytest.fixture
def service(mock_settings_repo, mock_model_repo) -> DeepfakeDetectorService:
    """Сервис с замоканными репозиториями."""
    return DeepfakeDetectorService(
        settings_repository=mock_settings_repo,
        model_repository=mock_model_repo
    )


@pytest.mark.asyncio
class TestDeepfakeDetectorService:
    """Модульные тесты для DeepfakeDetectorService (лондонский стиль)."""

    # ------------------------------------------------------------------
    # Тесты для GetSettingsAsync
    # ------------------------------------------------------------------
    async def test_get_settings_success(
        self,
        service: DeepfakeDetectorService,
        mock_settings_repo: IDeepfakeDetectorSettingsRepository,
    ):
        """Успешное получение настроек."""
        # Arrange
        path = "model.pth"
        expected_settings = DeepfakeDetectorObjectMother.create_settings()
        mock_settings_repo.GetDeepfakeDetectorSettingsAsync.return_value = expected_settings

        # Act
        result = await service.GetSettingsAsync(path)

        # Assert
        assert result is expected_settings
        mock_settings_repo.GetDeepfakeDetectorSettingsAsync.assert_called_once_with(path)

    async def test_get_settings_repository_error(
        self,
        service: DeepfakeDetectorService,
        mock_settings_repo: IDeepfakeDetectorSettingsRepository,
    ):
        """Ошибка репозитория при загрузке настроек."""
        # Arrange
        path = "model.pth"
        mock_settings_repo.GetDeepfakeDetectorSettingsAsync.side_effect = Exception("Repo error")

        # Act & Assert
        with pytest.raises(SettingsLoadError) as excinfo:
            await service.GetSettingsAsync(path)
        assert "Ошибка загрузки настроек" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, Exception)

    # ------------------------------------------------------------------
    # Тесты для DetectAsync
    # ------------------------------------------------------------------
    async def test_detect_success_binary_class(
        self,
        service: DeepfakeDetectorService,
        mock_model_repo: IDeepfakeDetectorRepository,
        mock_converter,
        mocker: MockerFixture,
    ):
        """Успешная детекция для бинарной классификации (num_classes=1)."""
        # Arrange
        input_image = DeepfakeDetectorObjectMother.create_core_image()
        settings = DeepfakeDetectorObjectMother.create_settings(num_classes=1, threshold=0.5)

        # Мокаем репозиторий моделей
        mock_modules = DeepfakeDetectorObjectMother.create_mock_modules(mocker)
        mock_model_repo.LoadModelAsync.return_value = mock_modules

        # Мокаем ImageConverter.ImageToTensor
        mock_tensor = torch.randn(3, 224, 224)  # без batch
        mock_converter.return_value = mock_tensor

        # Мокаем forward через модули
        # cnn возвращает некий вектор признаков
        cnn_feat = torch.randn(1, 1280)  # batch=1, dim
        mock_modules['cnn'].return_value = cnn_feat

        # vit возвращаем через ранее настроенный мок (в create_mock_modules уже есть)
        # проекции
        cnn_emb = torch.randn(1, settings.fusion_dim)
        vit_emb = torch.randn(1, settings.fusion_dim)
        mock_modules['cnn_proj'].return_value = cnn_emb
        mock_modules['vit_proj'].return_value = vit_emb

        # классификатор выдаёт логит
        logits = torch.tensor([[0.8]])  # после сигмоиды даст 0.69
        mock_modules['classifier'].return_value = logits

        # Act
        prob, is_fake = await service.DetectAsync(input_image, settings)

        # Assert
        expected_prob = torch.sigmoid(logits).item()
        expected_is_fake = expected_prob >= settings.threshold
        assert prob == expected_prob
        assert is_fake == expected_is_fake

        # Проверяем вызовы
        mock_model_repo.LoadModelAsync.assert_called_once_with(settings)
        mock_converter.assert_called_once_with(input_image, normalize=True)
        # Проверим, что тензор был передан в cnn (с batch)
        mock_modules['cnn'].assert_called_once()
        # args[0] должен быть тензором с batch
        call_args = mock_modules['cnn'].call_args[0][0]
        assert call_args.shape == (1, 3, 224, 224)

        # vit вызывается с pixel_values
        mock_modules['vit'].assert_called_once_with(pixel_values=mocker.ANY)
        # проекции вызываются
        mock_modules['cnn_proj'].assert_called_once_with(cnn_feat)
        mock_modules['vit_proj'].assert_called_once_with(mocker.ANY)  # vit_feat
        # классификатор с объединённым вектором
        mock_modules['classifier'].assert_called_once_with(mocker.ANY)

    async def test_detect_success_multiclass(
        self,
        service: DeepfakeDetectorService,
        mock_model_repo: IDeepfakeDetectorRepository,
        mock_converter,
        mocker: MockerFixture,
    ):
        """Успешная детекция для мультикласса (num_classes=2, softmax)."""
        # Arrange
        input_image = DeepfakeDetectorObjectMother.create_core_image()
        settings = DeepfakeDetectorObjectMother.create_settings(num_classes=2, threshold=0.7)

        mock_modules = DeepfakeDetectorObjectMother.create_mock_modules(mocker)
        mock_model_repo.LoadModelAsync.return_value = mock_modules
        mock_tensor = torch.randn(3, 224, 224)
        mock_converter.return_value = mock_tensor

        # Мокаем forward
        cnn_feat = torch.randn(1, 1280)
        mock_modules['cnn'].return_value = cnn_feat
        # vit уже замокан
        cnn_emb = torch.randn(1, settings.fusion_dim)
        vit_emb = torch.randn(1, settings.fusion_dim)
        mock_modules['cnn_proj'].return_value = cnn_emb
        mock_modules['vit_proj'].return_value = vit_emb
        # классификатор выдаёт логиты для двух классов
        logits = torch.tensor([[0.2, 1.5]])
        mock_modules['classifier'].return_value = logits

        # Act
        prob, is_fake = await service.DetectAsync(input_image, settings)

        # Assert
        expected_probs = torch.softmax(logits, dim=1)
        expected_prob_fake = expected_probs[0, 1].item()
        expected_is_fake = expected_prob_fake >= settings.threshold
        assert prob == expected_prob_fake
        assert is_fake == expected_is_fake

    async def test_detect_model_repository_error(
        self,
        service: DeepfakeDetectorService,
        mock_model_repo: IDeepfakeDetectorRepository,
        mock_converter,
        mocker: MockerFixture,
    ):
        """Ошибка при загрузке модели из репозитория."""
        input_image = DeepfakeDetectorObjectMother.create_core_image()
        settings = DeepfakeDetectorObjectMother.create_settings()
        mock_model_repo.LoadModelAsync.side_effect = Exception("Model load error")

        with pytest.raises(DetectionError) as excinfo:
            await service.DetectAsync(input_image, settings)
        assert "Ошибка детекции" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, Exception)

    async def test_detect_prepare_tensor_error(
        self,
        service: DeepfakeDetectorService,
        mock_model_repo: IDeepfakeDetectorRepository,
        mock_converter,
        mocker: MockerFixture,
    ):
        """Ошибка при подготовке тензора (ImageConverter бросает исключение)."""
        input_image = DeepfakeDetectorObjectMother.create_core_image()
        settings = DeepfakeDetectorObjectMother.create_settings()
        mock_modules = DeepfakeDetectorObjectMother.create_mock_modules(mocker)
        mock_model_repo.LoadModelAsync.return_value = mock_modules
        mock_converter.side_effect = RuntimeError("Conversion error")

        with pytest.raises(DetectionError) as excinfo:
            await service.DetectAsync(input_image, settings)
        assert "Ошибка детекции" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)

    async def test_detect_forward_error(
        self,
        service: DeepfakeDetectorService,
        mock_model_repo: IDeepfakeDetectorRepository,
        mock_converter,
        mocker: MockerFixture,
    ):
        """Ошибка во время прямого прохода (например, в CNN)."""
        input_image = DeepfakeDetectorObjectMother.create_core_image()
        settings = DeepfakeDetectorObjectMother.create_settings()
        mock_modules = DeepfakeDetectorObjectMother.create_mock_modules(mocker)
        mock_model_repo.LoadModelAsync.return_value = mock_modules
        mock_tensor = torch.randn(3, 224, 224)
        mock_converter.return_value = mock_tensor

        # Мокаем cnn, чтобы он бросил исключение
        mock_modules['cnn'].side_effect = RuntimeError("CNN error")

        with pytest.raises(DetectionError) as excinfo:
            await service.DetectAsync(input_image, settings)
        assert "Ошибка детекции" in str(excinfo.value)
        assert isinstance(excinfo.value.__cause__, RuntimeError)