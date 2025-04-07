using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Services;

namespace Eventor.Tests.Services;

public class MenuServiceTests
{
    private readonly Mock<IMenuRepository> _mockRepo;
    private readonly Mock<ILogger<MenuService>> _mockLogger;
    private readonly MenuService _menuService;
    private readonly Guid _testMenuId = Guid.NewGuid();
    private readonly Guid _testDayId = Guid.NewGuid();
    private readonly Guid _testItemId = Guid.NewGuid();

    public MenuServiceTests()
    {
        _mockRepo = new Mock<IMenuRepository>();
        _mockLogger = new Mock<ILogger<MenuService>>();
        _menuService = new MenuService(_mockRepo.Object, _mockLogger.Object);
    }

    private Menu CreateTestMenu() => new Menu(
        _testMenuId,
        "Test Menu",
        100
    );

    // GetAllMenuAsync
    [Fact]
    public async Task GetAllMenuAsync_ReturnsMenus()
    {
        // Arrange
        var menus = new List<Menu> { CreateTestMenu() };
        _mockRepo.Setup(r => r.GetAllMenuAsync()).ReturnsAsync(menus);

        // Act
        var result = await _menuService.GetAllMenuAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testMenuId, result[0].Id);
    }

    [Fact]
    public async Task GetAllMenuAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllMenuAsync())
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<MenuServiceException>(
            () => _menuService.GetAllMenuAsync());
    }

    // GetMenuByIdAsync
    [Fact]
    public async Task GetMenuByIdAsync_ValidId_ReturnsMenu()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId))
            .ReturnsAsync(testMenu);

        // Act
        var result = await _menuService.GetMenuByIdAsync(_testMenuId);

        // Assert
        Assert.Equal(_testMenuId, result.Id);
    }

    [Fact]
    public async Task GetMenuByIdAsync_NotFound_ThrowsException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMenuByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Menu)null!);

        // Act & Assert
        await Assert.ThrowsAsync<MenuNotFoundException>(
            () => _menuService.GetMenuByIdAsync(Guid.NewGuid()));
    }

    // GetMenuByDayAsync
    [Fact]
    public async Task GetMenuByDayAsync_ValidDay_ReturnsMenu()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByDayAsync(_testDayId))
            .ReturnsAsync(testMenu);

        // Act
        var result = await _menuService.GetMenuByDayAsync(_testDayId);

        // Assert
        Assert.Equal(_testMenuId, result.Id);
    }

    [Fact]
    public async Task GetMenuByDayAsync_NotFound_ThrowsException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMenuByDayAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Menu)null!);

        // Act & Assert
        await Assert.ThrowsAsync<MenuNotFoundException>(
            () => _menuService.GetMenuByDayAsync(Guid.NewGuid()));
    }

    // AddMenuAsync
    [Fact]
    public async Task AddMenuAsync_ValidMenu_SavesSuccessfully()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.InsertMenuAsync(testMenu))
            .Returns(Task.CompletedTask);

        // Act
        await _menuService.AddMenuAsync(testMenu);

        // Assert
        _mockRepo.Verify(r => r.InsertMenuAsync(testMenu), Times.Once);
    }

    [Fact]
    public async Task AddMenuAsync_DbException_ThrowsCreateException()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.InsertMenuAsync(testMenu))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<MenuCreateException>(
            () => _menuService.AddMenuAsync(testMenu));
    }

    // AddItemAsync
    [Fact]
    public async Task AddItemAsync_ValidData_SavesSuccessfully()
    {
        // Arrange
        _mockRepo.Setup(r => r.InsertItemAsync(_testMenuId, _testItemId, 5))
            .Returns(Task.CompletedTask);

        // Act
        await _menuService.AddItemAsync(_testMenuId, _testItemId, 5);

        // Assert
        _mockRepo.Verify(r => r.InsertItemAsync(_testMenuId, _testItemId, 5), Times.Once);
    }

    [Fact]
    public async Task AddItemAsync_DbException_ThrowsUpdateException()
    {
        // Arrange
        _mockRepo.Setup(r => r.InsertItemAsync(_testMenuId, _testItemId, 5))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<MenuUpdateException>(
            () => _menuService.AddItemAsync(_testMenuId, _testItemId, 5));
    }

    // UpdateMenuAsync
    [Fact]
    public async Task UpdateMenuAsync_ValidMenu_UpdatesSuccessfully()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId))
            .ReturnsAsync(testMenu);

        // Act
        await _menuService.UpdateMenuAsync(testMenu);

        // Assert
        _mockRepo.Verify(r => r.UpdateMenuAsync(testMenu), Times.Once);
    }

    [Fact]
    public async Task UpdateMenuAsync_NotFound_ThrowsException()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId))
            .ReturnsAsync((Menu)null!);

        // Act & Assert
        await Assert.ThrowsAsync<MenuNotFoundException>(
            () => _menuService.UpdateMenuAsync(testMenu));
    }

    // DeleteMenuAsync
    [Fact]
    public async Task DeleteMenuAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var testMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId))
            .ReturnsAsync(testMenu);

        // Act
        await _menuService.DeleteMenuAsync(_testMenuId);

        // Assert
        _mockRepo.Verify(r => r.DeleteMenuAsync(_testMenuId), Times.Once);
    }

    [Fact]
    public async Task DeleteMenuAsync_NotFound_ThrowsException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId))
            .ReturnsAsync((Menu)null!);

        // Act & Assert
        await Assert.ThrowsAsync<MenuNotFoundException>(
            () => _menuService.DeleteMenuAsync(_testMenuId));
    }

    // DeleteItemAsync
    [Fact]
    public async Task DeleteItemAsync_ValidData_DeletesSuccessfully()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteItemAsync(_testMenuId, _testItemId))
            .Returns(Task.CompletedTask);

        // Act
        await _menuService.DeleteItemAsync(_testMenuId, _testItemId);

        // Assert
        _mockRepo.Verify(r => r.DeleteItemAsync(_testMenuId, _testItemId), Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_DbException_ThrowsUpdateException()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteItemAsync(_testMenuId, _testItemId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<MenuUpdateException>(
            () => _menuService.DeleteItemAsync(_testMenuId, _testItemId));
    }
}