using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Eventor.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

/// <summary>
/// Набор тестов для проверки функциональности репозитория мероприятий
/// </summary>
public class EventRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly EventRepository _repository;
    private readonly Mock<DbSet<EventDBModel>> _mockEventsDbSet;
    private readonly Mock<DbSet<UserEventDBModel>> _mockUserEventsDbSet;
    private readonly Mock<DbSet<LocationDBModel>> _mockLocationsDbSet;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EventRepositoryTests"/>
    /// </summary>
    public EventRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockEventsDbSet = new Mock<DbSet<EventDBModel>>();
        _mockUserEventsDbSet = new Mock<DbSet<UserEventDBModel>>();
        _mockLocationsDbSet = new Mock<DbSet<LocationDBModel>>();

        _mockContext.Setup(c => c.Events).Returns(_mockEventsDbSet.Object);
        _mockContext.Setup(c => c.UsersEvents).Returns(_mockUserEventsDbSet.Object);
        _mockContext.Setup(c => c.Locations).Returns(_mockLocationsDbSet.Object);

        _repository = new EventRepository(_mockContext.Object);
    }

    /// <summary>
    /// Проверяет, что метод GetAllEventsAsync возвращает все мероприятия, отсортированные по дате
    /// </summary>
    [Fact]
    public async Task GetAllEventsAsync_ReturnsAllEventsOrderedByDate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var today = DateOnly.FromDateTime(DateTime.Now);
        var locationId = Guid.NewGuid();

        // Заполняем тестовыми данными
        using (var context = new EventorDBContext(options))
        {
            context.Locations.Add(new LocationDBModel(locationId, "Test Location", "Desc", 1000));

            context.Events.AddRange(
                new EventDBModel(
                    id: Guid.NewGuid(),
                    locationId: locationId,
                    name: "Event 2",
                    description: "Description 2",
                    date: today.AddDays(1),
                    personCount: 10,
                    daysCount: 2,
                    percent: 10,
                    rating: 4.5),
                new EventDBModel(
                    id: Guid.NewGuid(),
                    locationId: locationId,
                    name: "Event 1",
                    description: "Description 1",
                    date: today,
                    personCount: 20,
                    daysCount: 3,
                    percent: 15,
                    rating: 4.8)
            );
            await context.SaveChangesAsync();
        }

        // Создаем новый контекст для теста
        using (var context = new EventorDBContext(options))
        {
            var repository = new EventRepository(context);

            // Act
            var result = await repository.GetAllEventsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Event 1", result[0].Name); // Должен быть первым, так как дата раньше
            Assert.Equal("Event 2", result[1].Name);
        }
    }

    /// <summary>
    /// Проверяет, что метод GetAllEventsByUserAsync возвращает мероприятия пользователя с локациями
    /// </summary>
    [Fact]
    public async Task GetAllEventsByUserAsync_ReturnsUserEventsWithLocations()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var locationId = Guid.NewGuid();
        var dayId = Guid.NewGuid();

        // Заполняем тестовыми данными
        using (var context = new EventorDBContext(options))
        {
            context.Locations.Add(new LocationDBModel(locationId, "Test Location", "Location Desc", 1000));

            context.Events.Add(new EventDBModel(
                eventId,
                locationId,
                "Test Event",
                "Test Description",
                DateOnly.FromDateTime(DateTime.Now),
                10, 2, 5, 4.5)
            {
                EventDays = new List<EventDayDBModel>
            {
                new EventDayDBModel(eventId, dayId)
                {
                    Day = new DayDBModel(dayId, Guid.NewGuid(), "Day 1", 1, "Day Desc", 100)
                }
            }
            });

            context.UsersEvents.Add(new UserEventDBModel(userId, eventId));
            await context.SaveChangesAsync();
        }

        // Создаем новый контекст для теста
        using (var context = new EventorDBContext(options))
        {
            var repository = new EventRepository(context);

            // Act
            var result = await repository.GetAllEventsByUserAsync(userId);

            // Assert
            var eventItem = Assert.Single(result);
            Assert.Equal("Test Event", eventItem.Name);
            Assert.Equal(locationId, eventItem.LocationId);
        }
    }

    /// <summary>
    /// Проверяет, что метод GetEventByIdAsync возвращает мероприятие с локацией
    /// </summary>
    [Fact]
    public async Task GetEventByIdAsync_ReturnsEventWithLocation()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var testEvent = CreateTestEventDBModel(DateTime.Now);
        testEvent.Id = eventId;

        // Настраиваем DbSet с поддержкой Include и асинхронных операций
        var events = new List<EventDBModel> { testEvent };
        var mockSet = SetupMockDbSet(events.AsQueryable());

        _mockContext.Setup(c => c.Events).Returns(mockSet.Object);
        _mockContext.Setup(c => c.Locations).Returns(SetupMockDbSet(new List<LocationDBModel> { testEvent.Location }.AsQueryable()).Object);

        // Act
        var result = await _repository.GetEventByIdAsync(eventId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventId, result.Id);
        Assert.Equal(testEvent.Location.Id, result.LocationId); // Проверка связанной сущности
    }

    /// <summary>
    /// Проверяет, что метод UpdateEventAsync корректно обновляет мероприятие
    /// </summary>
    [Fact]
    public async Task UpdateEventAsync_UpdatesEventProperties()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var existingEvent = CreateTestEventDBModel(DateTime.Now);
        existingEvent.Id = eventId;

        var updatedEvent = new Event(
            id: eventId,
            locationId: Guid.NewGuid(),
            name: "Updated Event",
            description: "Updated Description",
            date: DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            personCount: 30,
            daysCount: 4,
            percent: 15,
            rating: 4.7);

        // Настраиваем DbSet с поддержкой IQueryable и асинхронных операций
        var mockSet = SetupMockDbSet(new List<EventDBModel> { existingEvent }.AsQueryable());
        _mockContext.Setup(c => c.Events).Returns(mockSet.Object);

        // Act
        await _repository.UpdateEventAsync(updatedEvent);

        // Assert
        Assert.Equal("Updated Event", existingEvent.Name);
        Assert.Equal("Updated Description", existingEvent.Description);
        Assert.Equal(30, existingEvent.PersonCount);
        Assert.Equal(4, existingEvent.DaysCount);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет, что метод DeleteEventAsync корректно удаляет мероприятие
    /// </summary>
    [Fact]
    public async Task DeleteEventAsync_RemovesEventFromContext()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var testEvent = CreateTestEventDBModel(DateTime.Now);
        testEvent.Id = eventId;

        // Настройка мока для поддержки FirstOrDefaultAsync
        var mockSet = new Mock<DbSet<EventDBModel>>();
        mockSet.As<IQueryable<EventDBModel>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<EventDBModel>(new List<EventDBModel> { testEvent }.AsQueryable().Provider));

        mockSet.As<IQueryable<EventDBModel>>()
            .Setup(m => m.Expression)
            .Returns(new List<EventDBModel> { testEvent }.AsQueryable().Expression);

        mockSet.As<IQueryable<EventDBModel>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => new List<EventDBModel> { testEvent }.GetEnumerator());

        mockSet.Setup(m => m.FindAsync(eventId))
            .ReturnsAsync(testEvent);

        _mockContext.Setup(c => c.Events).Returns(mockSet.Object);

        // Act
        await _repository.DeleteEventAsync(eventId);

        // Assert
        mockSet.Verify(m => m.Remove(testEvent), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет конвертацию из EventDBModel в Event
    /// </summary>
    [Fact]
    public void EventConverter_ConvertsDBModelToCoreCorrectly()
    {
        // Arrange
        var dbModel = CreateTestEventDBModel(DateTime.Now);

        // Act
        var result = EventConverter.ConvertDBToCore(dbModel);

        // Assert
        Assert.Equal(dbModel.Id, result.Id);
        Assert.Equal(dbModel.Name, result.Name);
    }

    private EventDBModel CreateTestEventDBModel(DateTime date)
    {
        var locationId = Guid.NewGuid();
        return new EventDBModel(
            id: Guid.NewGuid(),
            locationId: locationId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(date),
            personCount: 20,
            daysCount: 3,
            percent: 15,
            rating: 4.8)
            {
                Location = new LocationDBModel(locationId, "Test Location", "Test Location Description", 1000)
            };
    }

    private Mock<DbSet<T>> SetupMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(data.Provider));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Expression).Returns(data.Expression);

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.ElementType).Returns(data.ElementType);

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        return mockSet;
    }
}