using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventor.Services;

namespace Eventor.Tests.Services;

public class FeedbackServiceTests
{
    private readonly Mock<IFeedbackRepository> _mockRepo;
    private readonly Mock<ILogger<FeedbackService>> _mockLogger;
    private readonly FeedbackService _feedbackService;
    private readonly Guid _testEventId = Guid.NewGuid();
    private readonly Guid _testUserId = Guid.NewGuid();
    private readonly Guid _testFeedbackId = Guid.NewGuid();

    public FeedbackServiceTests()
    {
        _mockRepo = new Mock<IFeedbackRepository>();
        _mockLogger = new Mock<ILogger<FeedbackService>>();
        _feedbackService = new FeedbackService(_mockRepo.Object, _mockLogger.Object);
    }

    private Feedback CreateTestFeedback() => new Feedback(
        _testFeedbackId,
        _testEventId,
        _testUserId,
        "Test Comment",
        5
    );

    // GetAllFeedbackAsync
    [Fact]
    public async Task GetAllFeedbackAsync_ReturnsFeedbackList()
    {
        // Arrange
        var feedbacks = new List<Feedback> { CreateTestFeedback() };
        _mockRepo.Setup(r => r.GetAllFeedbackAsync()).ReturnsAsync(feedbacks);

        // Act
        var result = await _feedbackService.GetAllFeedbackAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testFeedbackId, result[0].Id);
    }

    [Fact]
    public async Task GetAllFeedbackAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllFeedbackAsync())
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackServiceException>(
            () => _feedbackService.GetAllFeedbackAsync());
    }

    // GetAllFeedbacksByEventAsync
    [Fact]
    public async Task GetAllFeedbacksByEventAsync_ReturnsFilteredFeedback()
    {
        // Arrange
        var feedbacks = new List<Feedback> { CreateTestFeedback() };
        _mockRepo.Setup(r => r.GetAllFeedbacksByEventAsync(_testEventId))
            .ReturnsAsync(feedbacks);

        // Act
        var result = await _feedbackService.GetAllFeedbacksByEventAsync(_testEventId);

        // Assert
        Assert.Equal(_testEventId, result[0].EventId);
    }

    [Fact]
    public async Task GetAllFeedbacksByEventAsync_EmptyResult_LogsInformation()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllFeedbacksByEventAsync(_testEventId))
            .ReturnsAsync(new List<Feedback>());

        // Act
        var result = await _feedbackService.GetAllFeedbacksByEventAsync(_testEventId);

        // Assert
        Assert.Empty(result);
        // Можно добавить VerifyLog для проверки логов
    }

    // GetAllFeedbacksByUserAsync
    [Fact]
    public async Task GetAllFeedbacksByUserAsync_ReturnsUserFeedback()
    {
        // Arrange
        var feedbacks = new List<Feedback> { CreateTestFeedback() };
        _mockRepo.Setup(r => r.GetAllFeedbacksByUserAsync(_testUserId))
            .ReturnsAsync(feedbacks);

        // Act
        var result = await _feedbackService.GetAllFeedbacksByUserAsync(_testUserId);

        // Assert
        Assert.Equal(_testUserId, result[0].PersonId);
    }

    // GetFeedbackByIdAsync
    [Fact]
    public async Task GetFeedbackByIdAsync_ValidId_ReturnsFeedback()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync(testFeedback);

        // Act
        var result = await _feedbackService.GetFeedbackByIdAsync(_testFeedbackId);

        // Assert
        Assert.Equal("Test Comment", result.Comment);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_NotFound_ThrowsException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Feedback)null!);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackNotFoundException>(
            () => _feedbackService.GetFeedbackByIdAsync(Guid.NewGuid()));
    }

    // AddFeedbackAsync
    [Fact]
    public async Task AddFeedbackAsync_ValidData_SavesSuccessfully()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.InsertFeedbackAsync(testFeedback))
            .Returns(Task.CompletedTask);

        // Act
        await _feedbackService.AddFeedbackAsync(testFeedback);

        // Assert
        _mockRepo.Verify(r => r.InsertFeedbackAsync(testFeedback), Times.Once);
    }

    [Fact]
    public async Task AddFeedbackAsync_DbException_ThrowsCreateException()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.InsertFeedbackAsync(testFeedback))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackCreateException>(
            () => _feedbackService.AddFeedbackAsync(testFeedback));
    }

    // UpdateFeedbackAsync
    [Fact]
    public async Task UpdateFeedbackAsync_ValidData_UpdatesSuccessfully()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync(testFeedback);

        // Act
        await _feedbackService.UpdateFeedbackAsync(testFeedback);

        // Assert
        _mockRepo.Verify(r => r.UpdateFeedbackAsync(testFeedback), Times.Once);
    }

    [Fact]
    public async Task UpdateFeedbackAsync_NotFound_ThrowsException()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync((Feedback)null!);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackNotFoundException>(
            () => _feedbackService.UpdateFeedbackAsync(testFeedback));
    }

    // DeleteFeedbackAsync
    [Fact]
    public async Task DeleteFeedbackAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync(testFeedback);

        // Act
        await _feedbackService.DeleteFeedbackAsync(_testFeedbackId);

        // Assert
        _mockRepo.Verify(r => r.DeleteFeedbackAsync(_testFeedbackId), Times.Once);
    }

    [Fact]
    public async Task DeleteFeedbackAsync_NotFound_ThrowsException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync((Feedback)null!);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackNotFoundException>(
            () => _feedbackService.DeleteFeedbackAsync(_testFeedbackId));
    }
}