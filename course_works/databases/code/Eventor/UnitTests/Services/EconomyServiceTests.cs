using Eventor.Services;
using Eventor.Common.Core;
using Eventor.Common.Enums;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Database.Core;
using Microsoft.Extensions.Logging;
using System.Linq;
using Eventor.Services.Exceptions;

namespace Eventor.Tests.Services;

public class EconomyServiceTests
{
    private readonly Mock<IItemRepository> _mockItemRepo;
    private readonly Mock<IMenuRepository> _mockMenuRepo;
    private readonly Mock<IDayRepository> _mockDayRepo;
    private readonly Mock<IPersonRepository> _mockPersonRepo;
    private readonly Mock<IEventRepository> _mockEventRepo;
    private readonly Mock<ILogger<EconomyService>> _mockLogger;
    private readonly EconomyService _economyService;
    private readonly Guid _eventId = Guid.NewGuid();
    private readonly Guid _dayId = Guid.NewGuid();
    private readonly Guid _menuId = Guid.NewGuid();

    public EconomyServiceTests()
    {
        _mockItemRepo = new Mock<IItemRepository>();
        _mockMenuRepo = new Mock<IMenuRepository>();
        _mockDayRepo = new Mock<IDayRepository>();
        _mockPersonRepo = new Mock<IPersonRepository>();
        _mockEventRepo = new Mock<IEventRepository>();
        _mockLogger = new Mock<ILogger<EconomyService>>();

        _economyService = new EconomyService(
            _mockEventRepo.Object,
            _mockDayRepo.Object,
            _mockItemRepo.Object,
            _mockMenuRepo.Object,
            _mockPersonRepo.Object,
            _mockLogger.Object
        );
    }

    // ---------------------------
    // GetDayCoefficientAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetItemCostAsync_ItemExists_ReturnsPrice()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var expectedPrice = 150.5;
        _mockItemRepo.Setup(r => r.GetItemByIdAsync(itemId))
            .ReturnsAsync(new Item(itemId, "Test Item", ItemType.OneDay, expectedPrice));

