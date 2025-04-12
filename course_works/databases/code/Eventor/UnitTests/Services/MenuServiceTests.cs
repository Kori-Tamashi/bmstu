using Eventor.Common.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Database.Core;
using Eventor.Services;
using Microsoft.Extensions.Logging;

namespace Eventor.Tests.Services;

public class MenuServiceTests
{
    private readonly Mock<IMenuRepository> _mockRepo;
    private readonly MenuService _menuService;
    private readonly Guid _testMenuId = Guid.NewGuid();
    private readonly Guid _testDayId = Guid.NewGuid();
    private readonly Guid _testItemId = Guid.NewGuid();
    private readonly Guid _nonExistingMenuId = Guid.NewGuid();

    public MenuServiceTests()
    {
        _mockRepo = new Mock<IMenuRepository>();
        var logger = Mock.Of<ILogger<MenuService>>();
        _menuService = new MenuService(_mockRepo.Object, logger);
    }

    private Menu CreateTestMenu() => new Menu(
        _testMenuId,
        "Test Menu",
        100
    );

    [Fact]
    public async Task GetAllMenuAsync_ReturnsMenus_WhenDataExists()
    {
        // Arrange
        var expectedMenus = new List<Menu> { CreateTestMenu() };
        _mockRepo.Setup(r => r.GetAllMenuAsync()).ReturnsAsync(expectedMenus);

        // Act
        var result = await _menuService.GetAllMenuAsync();

        // Assert
        Assert.Equal(expectedMenus, result);
        _mockRepo.Verify(r => r.GetAllMenuAsync(), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task GetAllMenuAsync_ThrowsServiceException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.GetAllMenuAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<MenuServiceException>(
            () => _menuService.GetAllMenuAsync());
    }

    [Fact]
    public async Task GetMenuByIdAsync_ReturnsMenu_WhenExists()
    {
        // Arrange
        var expectedMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId)).ReturnsAsync(expectedMenu);

        // Act
        var result = await _menuService.GetMenuByIdAsync(_testMenuId);

        // Assert
        Assert.Equal(expectedMenu.Name, result.Name);
        _mockRepo.Verify(r => r.GetMenuByIdAsync(_testMenuId), Times.Once);
    }

    [Fact]
    public async Task GetMenuByIdAsync_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_nonExistingMenuId))
            .ReturnsAsync((Menu)null);

        // Act & Assert
        await Assert.ThrowsAsync<MenuNotFoundException>(
            () => _menuService.GetMenuByIdAsync(_nonExistingMenuId));
    }

    [Fact]
    public async Task GetMenuByDayAsync_ReturnsMenu_WhenExists()
    {
        // Arrange
        var expectedMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.GetMenuByDayAsync(_testDayId)).ReturnsAsync(expectedMenu);

        // Act
        var result = await _menuService.GetMenuByDayAsync(_testDayId);

        // Assert
        Assert.Equal(expectedMenu.Id, result.Id);
        _mockRepo.Verify(r => r.GetMenuByDayAsync(_testDayId), Times.Once);
    }

    [Fact]
    public async Task AddMenuAsync_SavesMenu_WithCorrectData()
    {
        // Arrange
        var newMenu = CreateTestMenu();
        _mockRepo.Setup(r => r.InsertMenuAsync(newMenu)).Returns(Task.CompletedTask);

        // Act
        await _menuService.AddMenuAsync(newMenu);

        // Assert
        _mockRepo.Verify(r => r.InsertMenuAsync(It.Is<Menu>(m =>
            m.Id == newMenu.Id &&
            m.Name == "Test Menu")),
            Times.Once);
    }

    [Fact]
    public async Task AddItemAsync_CallsRepository_WithCorrectParameters()
    {
        // Arrange
        const int amount = 5;
        _mockRepo.Setup(r => r.InsertItemAsync(_testMenuId, _testItemId, amount))
            .Returns(Task.CompletedTask);

        // Act
        await _menuService.AddItemAsync(_testMenuId, _testItemId, amount);

        // Assert
        _mockRepo.Verify(r => r.InsertItemAsync(
            _testMenuId,
            _testItemId,
            It.Is<int>(a => a == amount)),
            Times.Once);
    }

    [Fact]
    public async Task UpdateMenuAsync_UpdatesExistingMenu_WhenValid()
    {
        // Arrange
        var existingMenu = CreateTestMenu();
        var updatedMenu = existingMenu;
        existingMenu.Name = "Updated Menu";

        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId)).ReturnsAsync(existingMenu);
        _mockRepo.Setup(r => r.UpdateMenuAsync(updatedMenu)).Returns(Task.CompletedTask);

        // Act
        await _menuService.UpdateMenuAsync(updatedMenu);

        // Assert
        _mockRepo.Verify(r => r.UpdateMenuAsync(It.Is<Menu>(m =>
            m.Id == _testMenuId &&
            m.Name == "Updated Menu")),
            Times.Once);
    }

    [Fact]
    public async Task DeleteMenuAsync_DeletesMenu_WhenExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMenuByIdAsync(_testMenuId))
            .ReturnsAsync(CreateTestMenu());

        // Act
        await _menuService.DeleteMenuAsync(_testMenuId);

        // Assert
        _mockRepo.Verify(r => r.DeleteMenuAsync(_testMenuId), Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_CallsRepository_WithCorrectParameters()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteItemAsync(_testMenuId, _testItemId))
            .Returns(Task.CompletedTask);

        // Act
        await _menuService.DeleteItemAsync(_testMenuId, _testItemId);

        // Assert
        _mockRepo.Verify(r => r.DeleteItemAsync(
            _testMenuId,
            _testItemId),
            Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task DeleteItemAsync_ThrowsUpdateException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.DeleteItemAsync(_testMenuId, _testItemId))
            .ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<MenuUpdateException>(
            () => _menuService.DeleteItemAsync(_testMenuId, _testItemId));
    }
}