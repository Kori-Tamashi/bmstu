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

public class MenuRepositoryTests : IDisposable
{
    private readonly EventorDBContext _context;
    private readonly ILogger<MenuRepository> _logger;
    private readonly MenuRepository _repository;
    private readonly DbContextOptions<EventorDBContext> _options;

    public MenuRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventorDBContext(_options);
        _logger = Mock.Of<ILogger<MenuRepository>>();
        _repository = new MenuRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private MenuDBModel CreateTestMenu(
        Guid? id = null,
        string name = "Test Menu",
        double cost = 1000)
    {
        return new MenuDBModel(
            id ?? Guid.NewGuid(),
            name,
            cost);
    }

    [Fact]
    public async Task GetAllMenuAsync_ShouldReturnAllMenus()
    {
        // Arrange
        var menus = new List<MenuDBModel>
        {
            CreateTestMenu(name: "Menu 1"),
            CreateTestMenu(name: "Menu 2")
        };

        await _context.Menu.AddRangeAsync(menus);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllMenuAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Name == "Menu 1");
    }

    [Fact]
    public async Task GetAllMenuAsync_ShouldLogErrorOnFailure()
    {
        // Arrange
        var corruptedContext = new Mock<EventorDBContext>(_options);
        corruptedContext.Setup(c => c.Menu).Throws<Exception>();
        var repository = new MenuRepository(corruptedContext.Object, _logger);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllMenuAsync());
        Mock.Get(_logger).VerifyLog(LogLevel.Error, "Ошибка получения списка меню");
    }

    [Fact]
    public async Task GetMenuByIdAsync_ShouldReturnCorrectMenu()
    {
        // Arrange
        var menu = CreateTestMenu();
        await _context.Menu.AddAsync(menu);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMenuByIdAsync(menu.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(menu.Cost, result.Cost);
    }

    [Fact]
    public async Task GetMenuByDayAsync_ShouldReturnLinkedMenu()
    {
        // Arrange
        var dayId = Guid.NewGuid();
        var menu = CreateTestMenu();
        var day = new DayDBModel(dayId, menu.Id, "Test Day", 1, "Desc", 500)
        {
            Menu = menu
        };

        await _context.Days.AddAsync(day);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMenuByDayAsync(dayId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(menu.Name, result.Name);
    }

    [Fact]
    public async Task InsertMenuAsync_ShouldAddNewMenu()
    {
        // Arrange
        var newMenu = new Menu(
            Guid.NewGuid(),
            "New Menu",
            2000);

        // Act
        await _repository.InsertMenuAsync(newMenu);
        var result = await _context.Menu.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newMenu.Name, result.Name);
    }

    [Fact]
    public async Task UpdateMenuAsync_ShouldUpdateExistingMenu()
    {
        // Arrange
        var menu = CreateTestMenu();
        await _context.Menu.AddAsync(menu);
        await _context.SaveChangesAsync();

        var updatedMenu = new Menu(
            menu.Id,
            "Updated Name",
            1500);

        // Act
        await _repository.UpdateMenuAsync(updatedMenu);
        var result = await _context.Menu.FindAsync(menu.Id);

        // Assert
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal(1500, result.Cost);
    }

    [Fact]
    public async Task DeleteMenuAsync_ShouldRemoveMenu()
    {
        // Arrange
        var menu = CreateTestMenu();
        await _context.Menu.AddAsync(menu);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteMenuAsync(menu.Id);
        var result = await _context.Menu.FindAsync(menu.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteMenuAsync_ShouldLogWarningForNonExistingId()
    {
        // Act
        await _repository.DeleteMenuAsync(Guid.NewGuid());

        // Assert
        Mock.Get(_logger).VerifyLog(LogLevel.Warning, "не найдено");
    }
}
