using Eventor.Common.Core;
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

public class MenuRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly MenuRepository _repository;
    private readonly Mock<DbSet<MenuDBModel>> _mockMenuDbSet;
    private readonly Mock<DbSet<DayDBModel>> _mockDaysDbSet;

    public MenuRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockMenuDbSet = new Mock<DbSet<MenuDBModel>>();
        _mockDaysDbSet = new Mock<DbSet<DayDBModel>>();

        _mockContext.Setup(c => c.Menu).Returns(_mockMenuDbSet.Object);
        _mockContext.Setup(c => c.Days).Returns(_mockDaysDbSet.Object);
        _repository = new MenuRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetAllMenuAsync_ReturnsAllMenusWithItems()
    {
        // Arrange
        var menuId = Guid.NewGuid();
        var menus = new List<MenuDBModel>
        {
            new(menuId, "Main Menu", 1000)
            {
                MenuItems = new List<MenuItemsDBModel>
                {
                    new(menuId, Guid.NewGuid(), 2)
                }
            }
        };

        var mockSet = SetupMockDbSet(menus.AsQueryable());
        _mockContext.Setup(c => c.Menu).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAllMenuAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Main Menu", result[0].Name);
    }

    [Fact]
    public async Task GetMenuByIdAsync_ReturnsCorrectMenu()
    {
        // Arrange
        var menuId = Guid.NewGuid();
        var testMenu = new MenuDBModel(menuId, "Test Menu", 1500);

        var mockSet = SetupMockDbSet(new List<MenuDBModel> { testMenu }.AsQueryable());
        _mockContext.Setup(c => c.Menu).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetMenuByIdAsync(menuId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(menuId, result.Id);
    }

    [Fact]
    public async Task GetMenuByDayAsync_ReturnsMenuForDay()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var testDay = new DayDBModel(dayId, menuId, "Day 1", 1, "Test Day", 500)
        {
            Menu = new MenuDBModel(menuId, "Day Menu", 800)
        };

        // Настраиваем DbSet без мокирования Include
        var mockSet = SetupMockDbSet(new List<DayDBModel> { testDay }.AsQueryable());
        _mockContext.Setup(c => c.Days).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetMenuByDayAsync(dayId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Day Menu", result.Name);
    }

    [Fact]
    public async Task InsertMenuAsync_AddsNewMenuToDatabase()
    {
        // Arrange
        var newMenu = new Menu(Guid.NewGuid(), "New Menu", 2000);
        MenuDBModel capturedMenu = null;

        _mockMenuDbSet.Setup(m => m.AddAsync(It.IsAny<MenuDBModel>(), default))
            .Callback<MenuDBModel, CancellationToken>((m, _) => capturedMenu = m)
            .ReturnsAsync(() => null);

        // Act
        await _repository.InsertMenuAsync(newMenu);

        // Assert
        Assert.NotNull(capturedMenu);
        Assert.Equal(newMenu.Name, capturedMenu.Name);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateMenuAsync_UpdatesExistingMenu()
    {
        // Arrange
        var menuId = Guid.NewGuid();
        var existingMenu = new MenuDBModel(menuId, "Old Name", 1000);
        var updatedMenu = new Menu(menuId, "New Name", 1500);

        var mockSet = SetupMockDbSet(new List<MenuDBModel> { existingMenu }.AsQueryable());
        _mockContext.Setup(c => c.Menu).Returns(mockSet.Object);

        // Act
        await _repository.UpdateMenuAsync(updatedMenu);

        // Assert
        Assert.Equal("New Name", existingMenu.Name);
        Assert.Equal(1500, existingMenu.Cost);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteMenuAsync_RemovesMenuFromDatabase()
    {
        // Arrange
        var menuId = Guid.NewGuid();
        var testMenu = new MenuDBModel(menuId, "Test Menu", 1000);

        // Настройка DbSet с поддержкой асинхронных операций
        var mockSet = SetupMockDbSet(new List<MenuDBModel> { testMenu }.AsQueryable());

        // Настройка FirstOrDefaultAsync для возврата testMenu
        mockSet.As<IAsyncEnumerable<MenuDBModel>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<MenuDBModel>(new List<MenuDBModel> { testMenu }.GetEnumerator()));

        _mockContext.Setup(c => c.Menu).Returns(mockSet.Object);

        // Act
        await _repository.DeleteMenuAsync(menuId);

        // Assert
        mockSet.Verify(m => m.Remove(It.Is<MenuDBModel>(m => m.Id == menuId)), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
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