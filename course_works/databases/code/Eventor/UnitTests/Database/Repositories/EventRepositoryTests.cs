using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

public class EventRepositoryTests : IDisposable
{
    private readonly EventorDBContext _context;
    private readonly ILogger<EventRepository> _logger;
    private readonly EventRepository _repository;
    private readonly DbContextOptions<EventorDBContext> _options;

    public EventRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventorDBContext(_options);
        _logger = Mock.Of<ILogger<EventRepository>>();
        _repository = new EventRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllEventsAsync_ShouldReturnOrderedEvents()
    {
        // Arrange
        var events = new List<EventDBModel>
        {
            CreateTestEvent(DateTime.Now.AddDays(1)),
            CreateTestEvent(DateTime.Now)
        };

        await _context.Events.AddRangeAsync(events);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllEventsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date < result[1].Date);
    }

    [Fact]
    public async Task GetAllEventsByUserAsync_ShouldReturnUserEvents()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEvent = new UserEventDBModel(userId, Guid.NewGuid())
        {
            Event = CreateTestEvent(DateTime.Now)
        };

        await _context.UsersEvents.AddAsync(userEvent);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllEventsByUserAsync(userId);

        // Assert
        Assert.Single(result);
        Assert.Equal(userEvent.Event.Name, result[0].Name);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEventWithRelations()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var testEvent = CreateTestEvent(DateTime.Now, eventId);

        await _context.Events.AddAsync(testEvent);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetEventByIdAsync(eventId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventId, result.Id);
        Assert.NotNull(result.LocationId);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldThrowWhenNotFound()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _repository.GetEventByIdAsync(Guid.NewGuid())
        );

        Assert.Contains("не найдено", exception.Message);
    }

    [Fact]
    public async Task InsertEventAsync_ShouldAddNewEvent()
    {
        // Arrange
        var newEvent = new Event(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "New Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 5, 4.5);

        // Act
        await _repository.InsertEventAsync(newEvent);
        var result = await _context.Events.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newEvent.Name, result.Name);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateExistingEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var existingEvent = CreateTestEvent(DateTime.Now, eventId);

        await _context.Events.AddAsync(existingEvent);
        await _context.SaveChangesAsync();

        var updatedEvent = new Event(
            eventId,
            Guid.NewGuid(),
            "Updated Name",
            "Updated Desc",
            DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            20, 3, 10, 4.8);

        // Act
        await _repository.UpdateEventAsync(updatedEvent);
        var result = await _context.Events.FindAsync(eventId);

        // Assert
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal(20, result.PersonCount);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldRemoveEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var testEvent = CreateTestEvent(DateTime.Now, eventId);

        await _context.Events.AddAsync(testEvent);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteEventAsync(eventId);
        var result = await _context.Events.FindAsync(eventId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetEventByDayAsync_ShouldReturnCorrectEvent()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var eventDay = new EventDayDBModel(Guid.NewGuid(), dayId)
        {
            Event = CreateTestEvent(DateTime.Now)
        };

        await _context.EventsDays.AddAsync(eventDay);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetEventByDayAsync(dayId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventDay.Event.Id, result.Id);
    }

    private EventDBModel CreateTestEvent(DateTime date, Guid? id = null)
    {
        return new EventDBModel(
            id ?? Guid.NewGuid(),
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(date),
            10, 2, 5, 4.5)
        {
            Location = new LocationDBModel(Guid.NewGuid(), "Location", "Desc", 1000)
        };
    }
}