using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

public class DayRepositoryTests : IDisposable
{
    private readonly EventorDBContext _context;
    private readonly ILogger<DayRepository> _logger;
    private readonly DayRepository _repository;
    private readonly DbContextOptions<EventorDBContext> _options;

    public DayRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventorDBContext(_options);
        _logger = Mock.Of<ILogger<DayRepository>>();
        _repository = new DayRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllDaysAsync_ShouldReturnAllDays()
    {
        // Arrange
        var days = new List<DayDBModel>
        {
            new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 1", 1, "Desc", 100),
            new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 2", 2, "Desc", 200)
        };

        await _context.Days.AddRangeAsync(days);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllDaysAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Name == "Day 1");
        Assert.Contains(result, d => d.Name == "Day 2");
    }

    [Fact]
    public async Task GetAllDaysByEventAsync_ShouldFilterByEventId()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayInEvent = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Event Day", 1, "Desc", 100);
        var dayNotInEvent = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Other Day", 2, "Desc", 200);

        await _context.EventsDays.AddRangeAsync(
            new EventDayDBModel(eventId, dayInEvent.Id) { Day = dayInEvent },
            new EventDayDBModel(Guid.NewGuid(), dayNotInEvent.Id) { Day = dayNotInEvent }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllDaysByEventAsync(eventId);

        // Assert
        var day = Assert.Single(result);
        Assert.Equal("Event Day", day.Name);
    }

    [Fact]
    public async Task GetAllDaysByPersonAsync_ShouldFilterByPersonId()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var day1 = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 1", 1, "Desc", 100);
        var day2 = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 2", 2, "Desc", 200);

        await _context.PersonsDays.AddRangeAsync(
            new PersonDayDBModel(personId, day1.Id) { Day = day1 },
            new PersonDayDBModel(personId, day2.Id) { Day = day2 },
            new PersonDayDBModel(Guid.NewGuid(), day1.Id) { Day = day1 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllDaysByPersonAsync(personId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Name == "Day 1");
        Assert.Contains(result, d => d.Name == "Day 2");
    }

    [Fact]
    public async Task GetDayByIdAsync_ShouldReturnCorrectDay()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var expectedDay = new DayDBModel(dayId, Guid.NewGuid(), "Test Day", 1, "Test Desc", 150);

        await _context.Days.AddAsync(expectedDay);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetDayByIdAsync(dayId);

        // Assert
        Assert.Equal(dayId, result.Id);
        Assert.Equal("Test Desc", result.Description);
    }

    [Fact]
    public async Task GetDayByIdAsync_ShouldThrowWhenNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _repository.GetDayByIdAsync(invalidId)
        );

        Assert.Contains(invalidId.ToString(), exception.Message);
    }

    [Fact]
    public async Task InsertPersonToDayAsync_ShouldAddPersonDayRelation()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();

        // Создаем связанные сущности для валидности внешних ключей
        await _context.Persons.AddAsync(new PersonDBModel(personId, "Test Person", PersonType.Standart, true));
        await _context.Days.AddAsync(new DayDBModel(dayId, Guid.NewGuid(), "Test Day", 1, "Desc", 100));
        await _context.SaveChangesAsync();

        // Act
        await _repository.InsertPersonToDayAsync(personId, dayId);

        // Assert
        var relation = await _context.PersonsDays
            .FirstOrDefaultAsync(pd => pd.PersonId == personId && pd.DayId == dayId);

        Assert.NotNull(relation);
        Assert.Equal(personId, relation.PersonId);
        Assert.Equal(dayId, relation.DayId);
    }

    [Fact]
    public async Task InsertPersonToDayAsync_ShouldHandleDuplicateEntries()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();

        // Создаем участника и день для валидности FK
        await _context.Persons.AddAsync(new PersonDBModel(personId, "Test Person", PersonType.Standart, true));
        await _context.Days.AddAsync(new DayDBModel(dayId, Guid.NewGuid(), "Test Day", 1, "Desc", 100));
        await _context.SaveChangesAsync();

        // Первое добавление связи
        await _repository.InsertPersonToDayAsync(personId, dayId);

        // Очищаем контекст для имитации нового запроса
        _context.ChangeTracker.Clear();

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            _repository.InsertPersonToDayAsync(personId, dayId));

        // Проверка для разных типов БД
        switch (exception)
        {
            case DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx:
                Assert.Equal("23505", pgEx.SqlState); // Код ошибки уникальности PostgreSQL
                break;
            case ArgumentException argEx:
                Assert.Contains("same key", argEx.Message, StringComparison.OrdinalIgnoreCase);
                break;
            default:
                Assert.Fail($"Unexpected exception type: {exception.GetType()}");
                break;
        }
    }

    [Fact]
    public async Task InsertDayAsync_ShouldAddNewDay()
    {
        // Arrange
        var newDay = new Day(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "New Day",
            3,
            "New Description",
            300);

        // Act
        await _repository.InsertDayAsync(newDay);
        var result = await _context.Days.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newDay.Name, result.Name);
        Assert.Equal(newDay.Price, result.Price);
    }

    [Fact]
    public async Task UpdateDayAsync_ShouldModifyExistingDay()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var originalDay = new DayDBModel(dayId, Guid.NewGuid(), "Old Name", 1, "Old Desc", 100);

        await _context.Days.AddAsync(originalDay);
        await _context.SaveChangesAsync();

        // Detach the entity to avoid tracking conflict
        _context.Entry(originalDay).State = EntityState.Detached;

        var updatedDay = new Day(
            dayId,
            originalDay.MenuId,
            "New Name",
            2,
            "New Desc",
            200);

        // Act
        await _repository.UpdateDayAsync(updatedDay);
        var result = await _context.Days.FindAsync(dayId);

        // Assert
        Assert.Equal("New Name", result.Name);
        Assert.Equal(200, result.Price);
    }

    [Fact]
    public async Task DeleteDayAsync_ShouldRemoveDay()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var day = new DayDBModel(dayId, Guid.NewGuid(), "Delete Day", 1, "Desc", 100);

        await _context.Days.AddAsync(day);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteDayAsync(dayId);
        var result = await _context.Days.FindAsync(dayId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteDayAsync_ShouldThrowWhenDayNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _repository.DeleteDayAsync(invalidId));
    }
}