using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventor.Tests.Services;

public class EventServiceTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly Mock<ILogger<EventService>> _mockLogger;
    private readonly EventService _eventService;
    private readonly Guid _testUserId = Guid.NewGuid();
    private readonly Guid _testEventId = Guid.NewGuid();

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

    // GetAllEventsAsync
    [Fact]
    public async Task GetAllEventsAsync_ReturnsEvents()
    {
        // Arrange
        var events = new List<Event> { CreateTestEvent() };
        _mockRepo.Setup(r => r.GetAllEventsAsync()).ReturnsAsync(events);

        // Act
        var result = await _eventService.GetAllEventsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testEventId, result[0].Id);
    }

    [Fact]
    public async Task GetAllEventsAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllEventsAsync())
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<EventServiceException>(
            () => _eventService.GetAllEventsAsync());
    }

    // GetAllEventsByUserAsync
    [Fact]
    public async Task GetAllEventsByUserAsync_ReturnsUserEvents()
    {
        // Arrange
        var events = new List<Event> { CreateTestEvent() };
        _mockRepo.Setup(r => r.GetAllEventsByUserAsync(_testUserId))
            .ReturnsAsync(events);

        // Act
        var result = await _eventService.GetAllEventsByUserAsync(_testUserId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testEventId, result[0].Id);
    }

    [Fact]
    public async Task GetAllEventsByUserAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllEventsByUserAsync(_testUserId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<EventServiceException>(
            () => _eventService.GetAllEventsByUserAsync(_testUserId));
    }

    // GetEventByIdAsync
    [Fact]
    public async Task GetEventByIdAsync_ValidId_ReturnsEvent()
    {
        // Arrange
        var testEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.GetEventByIdAsync(_testEventId))
            .ReturnsAsync(testEvent);

        // Act
        var result = await _eventService.GetEventByIdAsync(_testEventId);

        // Assert
        Assert.Equal(_testEventId, result.Id);
    }

    [Fact]
    public async Task GetEventByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetEventByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(
            () => _eventService.GetEventByIdAsync(Guid.NewGuid()));
    }

    // AddEventAsync
    [Fact]
    public async Task AddEventAsync_ValidEvent_SavesSuccessfully()
    {
        // Arrange
        var testEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.InsertEventAsync(testEvent)).Returns(Task.CompletedTask);

        // Act
        await _eventService.AddEventAsync(testEvent);

        // Assert
        _mockRepo.Verify(r => r.InsertEventAsync(testEvent), Times.Once);
    }

    [Fact]
    public async Task AddEventAsync_DbException_ThrowsCreateException()
    {
        // Arrange
        var testEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.InsertEventAsync(testEvent))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<EventCreateException>(
            () => _eventService.AddEventAsync(testEvent));
    }

    // UpdateEventAsync
    [Fact]
    public async Task UpdateEventAsync_ValidEvent_UpdatesSuccessfully()
    {
        // Arrange
        var testEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.GetEventByIdAsync(_testEventId))
            .ReturnsAsync(testEvent);

        // Act
        await _eventService.UpdateEventAsync(testEvent);

        // Assert
        _mockRepo.Verify(r => r.UpdateEventAsync(testEvent), Times.Once);
    }

    [Fact]
    public async Task UpdateEventAsync_NonExistingEvent_ThrowsNotFoundException()
    {
        // Arrange
        var testEvent = CreateTestEvent();
        _mockRepo.Setup(r => r.GetEventByIdAsync(_testEventId))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(
            () => _eventService.UpdateEventAsync(testEvent));
    }

    // DeleteEventAsync
    [Fact]
    public async Task DeleteEventAsync_ValidId_DeletesSuccessfully()
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
    public async Task DeleteEventAsync_NonExistingEvent_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteEventAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(
            () => _eventService.DeleteEventAsync(Guid.NewGuid()));
    }
}