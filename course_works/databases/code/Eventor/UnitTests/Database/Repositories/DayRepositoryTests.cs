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
/// Набор тестов для проверки функциональности репозитория дней мероприятий
/// </summary>
public class DayRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly DayRepository _repository;
    private readonly Mock<DbSet<DayDBModel>> _mockDaysDbSet;
    private readonly Mock<DbSet<EventDayDBModel>> _mockEventDaysDbSet;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DayRepositoryTests"/>
    /// </summary>
    public DayRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockDaysDbSet = new Mock<DbSet<DayDBModel>>();
        _mockEventDaysDbSet = new Mock<DbSet<EventDayDBModel>>();

        _mockContext.Setup(c => c.Days).Returns(_mockDaysDbSet.Object);
        _mockContext.Setup(c => c.EventsDays).Returns(_mockEventDaysDbSet.Object);

        _repository = new DayRepository(_mockContext.Object);
    }

    /// <summary>
    /// Проверяет, что метод GetAllDaysAsync возвращает все дни мероприятий
    /// </summary>
    [Fact]
    public async Task GetAllDaysAsync_ReturnsAllDays()
    {
        // Arrange
        var testData = new List<DayDBModel>
        {
            new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 1", 1, "Desc", 100),
            new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 2", 2, "Desc", 200)
        }.AsQueryable();

        // Настройка мока DbSet
        var mockSet = new Mock<DbSet<DayDBModel>>();
        mockSet.As<IAsyncEnumerable<DayDBModel>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<DayDBModel>(testData.GetEnumerator()));

        mockSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<DayDBModel>(testData.Provider));

        mockSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.Expression).Returns(testData.Expression);
        mockSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.ElementType).Returns(testData.ElementType);
        mockSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

        _mockContext.Setup(c => c.Days).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAllDaysAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Collection(result,
            d => Assert.Equal("Day 1", d.Name),
            d => Assert.Equal("Day 2", d.Name));
    }

    /// <summary>
    /// Проверяет, что метод GetAllDaysByEventAsync возвращает дни только для указанного мероприятия
    /// </summary>
    [Fact]
    public async Task GetAllDaysByEventAsync_ReturnsFilteredDays()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();

        // Заполняем тестовыми данными
        using (var context = new EventorDBContext(options))
        {
            context.EventsDays.AddRange(
                new EventDayDBModel(eventId, dayId)
                {
                    Day = new DayDBModel(dayId, Guid.NewGuid(), "Day 1", 1, "Event Day", 100)
                },
                new EventDayDBModel(Guid.NewGuid(), Guid.NewGuid())
                {
                    Day = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 2", 2, "Other Day", 200)
                }
            );
            await context.SaveChangesAsync();
        }

        // Создаем новый контекст для теста
        using (var context = new EventorDBContext(options))
        {
            var repository = new DayRepository(context);

            // Act
            var result = await repository.GetAllDaysByEventAsync(eventId);

            // Assert
            var day = Assert.Single(result);
            Assert.Equal(dayId, day.Id);
            Assert.Equal("Event Day", day.Description);
            Assert.Equal(100, day.Price);
        }
    }

    [Fact]
    public async Task GetAllDaysByPersonAsync_ReturnsCorrectDays()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var personId = Guid.NewGuid();
        var day1 = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 1", 1, "Desc", 100);
        var day2 = new DayDBModel(Guid.NewGuid(), Guid.NewGuid(), "Day 2", 2, "Desc", 200);

        using (var context = new EventorDBContext(options))
        {
            context.PersonsDays.AddRange(
                new PersonDayDBModel(personId, day1.Id) { Day = day1 },
                new PersonDayDBModel(personId, day2.Id) { Day = day2 },
                new PersonDayDBModel(Guid.NewGuid(), day1.Id) { Day = day1 }
            );
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new EventorDBContext(options))
        {
            var repository = new DayRepository(context);
            var result = await repository.GetAllDaysByPersonAsync(personId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Collection(result,
                d => Assert.Equal("Day 1", d.Name),
                d => Assert.Equal("Day 2", d.Name));
        }
    }

    [Fact]
    public async Task GetSelectedDaysAsync_ReturnsEmptyList_WhenNoDaysSelected()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var personId = Guid.NewGuid();

        // Используем реальный контекст с in-memory базой
        using (var context = new EventorDBContext(options))
        {
            var repository = new DayRepository(context);

            // Act
            var result = await repository.GetAllDaysByPersonAsync(personId);

            // Assert
            Assert.Empty(result);
        }
    }

    /// <summary>
    /// Проверяет, что метод GetDayByIdAsync возвращает корректный день по идентификатору
    /// </summary>
    [Fact]
    public async Task GetDayByIdAsync_ReturnsCorrectDay()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var testDay = new DayDBModel(dayId, menuId, "Test Day", 1, "Test Desc", 150);

        var testData = new List<DayDBModel> { testDay }.AsQueryable();

        // Настройка мока DbSet<DayDBModel>
        var mockDaysDbSet = new Mock<DbSet<DayDBModel>>();

        // Поддержка IAsyncEnumerable
        mockDaysDbSet.As<IAsyncEnumerable<DayDBModel>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<DayDBModel>(testData.GetEnumerator()));

        // Поддержка IQueryable
        mockDaysDbSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<DayDBModel>(testData.Provider));

        mockDaysDbSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.Expression).Returns(testData.Expression);

        mockDaysDbSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.ElementType).Returns(testData.ElementType);

        mockDaysDbSet.As<IQueryable<DayDBModel>>()
            .Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

        _mockContext.Setup(c => c.Days).Returns(mockDaysDbSet.Object);

        // Act
        var result = await _repository.GetDayByIdAsync(dayId);

        // Assert
        Assert.Equal(dayId, result.Id);
        Assert.Equal(menuId, result.MenuId);
        Assert.Equal("Test Desc", result.Description);
        Assert.Equal(150, result.Price);
    }

    /// <summary>
    /// Проверяет, что метод InsertDayAsync добавляет полные данные дня в контекст
    /// </summary>
    [Fact]
    public async Task InsertDayAsync_AddsFullDayDataToContext()
    {
        // Arrange
        var day = new Common.Core.Day(
            id: Guid.NewGuid(),
            menuId: Guid.NewGuid(),
            name: "New Day",
            sequenceNumber: 3,
            description: "Full Description",
            price: 300);

        DayDBModel capturedDay = null;

        _mockDaysDbSet.Setup(m => m.AddAsync(It.IsAny<DayDBModel>(), default))
            .Callback<DayDBModel, CancellationToken>((d, _) => capturedDay = d)
            .ReturnsAsync(() => null);

        // Act
        await _repository.InsertDayAsync(day);

        // Assert
        Assert.NotNull(capturedDay);
        Assert.Equal(day.Id, capturedDay.Id);
        Assert.Equal(day.MenuId, capturedDay.MenuId);
        Assert.Equal(day.SequenceNumber, capturedDay.SequenceNumber);
        Assert.Equal(day.Description, capturedDay.Description);
        Assert.Equal(day.Price, capturedDay.Price);

        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет, что метод UpdateDayAsync обновляет все поля дня
    /// </summary>
    [Fact]
    public async Task UpdateDayAsync_UpdatesAllFields()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();

        // Исходные данные
        var existingDay = new DayDBModel(dayId, menuId, "Old Name", 1, "Old Desc", 100);

        // Обновлённые данные
        var updatedDay = new Common.Core.Day(
            dayId,
            menuId,  // Используем тот же MenuId
            "New Name",
            2,
            "New Desc",
            200);

        // Настройка моков
        _mockDaysDbSet.Setup(m => m.FindAsync(dayId))
            .ReturnsAsync(existingDay);

        // Важно: Настраиваем вызов Update
        _mockDaysDbSet.Setup(m => m.Update(It.IsAny<DayDBModel>()))
            .Callback<DayDBModel>(entity =>
            {
                // Применяем изменения к существующей сущности
                existingDay.Name = entity.Name;
                existingDay.SequenceNumber = entity.SequenceNumber;
                existingDay.Description = entity.Description;
                existingDay.Price = entity.Price;
            });

        // Act
        await _repository.UpdateDayAsync(updatedDay);

        // Assert
        Assert.Equal("New Name", existingDay.Name);
        Assert.Equal(2, existingDay.SequenceNumber);
        Assert.Equal("New Desc", existingDay.Description);
        Assert.Equal(200, existingDay.Price);

        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет, что метод DeleteDayAsync выбрасывает исключение, когда день не найден
    /// </summary>
    [Fact]
    public async Task DeleteDayAsync_WhenDayNotFound_ThrowsArgumentException()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        _mockDaysDbSet.Setup(m => m.FindAsync(dayId)).ReturnsAsync((DayDBModel)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _repository.DeleteDayAsync(dayId)
        );

        Assert.Equal($"День с ID {dayId} не найден.", exception.Message);
    }
}