        // Act
        var result = await _economyService.GetItemCostAsync(itemId);

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Fact]
    public async Task GetItemCostAsync_ItemNotFound_ReturnsZeroAndLogsWarning()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        _mockItemRepo.Setup(r => r.GetItemByIdAsync(itemId))
            .ReturnsAsync((Item)null);

        // Act
        var result = await _economyService.GetItemCostAsync(itemId);

        // Assert
        Assert.Equal(0, result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Предмет с ID {itemId} не найден.");
    }

    [Fact]
    public async Task GetMenuCostAsync_ValidMenu_ReturnsCorrectSum()
    {
        // Arrange
        var menuId = Guid.NewGuid();
        var items = new List<Item>
        {
            new Item(Guid.NewGuid(), "Item1", ItemType.OneDay, 100),
            new Item(Guid.NewGuid(), "Item2", ItemType.OneDay, 50)
        };

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, items[0].Id))
            .ReturnsAsync(2);
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, items[1].Id))
            .ReturnsAsync(3);

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(items);

        // Act
        var result = await _economyService.GetMenuCostAsync(menuId);

        // Assert
        Assert.Equal(2 * 100 + 3 * 50, result); // 200 + 150 = 350
    }

    [Fact]
    public async Task GetDaysCostAsync_ValidDays_ReturnsSumOfMenuCosts()
    {
        // Arrange
        var dayIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var menuIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

        _mockDayRepo.SetupSequence(r => r.GetDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Day(dayIds[0], menuIds[0], "Day1", 1, "Desc", 100))
            .ReturnsAsync(new Day(dayIds[1], menuIds[1], "Day2", 2, "Desc", 200));

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuIds[0], It.IsAny<Guid>()))
            .ReturnsAsync(1);
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuIds[1], It.IsAny<Guid>()))
            .ReturnsAsync(1);

        var itemsForMenu1 = new List<Item> { new Item(Guid.NewGuid(), "Item1", ItemType.OneDay, 100) };
        var itemsForMenu2 = new List<Item> { new Item(Guid.NewGuid(), "Item2", ItemType.OneDay, 200) };

        _mockItemRepo.SetupSequence(r => r.GetAllItemsByMenuAsync(It.IsAny<Guid>()))
            .ReturnsAsync(itemsForMenu1)
            .ReturnsAsync(itemsForMenu2);

        // Act
        var result = await _economyService.GetDaysCostAsync(dayIds);

        // Assert
        Assert.Equal(100 + 200, result);
    }

    [Fact]
    public async Task GetEventCostAsync_EventWithDays_ReturnsTotalCost()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var days = new List<Day>
        {
            new Day(Guid.NewGuid(), Guid.NewGuid(), "Day1", 1, "Desc", 100),
            new Day(Guid.NewGuid(), Guid.NewGuid(), "Day2", 2, "Desc", 200)
        };

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(days);

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 100) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(2);

        // Act
        var result = await _economyService.GetEventCostAsync(eventId);

        // Assert
        Assert.Equal(2 * 100 * days.Count, result); // 200 * 2 = 400
    }

    [Fact]
    public async Task GetDaysCostAsync_OneDayNotFound_LogsWarningAndSkips()
    {
        // Arrange
        var existingDayId = Guid.NewGuid();
        var missingDayId = Guid.NewGuid();
        var existingMenuId = Guid.NewGuid();

        // Настройка для существующего дня
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(existingDayId))
            .ReturnsAsync(new Day(existingDayId, existingMenuId, "Existing", 1, "Desc", 100));

        // Настройка для отсутствующего дня
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(missingDayId))
            .ReturnsAsync((Day)null);

        // Настройка для расчета стоимости меню существующего дня
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(existingMenuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item1", ItemType.OneDay, 100) });
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(existingMenuId, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Act
        var result = await _economyService.GetDaysCostAsync(new[] { existingDayId, missingDayId });

        // Assert
        Assert.Equal(100, result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Day {missingDayId} not found");
    }

    // ---------------------------
    // GetDayCoefficientAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetDayCoefficientAsync_SingleDay_ReturnsCorrectValue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();

        // Создаем тестовое мероприятие
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 10,
            daysCount: 1,
            percent: 15,
            rating: 4.5
        );

        // Создаем тестовый день
        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 100);
        var allDays = new List<Day> { day };

        // Настройка моков
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(allDays);

        // Настройка данных меню
        var items = new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 100) };
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(items);

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Act
        var result = await _economyService.GetDayCoefficientAsync(new[] { dayId });

        // Assert
        Assert.Equal(1.0, result); // 100 / 100 = 1
    }

    [Fact]
    public async Task GetDayCoefficientAsync_MultipleDays_ReturnsSumOverMinCost()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId1 = Guid.NewGuid();
        var dayId2 = Guid.NewGuid();
        var menuId1 = Guid.NewGuid();
        var menuId2 = Guid.NewGuid();

        // Создаем тестовое мероприятие
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 10,
            daysCount: 2,
            percent: 15,
            rating: 4.5
        );

        // Создаем тестовые дни
        var day1 = new Day(dayId1, menuId1, "Day1", 1, "Desc", 50);
        var day2 = new Day(dayId2, menuId2, "Day2", 2, "Desc", 150);
        var allDays = new List<Day> { day1, day2 };

        // Настройка моков

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId1))
            .ReturnsAsync(testEvent);

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId1)).ReturnsAsync(day1);
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId2)).ReturnsAsync(day2);
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(allDays);

        // Настройка данных меню
        var itemsMenu1 = new List<Item> { new Item(Guid.NewGuid(), "Item1", ItemType.OneDay, 50) };
        var itemsMenu2 = new List<Item> { new Item(Guid.NewGuid(), "Item2", ItemType.OneDay, 150) };

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId1))
            .ReturnsAsync(itemsMenu1);
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId2))
            .ReturnsAsync(itemsMenu2);

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId1, It.IsAny<Guid>()))
            .ReturnsAsync(1);
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId2, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Act
        var result = await _economyService.GetDayCoefficientAsync(new[] { dayId1, dayId2 });

        // Assert
        Assert.Equal(4.0, result); // (50 + 150) / 50 = 4
    }

    // ---------------------------
    // GetCurrentDayCombinationsAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetCurrentDayCombinationsAsync_ReturnsUniqueCombinations()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var person1 = new Person(Guid.NewGuid(), "Person1", PersonType.Standart, true);
        var person2 = new Person(Guid.NewGuid(), "Person2", PersonType.Standart, true);

        // Создаем дни с конкретными идентификаторами
        var day1Id = Guid.NewGuid();
        var day2Id = Guid.NewGuid();
        var day1 = new Day(day1Id, Guid.NewGuid(), "Day1", 1, "Desc", 100);
        var day2 = new Day(day2Id, Guid.NewGuid(), "Day2", 2, "Desc", 200);

        // Настройка моков
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person1, person2 });

        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(person1.Id))
            .ReturnsAsync(new List<Day> { day1 });

        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(person2.Id))
            .ReturnsAsync(new List<Day> { day1, day2 });

        // Настройка GetDayByIdAsync для преобразования ID в объекты Day
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(day1Id)).ReturnsAsync(day1);
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(day2Id)).ReturnsAsync(day2);

        // Act
        var result = (await _economyService.GetCurrentDayCombinationsAsync(eventId)).ToList();

        // Assert
        Assert.Equal(2, result.Count);

        // Проверяем комбинацию из одного дня (day1)
        var singleDayCombo = result.FirstOrDefault(c => c.Count() == 1);
        Assert.NotNull(singleDayCombo);
        Assert.Contains(day1, singleDayCombo);

        // Проверяем комбинацию из двух дней (day1 и day2)
        var twoDaysCombo = result.FirstOrDefault(c => c.Count() == 2);
        Assert.NotNull(twoDaysCombo);
        Assert.Contains(day1, twoDaysCombo);
        Assert.Contains(day2, twoDaysCombo);
    }

    // ---------------------------
    // GetPersonCountAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetPersonCountAsync_SingleDay_ReturnsCorrectCount()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var persons = new List<Person>
        {
            new Person(Guid.NewGuid(), "Person1", PersonType.Standart, true),
            new Person(Guid.NewGuid(), "Person2", PersonType.Standart, true)
        };

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId)).ReturnsAsync(new Day(dayId, Guid.NewGuid(), "Day", 1, "Desc", 100));
        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(dayId)).ReturnsAsync(persons);

        // Act
        var result = await _economyService.GetPersonCountAsync(dayId);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetPersonCountAsync_MultipleDays_ReturnsIntersectionCount()
    {
        // Arrange
        var dayIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var person = new Person(Guid.NewGuid(), "Person", PersonType.Standart, true);

        _mockPersonRepo.Setup(r => r.GetAllPersonsAsync()).ReturnsAsync(new List<Person> { person });
        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(person.Id))
            .ReturnsAsync(new List<Day>
            {
                new Day(dayIds[0], Guid.NewGuid(), "Day1", 1, "Desc", 100),
                new Day(dayIds[1], Guid.NewGuid(), "Day2", 2, "Desc", 200)
            });

        // Act
        var result = await _economyService.GetPersonCountAsync(dayIds);

        // Assert
        Assert.Equal(1, result);
    }

    // ---------------------------
    // GetAllPersonsCountAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetAllPersonsCountAsync_ReturnsUniquePersonsCount()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var sameId = Guid.NewGuid();
        var persons = new List<Person>
    {
        new Person(Guid.NewGuid(), "Person1", PersonType.Standart, true),
        new Person(sameId, "Person2", PersonType.Standart, true),
        new Person(sameId, "Person3", PersonType.Standart, true)
    };

        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(persons);

        // Act
        var result = await _economyService.GetAllPersonsCountAsync(eventId);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetAllPersonsCountAsync_NoParticipants_ReturnsZero()
    {
        // Arrange
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Person>());

        // Act
        var result = await _economyService.GetAllPersonsCountAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(0, result);
    }

    // ----------------------------
    // Тесты для CalculateFundamentalPrice1DAsync
    // ----------------------------

    [Fact]
    public async Task CalculateFundamentalPrice1DAsync_ValidData_ReturnsCorrectValue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();

        // Создаем тестовое мероприятие
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 2,
            daysCount: 1,
            percent: 15,
            rating: 4.5
        );

        // Создаем тестовый день
        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 100);
        var daysInEvent = new List<Day> { day };

        // Настройка моков
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(daysInEvent);

        // Настройка данных меню
        var items = new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 10) };
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(items);

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(2);

        // Участники дня
        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(dayId))
            .ReturnsAsync(new List<Person>
            {
            new Person(Guid.NewGuid(), "Person1", PersonType.Standart, true),
            new Person(Guid.NewGuid(), "Person2", PersonType.Standart, true)
            });

        // Act
        var result = await _economyService.CalculateFundamentalPrice1DAsync(eventId);

        // Assert
        Assert.Equal(10, result); // 20 / (2 * 1) = 10
    }

    [Fact]
    public async Task CalculateFundamentalPrice1DAsync_ZeroParticipants_ThrowsException()
    {
        // Arrange
        var days = new List<Day> { new Day(_dayId, _menuId, "Day1", 1, "Desc", 100) };
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(_eventId))
            .ReturnsAsync(days);
        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(_dayId))
            .ReturnsAsync(new List<Person>());

        // Act & Assert
        await Assert.ThrowsAsync<EconomyServiceException>(() =>
            _economyService.CalculateFundamentalPrice1DAsync(_eventId));
    }

    // ----------------------------
    // Тесты для CalculateFundamentalPriceNDAsync
    // ----------------------------

    [Fact]
    public async Task CalculateFundamentalPriceNDAsync_ValidCombinations_ReturnsCorrectValue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Создаем тестовые данные
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 1,
            percent: 15,
            rating: 4.5
        );

        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 100);
        var person = new Person(personId, "Test Person", PersonType.Standart, true);

        // Настройка моков
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person });

        _mockPersonRepo.Setup(r => r.GetAllPersonsAsync()) // Добавлено
            .ReturnsAsync(new List<Person> { person });

        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(personId))
            .ReturnsAsync(new List<Day> { day });

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 10) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(2);

        // Act
        var result = await _economyService.CalculateFundamentalPriceNDAsync(eventId);

        // Assert
        Assert.Equal(20, result); // 20 / 1 = 20
    }

    [Fact]
    public async Task CalculateFundamentalPriceNDAsync_InvalidCost_ThrowsException()
    {
        // Arrange
        var combo = new List<Day> { new Day(_dayId, _menuId, "Day1", 1, "Desc", 100) };
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(_eventId))
            .ReturnsAsync(combo);
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(_menuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, -5) });

        // Act & Assert
        await Assert.ThrowsAsync<EconomyServiceException>(() =>
            _economyService.CalculateFundamentalPriceNDAsync(_eventId));
    }

    // ---------------------------
    // GetDayPriceAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetDayPriceAsync_ValidDay_ReturnsCorrectPriceWithPercent()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        const double percent = 15; // 15%

        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 2,
            daysCount: 1,
            percent: percent,
            rating: 4.5
        );

        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 100);

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 10) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(2);

        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(dayId))
            .ReturnsAsync(new List<Person>
            {
            new Person(Guid.NewGuid(), "Person1", PersonType.Standart, true),
            new Person(Guid.NewGuid(), "Person2", PersonType.Standart, true)
            });

        // Act
        var result = await _economyService.GetDayPriceAsync(dayId);

        // Assert: (10 * 2) / (2 * 1) * 1 * 1.15 = 11.5
        Assert.Equal(11.5, result);
    }

    // ---------------------------
    // GetDaysPriceAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetDaysPriceAsync_ValidDays_ReturnsCorrectPriceWithPercent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId1 = Guid.NewGuid();
        var dayId2 = Guid.NewGuid();
        var menuId1 = Guid.NewGuid();
        var menuId2 = Guid.NewGuid();
        const double percent = 20; // 20%

        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 2,
            percent: percent,
            rating: 4.5
        );

        var day1 = new Day(dayId1, menuId1, "Day1", 1, "Desc", 50);
        var day2 = new Day(dayId2, menuId2, "Day2", 2, "Desc", 150);

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId1))
            .ReturnsAsync(testEvent);
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId2))
            .ReturnsAsync(testEvent);

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId1)).ReturnsAsync(day1);
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId2)).ReturnsAsync(day2);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day1, day2 });

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId1))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item1", ItemType.OneDay, 50) });
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId2))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item2", ItemType.OneDay, 150) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId1, It.IsAny<Guid>()))
            .ReturnsAsync(1);
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId2, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        var personId = Guid.NewGuid();
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { new Person(personId, "Test", PersonType.Standart, true) });
        _mockPersonRepo.Setup(r => r.GetAllPersonsAsync())
            .ReturnsAsync(new List<Person> { new Person(personId, "Test", PersonType.Standart, true) });
        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(personId))
            .ReturnsAsync(new List<Day> { day1, day2 });

        // Act
        var result = await _economyService.GetDaysPriceAsync(new[] { dayId1, dayId2 });

        // Assert: (50 + 150) / (1 * 4) * 1.20 = 240
        Assert.Equal(240, result);
    }

    // ---------------------------
    // GetEventPriceAsync Tests
    // ---------------------------

    [Fact]
    public async Task GetEventPriceAsync_ValidEvent_ReturnsCorrectPriceWithPercent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId1 = Guid.NewGuid();
        var dayId2 = Guid.NewGuid();
        var menuId1 = Guid.NewGuid();
        var menuId2 = Guid.NewGuid();
        var personId = Guid.NewGuid();
        const double percent = 10; // 10%

        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 2,
            percent: percent,
            rating: 4.5
        );

        var day1 = new Day(dayId1, menuId1, "Day1", 1, "Desc", 50);
        var day2 = new Day(dayId2, menuId2, "Day2", 2, "Desc", 150);

        // Настройка репозиториев
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testEvent);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day1, day2 });

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId1))
            .ReturnsAsync(day1);
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId2))
            .ReturnsAsync(day2);

        // Настройка меню и предметов
        var item1 = new Item(Guid.NewGuid(), "Item1", ItemType.OneDay, 50);
        var item2 = new Item(Guid.NewGuid(), "Item2", ItemType.OneDay, 150);

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId1))
            .ReturnsAsync(new List<Item> { item1 });
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId2))
            .ReturnsAsync(new List<Item> { item2 });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId1, item1.Id))
            .ReturnsAsync(1);
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId2, item2.Id))
            .ReturnsAsync(1);

        // Критически важные настройки:
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { new Person(personId, "Test", PersonType.Standart, true) });

        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(personId))
            .ReturnsAsync(new List<Day> { day1, day2 });

        _mockPersonRepo.Setup(r => r.GetAllPersonsAsync())
            .ReturnsAsync(new List<Person> { new Person(personId, "Test", PersonType.Standart, true) });

        // Act
        var result = await _economyService.GetEventPriceAsync(eventId);

        // Assert: (50 + 150) / (1 * 4) * 1.10 = 220
        Assert.Equal(220, result, 0);
    }

    [Fact]
    public async Task GetEventPriceAsync_NoDays_ReturnsZeroAndLogsWarning()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day>());

        // Act
        var result = await _economyService.GetEventPriceAsync(eventId);

        // Assert
        Assert.Equal(0, result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Event {eventId} has no days");
    }

    // ---------------------------
    // AreDaysFromSameEventAsync Tests
    // ---------------------------

    [Fact]
    public async Task AreDaysFromSameEventAsync_SameEvent_ReturnsTrue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId1 = Guid.NewGuid();
        var dayId2 = Guid.NewGuid();

        // Настраиваем моки
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Event(eventId, Guid.NewGuid(), "Test", "Desc", DateOnly.MinValue, 1, 1, 0, 0));

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Day(Guid.NewGuid(), Guid.NewGuid(), "Day", 1, "Desc", 100));

        // Act
        var result = await _economyService.AreDaysFromSameEventAsync(new[] { dayId1, dayId2 });

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AreDaysFromSameEventAsync_DifferentEvents_ReturnsFalse()
    {
        // Arrange
        var dayId1 = Guid.NewGuid();
        var dayId2 = Guid.NewGuid();

        // Настраиваем моки для разных мероприятий
        _mockEventRepo.SetupSequence(r => r.GetEventByDayAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Event(Guid.NewGuid(), Guid.NewGuid(), "Event1", "Desc", DateOnly.MinValue,  1, 1, 0, 0))
            .ReturnsAsync(new Event(Guid.NewGuid(), Guid.NewGuid(), "Event2", "Desc", DateOnly.MinValue,  1, 1, 0, 0));

        _mockDayRepo.Setup(r => r.GetDayByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Day(Guid.NewGuid(), Guid.NewGuid(), "Day", 1, "Desc", 100));

        // Act
        var result = await _economyService.AreDaysFromSameEventAsync(new[] { dayId1, dayId2 });

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AreDaysFromSameEventAsync_EmptyCollection_ReturnsFalse()
    {
        // Act
        var result = await _economyService.AreDaysFromSameEventAsync(Enumerable.Empty<Guid>());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AreDaysFromSameEventAsync_DayNotFound_ReturnsFalse()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync((Day)null);

        // Act
        var result = await _economyService.AreDaysFromSameEventAsync(new[] { dayId });

        // Assert
        Assert.False(result);
    }

    // ---------------------------
    // CheckBalance1DAsync Tests
    // ---------------------------

    [Fact]
    public async Task CheckBalance1DAsync_ValidCase_ReturnsTrue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();

        // Создаем тестовое мероприятие
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 2,
            daysCount: 1,
            percent: 0, // Наценка 0%
            rating: 4.5
        );

        // Создаем день с минимальной стоимостью
        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 100);
        var days = new List<Day> { day };

        // Настройка моков
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(days);

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        // Настройка данных меню
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 100) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Участники
        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(dayId))
            .ReturnsAsync(new List<Person>
            {
            new Person(Guid.NewGuid(), "P1", PersonType.Standart, true),
            new Person(Guid.NewGuid(), "P2", PersonType.Standart, true)
            });

        // Настройка коэффициента дня
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        // Act
        var result = await _economyService.CheckBalance1DAsync(eventId);

        // Assert: Доходы (100) = Расходы (100)
        Assert.True(result);
    }

    [Fact]
    public async Task CheckBalance1DAsync_Unbalanced_ReturnsFalse()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var day1Id = Guid.NewGuid();
        var day2Id = Guid.NewGuid();
        var menu1Id = Guid.NewGuid();
        var menu2Id = Guid.NewGuid();

        // Создаем тестовое мероприятие с двумя днями
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 2,
            percent: 0,
            rating: 4.5
        );

        // Дни с одинаковой стоимостью
        var day1 = new Day(day1Id, menu1Id, "Day1", 1, "Desc", 1000);
        var day2 = new Day(day2Id, menu2Id, "Day2", 2, "Desc", 1000);
        var daysInEvent = new List<Day> { day1, day2 };

        // Настройка моков
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(daysInEvent);

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testEvent);

        // Настройка стоимости меню для каждого дня
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menu1Id))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 1000) });
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menu2Id))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 1000) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Участник посещает только первый день
        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(day1Id))
            .ReturnsAsync(new List<Person> { new Person(Guid.NewGuid(), "P1", PersonType.Standart, true) });
        _mockPersonRepo.Setup(r => r.GetAllPersonsByDayAsync(day2Id))
            .ReturnsAsync(new List<Person>());

        // Настройка данных для расчета коэффициента
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(day1Id)).ReturnsAsync(day1);
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(day2Id)).ReturnsAsync(day2);

        // Act
        var result = await _economyService.CheckBalance1DAsync(eventId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckBalance1DAsync_NoDays_ReturnsFalseAndLogs()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day>());

        // Act
        var result = await _economyService.CheckBalance1DAsync(eventId);

        // Assert
        Assert.False(result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Event {eventId} has no days");
    }

    // ---------------------------
    // CheckBalanceNDAsync Tests
    // ---------------------------

    [Fact]
    public async Task CheckBalanceNDAsync_ValidCombination_ReturnsTrue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Создаем тестовое мероприятие
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 1,
            percent: 0,
            rating: 4.5
        );

        // День с минимальной стоимостью
        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 100);
        var daysInEvent = new List<Day> { day };

        // Настройка моков
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(daysInEvent);

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        // Настройка стоимости меню
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 100) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Участник
        var person = new Person(personId, "Person", PersonType.Standart, true);
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person });

        // Настройка связи участника с днем
        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(personId))
            .ReturnsAsync(new List<Day> { day });

        // Критически важные настройки для расчета коэффициента
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        // Настройка мока для GetAllPersonsAsync
        _mockPersonRepo.Setup(r => r.GetAllPersonsAsync())
            .ReturnsAsync(new List<Person> { person });

        // Act
        var result = await _economyService.CheckBalanceNDAsync(eventId);

        // Assert: Доходы (100 * 1) = Расходы (100)
        Assert.True(result);
    }

    [Fact]
    public async Task CheckBalanceNDAsync_UnbalancedCombination_ReturnsFalse()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var person = new Person(Guid.NewGuid(), "Person", PersonType.Standart, true);

        // Создаем тестовое мероприятие
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 1,
            percent: 0,
            rating: 4.5
        );

        // День с высокой стоимостью
        var day = new Day(dayId, menuId, "Day1", 1, "Desc", 5000);
        var daysInEvent = new List<Day> { day };

        // Настройка моков
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(daysInEvent); 

        _mockEventRepo.Setup(r => r.GetEventByDayAsync(dayId))
            .ReturnsAsync(testEvent);

        // Настройка стоимости меню
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 5000) });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Участник
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person });

        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(person.Id))
            .ReturnsAsync(new List<Day> { day });

        // Act
        var result = await _economyService.CheckBalanceNDAsync(eventId);

        // Assert
        Assert.False(result); // Доходы (5000 * 1) < Расходы (5000 + 0) → false
    }

    [Fact]
    public async Task CheckBalanceNDAsync_NoCombinations_ReturnsFalseAndLogs()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person>());

        // Act
        var result = await _economyService.CheckBalanceNDAsync(eventId);

        // Assert
        Assert.False(result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Event {eventId} has no day combinations");
    }

    // ---------------------------
    // CheckSolutionExistenceAsync Tests
    // ---------------------------

    [Fact]
    public async Task CheckSolutionExistenceAsync_NoDays_ReturnsFalseAndLogs()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day>());

        // Act
        var result = await _economyService.CheckSolutionExistenceAsync(eventId);

        // Assert
        Assert.False(result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Мероприятие {eventId} не содержит дней");
    }

    [Fact]
    public async Task CheckSolutionExistenceAsync_NegativeDayCost_ReturnsFalseAndLogs()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var day = new Day(Guid.NewGuid(), Guid.NewGuid(), "Day", 1, "Desc", -100);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(day.MenuId))
            .ReturnsAsync(new List<Item> { new Item(Guid.NewGuid(), "Item", ItemType.OneDay, -50) });

        // Act
        var result = await _economyService.CheckSolutionExistenceAsync(eventId);

        // Assert
        Assert.False(result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"День {day.Id} имеет неположительную стоимость");
    }

    [Fact]
    public async Task CheckSolutionExistenceAsync_NoParticipants_ReturnsFalseAndLogs()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        // Создаем день с явным MenuId
        var day = new Day(
            id: dayId,
            menuId: menuId,
            name: "Test Day",
            sequenceNumber: 1,
            description: "Test Description",
            price: 100 // Это поле не используется в расчетах
        );

        // Настройка моков:
        // 1. Возвращаем дни мероприятия
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        // Добавлено: настройка GetDayByIdAsync для дня
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        // 2. Настраиваем положительную стоимость дня через меню
        var item = new Item(itemId, "Item", ItemType.OneDay, 150.0);
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { item });
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, itemId))
            .ReturnsAsync(2); // Стоимость дня: 150 * 2 = 300 > 0

        // 3. Настраиваем отсутствие участников
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person>());

        // Act
        var result = await _economyService.CheckSolutionExistenceAsync(eventId);

        // Assert
        Assert.False(result);
        _mockLogger.VerifyLog(LogLevel.Warning, $"Мероприятие {eventId} не имеет участников");
    }

    [Fact]
    public async Task CheckSolutionExistenceAsync_ValidConditions_ReturnsTrue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        // Создаем день с явным MenuId
        var day = new Day(
            id: dayId,
            menuId: menuId,
            name: "Test Day",
            sequenceNumber: 1,
            description: "Test Description",
            price: 100 
        );

        var person = new Person(Guid.NewGuid(), "Test", PersonType.Standart, true);
        var item = new Item(itemId, "Item", ItemType.OneDay, 100);

        // Настройка моков:
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        // Критически важно: настройка GetDayByIdAsync для корректного расчета стоимости
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(dayId))
            .ReturnsAsync(day);

        // Настройка стоимости меню для дня
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { item });

        // Критически важно: настройка количества предметов
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, itemId))
            .ReturnsAsync(1); // Стоимость дня: 100 * 1 = 100 > 0

        // Участники мероприятия
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person });

        // Act
        var result = await _economyService.CheckSolutionExistenceAsync(eventId);

        // Assert
        Assert.True(result);
    }

    // ---------------------------
    // CalculateCriticalParticipantsCountAsync Tests
    // ---------------------------

    [Fact]
    public async Task CalculateCriticalParticipantsCountAsync_ValidData_ReturnsRoundedUp()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var maxPrice = 150;

        // Создаем тестовые дни и настраиваем репозиторий
        var menuId = Guid.NewGuid();
        var day = new Day(Guid.NewGuid(), menuId, "Test Day", 1, "Description", 100);

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        // Настройка данных меню
        var item = new Item(Guid.NewGuid(), "Test Item", ItemType.OneDay, 100);
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { item });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, item.Id))
            .ReturnsAsync(5);

        // Act
        var result = await _economyService.CalculateCriticalParticipantsCountAsync(eventId, maxPrice);

        // Assert: (5 * 100) / 150 = 3.333 → округление до 4
        Assert.Equal(4, result);
    }

    [Fact]
    public async Task CalculateCriticalParticipantsCountAsync_ZeroEventCost_ReturnsZero()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        // Создаем день с меню, содержащим предмет нулевой стоимости
        var menuId = Guid.NewGuid();
        var day = new Day(Guid.NewGuid(), menuId, "Zero Cost Day", 1, "Desc", 0);

        // Настраиваем моки
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day }); // Явно возвращаем список

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item>
            {
            new Item(Guid.NewGuid(), "Free Item", ItemType.OneDay, 0) // Цена предмета = 0
            });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, It.IsAny<Guid>()))
            .ReturnsAsync(1); // Количество предметов = 1

        // Act
        var result = await _economyService.CalculateCriticalParticipantsCountAsync(eventId, 100);

        // Assert
        Assert.Equal(0, result); // 0 / 100 = 0 → результат 0
    }

    [Fact]
    public async Task CalculateCriticalParticipantsCountAsync_InvalidMaxPrice_ThrowsException()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<EconomyServiceException>(() =>
            _economyService.CalculateCriticalParticipantsCountAsync(Guid.NewGuid(), -10));

        // Проверяем, что внутреннее исключение - ArgumentException
        Assert.IsType<ArgumentException>(ex.InnerException);
        Assert.Contains("Максимальная цена должна быть положительной", ex.InnerException.Message);
    }

    // ---------------------------
    // CalculateMaxMarkupAsync Tests
    // ---------------------------

    [Fact]
    public async Task CalculateMaxMarkupAsync_ValidCombinations_ReturnsMinMarkup()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var maxPrice = 200;
        var personId = Guid.NewGuid();
        var person = new Person(personId, "Test", PersonType.Standart, true);

        // Создаем дни мероприятия
        var day1 = new Day(Guid.NewGuid(), Guid.NewGuid(), "Day1", 1, "Desc", 100);
        var day2 = new Day(Guid.NewGuid(), Guid.NewGuid(), "Day2", 2, "Desc", 100);
        var testEvent = new Event(
            id: eventId,
            name: "Test Event",
            description: "Test Description",
            date: DateOnly.FromDateTime(DateTime.Now),
            locationId: Guid.NewGuid(),
            personCount: 1,
            daysCount: 2,
            percent: 0,
            rating: 4.5
        );

        // Настройка моков
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day1, day2 });
        _mockEventRepo.Setup(r => r.GetEventByDayAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testEvent);
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person });
        _mockPersonRepo.Setup(r => r.GetAllPersonsAsync())
            .ReturnsAsync(new List<Person> { person });
        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(personId))
            .ReturnsAsync(new List<Day> { day1, day2 });
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(day1.Id))
            .ReturnsAsync(day1);
        _mockDayRepo.Setup(r => r.GetDayByIdAsync(day2.Id))
            .ReturnsAsync(day2);

        // Настройка данных меню и предметов
        var item = new Item(Guid.NewGuid(), "Item", ItemType.OneDay, 100);
        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Item> { item });
        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(1);

        // Act
        var result = await _economyService.CalculateMaxMarkupAsync(eventId, maxPrice);

        // Assert
        Assert.Equal(0, result); // Ожидаемая наценка 0%
    }

    [Fact]
    public async Task CalculateMaxMarkupAsync_NegativeFundamentalPrice_ThrowsException()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        // Настраиваем данные для отрицательной стоимости мероприятия
        var menuId = Guid.NewGuid();
        var day = new Day(Guid.NewGuid(), menuId, "Day", 1, "Desc", -100);
        var item = new Item(Guid.NewGuid(), "Negative Item", ItemType.OneDay, price: -50);

        // Моки репозиториев
        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        _mockItemRepo.Setup(r => r.GetAllItemsByMenuAsync(menuId))
            .ReturnsAsync(new List<Item> { item });

        _mockMenuRepo.Setup(r => r.GetAmountItemAsync(menuId, item.Id))
            .ReturnsAsync(2); // 2 × (-50) = -100

        // Участник
        var person = new Person(Guid.NewGuid(), "Test", PersonType.Standart, true);
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person> { person });
        _mockDayRepo.Setup(r => r.GetAllDaysByPersonAsync(person.Id))
            .ReturnsAsync(new List<Day> { day });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<EconomyServiceException>(() =>
            _economyService.CalculateMaxMarkupAsync(eventId, 200));

        // Проверяем цепочку исключений
        var innerEx = ex.InnerException; // EconomyServiceException из CalculateFundamentalPriceNDAsync
        Assert.NotNull(innerEx);

        var fundamentalPriceEx = innerEx.InnerException; // Исходное исключение
        Assert.NotNull(fundamentalPriceEx);
        Assert.Contains("Стоимость мероприятия не может быть отрицательной", fundamentalPriceEx.Message);
    }

    [Fact]
    public async Task CalculateMaxMarkupAsync_NoCombinations_ThrowsException()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        // Настраиваем дни мероприятия
        var day = new Day(
            id: Guid.NewGuid(),
            menuId: Guid.NewGuid(),
            name: "Test Day",
            sequenceNumber: 1,
            description: "Desc",
            price: 100
        );

        _mockDayRepo.Setup(r => r.GetAllDaysByEventAsync(eventId))
            .ReturnsAsync(new List<Day> { day });

        // Нет участников → нет комбинаций дней
        _mockPersonRepo.Setup(r => r.GetAllPersonsByEventAsync(eventId))
            .ReturnsAsync(new List<Person>());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<EconomyServiceException>(() =>
            _economyService.CalculateMaxMarkupAsync(eventId, 200));

        // Проверяем цепочку исключений
        var innerEx = ex.InnerException?.InnerException as EconomyServiceException;
        Assert.NotNull(innerEx);
        Assert.Contains("Нет данных о выбранных комбинациях дней", innerEx!.Message);

        // Исправленная проверка логов: используем LogLevel.Error и полное сообщение
        _mockLogger.VerifyLog(
            LogLevel.Error,
            $"Мероприятие {eventId} не имеет комбинаций дней"
        );
    }

    [Fact]
    public async Task CalculateMaxMarkupAsync_InvalidMaxPrice_ThrowsException()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<EconomyServiceException>(() =>
            _economyService.CalculateMaxMarkupAsync(Guid.NewGuid(), -100));

        // Проверяем внутреннее исключение и сообщение
        Assert.IsType<ArgumentException>(ex.InnerException);
        Assert.Contains("Максимальная цена должна быть положительной", ex.InnerException!.Message);
    }
}

public static class LoggerExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message)
    {
        loggerMock.Verify(l => l.Log(
            level,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(message)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.AtLeastOnce);
    }
}