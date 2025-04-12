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

public class EventServiceTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly Mock<ILogger<EventService>> _mockLogger;
    private readonly EventService _eventService;
    private readonly Guid _testUserId = Guid.NewGuid();
    private readonly Guid _testEventId = Guid.NewGuid();
    private readonly Guid _nonExistingEventId = Guid.NewGuid();

    public EventServiceTests()
    {
        _mockRepo = new Mock<IEventRepository>();
        _mockLogger = new Mock<ILogger<EventService>>();
        _eventService = new EventService(_mockRepo.Object, _mockLogger.Object);
    }

    private Event CreateTestEvent() => new Event(
        _testEventId,
        Guid.NewGuid(),
        "Test Event",
        "Test Description",
        DateOnly.FromDateTime(DateTime.Now),
        10,
        3,
        10.0,
        7.5
    );

    [Fact]
    public async Task GetAllEventsAsync_ReturnsEvents_WhenRepositoryReturnsData()
    {
        // Arrange
        var expectedEvents = new List<Event> { CreateTestEvent() };
        _mockRepo.Setup(r => r.GetAllEventsAsync()).ReturnsAsync(expectedEvents);

        // Act
        var result = await _eventService.GetAllEventsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testEventId, result[0].Id);
        _mockRepo.Verify(r => r.GetAllEventsAsync(), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    [InlineData(typeof(Exception))]
    public async Task GetAllEventsAsync_ThrowsServiceException_OnAnyError(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.GetAllEventsAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<EventServiceException>(
            () => _eventService.GetAllEventsAsync());
    }

    [Fact]
    public async Task GetAllEventsByUserAsync_ReturnsEvents_ForValidUserId()
    {
        // Arrange
        var expectedEvents = new List<Event> { CreateTestEvent() };
        _mockRepo.Setup(r => r.GetAllEventsByUserAsync(_testUserId))
            .ReturnsAsync(expectedEvents);

        // Act
        var result = await _eventService.GetAllEventsByUserAsync(_testUserId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testEventId, result[0].Id);
        _mockRepo.Verify(r => r.GetAllEventsByUserAsync(_testUserId), Times.Once);
    }

    [Fact]
    public async Task GetEventByIdAsync_ReturnsEvent_WhenExists()
    {
        // Arrange
        var expectedEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.GetEventByIdAsync(_testEventId))
            .ReturnsAsync(expectedEvent);

        // Act
        var result = await _eventService.GetEventByIdAsync(_testEventId);

        // Assert
        Assert.Equal(expectedEvent.Id, result.Id);
        _mockRepo.Verify(r => r.GetEventByIdAsync(_testEventId), Times.Once);
    }

    [Fact]
    public async Task GetEventByIdAsync_ThrowsNotFoundException_WhenEventNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetEventByIdAsync(_nonExistingEventId))
            .ThrowsAsync(new ArgumentException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(
            () => _eventService.GetEventByIdAsync(_nonExistingEventId));
    }

    [Fact]
    public async Task AddEventAsync_SavesEvent_WhenValid()
    {
        // Arrange
        var newEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.InsertEventAsync(newEvent))
            .Returns(Task.CompletedTask);

        // Act
        await _eventService.AddEventAsync(newEvent);

        // Assert
        _mockRepo.Verify(r => r.InsertEventAsync(It.Is<Event>(e =>
            e.Id == newEvent.Id &&
            e.Name == newEvent.Name)),
            Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task AddEventAsync_ThrowsCreateException_OnFailure(Type exceptionType)
    {
        // Arrange
        var newEvent = CreateTestEvent();
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.InsertEventAsync(newEvent)).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<EventCreateException>(
            () => _eventService.AddEventAsync(newEvent));
    }

    [Fact]
    public async Task UpdateEventAsync_UpdatesExistingEvent_WhenValid()
    {
        // Arrange
        var existingEvent = CreateTestEvent();
        var updatedEvent = existingEvent;
        updatedEvent.Name = "Updated Name";

        _mockRepo.Setup(r => r.GetEventByIdAsync(existingEvent.Id))
            .ReturnsAsync(existingEvent);
        _mockRepo.Setup(r => r.UpdateEventAsync(updatedEvent))
            .Returns(Task.CompletedTask);

        // Act
        await _eventService.UpdateEventAsync(updatedEvent);

        // Assert
        _mockRepo.Verify(r => r.UpdateEventAsync(It.Is<Event>(e =>
            e.Id == updatedEvent.Id &&
            e.Name == "Updated Name")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateEventAsync_ThrowsNotFoundException_WhenEventNotExists()
    {
        // Arrange
        var nonExistingEvent = CreateTestEvent();
        nonExistingEvent.Id = _nonExistingEventId;
        _mockRepo.Setup(r => r.GetEventByIdAsync(_nonExistingEventId))
            .ThrowsAsync(new ArgumentException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(
            () => _eventService.UpdateEventAsync(nonExistingEvent));
    }

    [Fact]
    public async Task DeleteEventAsync_DeletesExistingEvent_WhenValid()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteEventAsync(_testEventId))
            .Returns(Task.CompletedTask);

        // Act
        await _eventService.DeleteEventAsync(_testEventId);

        // Assert
        _mockRepo.Verify(r => r.DeleteEventAsync(_testEventId), Times.Once);
    }

    [Fact]
    public async Task DeleteEventAsync_ThrowsNotFoundException_WhenEventNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteEventAsync(_nonExistingEventId))
            .ThrowsAsync(new ArgumentException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(
            () => _eventService.DeleteEventAsync(_nonExistingEventId));
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task DeleteEventAsync_ThrowsDeleteException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.DeleteEventAsync(_testEventId)).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<EventDeleteException>(
            () => _eventService.DeleteEventAsync(_testEventId));
    }
}