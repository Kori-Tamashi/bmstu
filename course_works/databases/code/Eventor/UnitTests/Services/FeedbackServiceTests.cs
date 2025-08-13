using Eventor.Common.Core;
using Eventor.Services.Exceptions;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Database.Core;
using Eventor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eventor.Tests.Services;

public class FeedbackServiceTests
{
    private readonly Mock<IFeedbackRepository> _mockRepo;
    private readonly Mock<ILogger<FeedbackService>> _mockLogger;
    private readonly FeedbackService _feedbackService;
    private readonly Guid _testEventId = Guid.NewGuid();
    private readonly Guid _testUserId = Guid.NewGuid();
    private readonly Guid _testFeedbackId = Guid.NewGuid();
    private readonly Guid _nonExistingFeedbackId = Guid.NewGuid();

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

    [Fact]
    public async Task GetAllFeedbackAsync_ReturnsFeedback_WhenDataExists()
    {
        // Arrange
        var expectedFeedback = new List<Feedback> { CreateTestFeedback() };
        _mockRepo.Setup(r => r.GetAllFeedbackAsync()).ReturnsAsync(expectedFeedback);

        // Act
        var result = await _feedbackService.GetAllFeedbackAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testFeedbackId, result[0].Id);
        _mockRepo.Verify(r => r.GetAllFeedbackAsync(), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    [InlineData(typeof(Exception))]
    public async Task GetAllFeedbackAsync_ThrowsServiceException_OnErrors(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.GetAllFeedbackAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackServiceException>(
            () => _feedbackService.GetAllFeedbackAsync());
    }

    [Fact]
    public async Task GetAllFeedbacksByEventAsync_ReturnsFilteredResults()
    {
        // Arrange
        var feedbacks = new List<Feedback> { CreateTestFeedback() };
        _mockRepo.Setup(r => r.GetAllFeedbacksByEventAsync(_testEventId))
            .ReturnsAsync(feedbacks);

        // Act
        var result = await _feedbackService.GetAllFeedbacksByEventAsync(_testEventId);

        // Assert
        Assert.All(result, f => Assert.Equal(_testEventId, f.EventId));
        _mockRepo.Verify(r => r.GetAllFeedbacksByEventAsync(_testEventId), Times.Once);
    }

    [Fact]
    public async Task GetAllFeedbacksByUserAsync_ValidUserId_ReturnsUserFeedback()
    {
        // Arrange
        var feedbacks = new List<Feedback> { CreateTestFeedback() };
        _mockRepo.Setup(r => r.GetAllFeedbacksByUserAsync(_testUserId))
            .ReturnsAsync(feedbacks);

        // Act
        var result = await _feedbackService.GetAllFeedbacksByUserAsync(_testUserId);

        // Assert
        Assert.All(result, f => Assert.Equal(_testUserId, f.PersonId));
        _mockRepo.Verify(r => r.GetAllFeedbacksByUserAsync(_testUserId), Times.Once);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_ExistingId_ReturnsFeedback()
    {
        // Arrange
        var expectedFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync(expectedFeedback);

        // Act
        var result = await _feedbackService.GetFeedbackByIdAsync(_testFeedbackId);

        // Assert
        Assert.Equal(expectedFeedback.Comment, result.Comment);
        _mockRepo.Verify(r => r.GetFeedbackByIdAsync(_testFeedbackId), Times.Once);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_nonExistingFeedbackId))
            .ReturnsAsync((Feedback)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FeedbackNotFoundException>(
            () => _feedbackService.GetFeedbackByIdAsync(_nonExistingFeedbackId));

        Assert.Contains(_nonExistingFeedbackId.ToString(), ex.Message);
    }

    [Fact]
    public async Task AddFeedbackAsync_ValidData_CallsRepository()
    {
        // Arrange
        var newFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.InsertFeedbackAsync(newFeedback))
            .Returns(Task.CompletedTask);

        // Act
        await _feedbackService.AddFeedbackAsync(newFeedback);

        // Assert
        _mockRepo.Verify(r => r.InsertFeedbackAsync(It.Is<Feedback>(f =>
            f.Id == newFeedback.Id &&
            f.Comment == newFeedback.Comment)),
            Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(ArgumentException))]
    public async Task AddFeedbackAsync_ThrowsCreateException_OnFailure(Type exceptionType)
    {
        // Arrange
        var newFeedback = CreateTestFeedback();
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.InsertFeedbackAsync(newFeedback)).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackCreateException>(
            () => _feedbackService.AddFeedbackAsync(newFeedback));
    }

    [Fact]
    public async Task UpdateFeedbackAsync_ValidData_UpdatesEntity()
    {
        // Arrange
        var existingFeedback = CreateTestFeedback();
        var updatedFeedback = existingFeedback;
        updatedFeedback.Comment = "Updated Comment";

        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(existingFeedback.Id))
            .ReturnsAsync(existingFeedback);
        _mockRepo.Setup(r => r.UpdateFeedbackAsync(updatedFeedback))
            .Returns(Task.CompletedTask);

        // Act
        await _feedbackService.UpdateFeedbackAsync(updatedFeedback);

        // Assert
        _mockRepo.Verify(r => r.UpdateFeedbackAsync(It.Is<Feedback>(f =>
            f.Id == updatedFeedback.Id &&
            f.Comment == "Updated Comment")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateFeedbackAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var feedback = CreateTestFeedback();
        feedback.Id = _nonExistingFeedbackId;
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_nonExistingFeedbackId))
            .ReturnsAsync((Feedback)null);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackNotFoundException>(
            () => _feedbackService.UpdateFeedbackAsync(feedback));
    }

    [Fact]
    public async Task DeleteFeedbackAsync_ExistingId_DeletesEntity()
    {
        // Arrange
        var existingFeedback = CreateTestFeedback();
        _mockRepo.Setup(r => r.GetFeedbackByIdAsync(_testFeedbackId))
            .ReturnsAsync(existingFeedback);

        // Act
        await _feedbackService.DeleteFeedbackAsync(_testFeedbackId);

        // Assert
        _mockRepo.Verify(r => r.DeleteFeedbackAsync(_testFeedbackId), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task DeleteFeedbackAsync_ThrowsDeleteException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.DeleteFeedbackAsync(_testFeedbackId)).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<FeedbackDeleteException>(
            () => _feedbackService.DeleteFeedbackAsync(_testFeedbackId));
    }
}