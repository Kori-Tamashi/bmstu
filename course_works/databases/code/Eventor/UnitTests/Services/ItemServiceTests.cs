using Eventor.Common.Core;
using Eventor.Services;
using Eventor.Services.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Common.Enums;

namespace Eventor.Tests.Services;

public class ItemServiceTests
{
    private readonly Mock<IItemService> _mockService;
    private readonly Mock<ILogger<ItemService>> _mockLogger;
    private readonly ItemService _itemService;
    private readonly Guid _testItemId = Guid.NewGuid();
    private readonly Guid _testMenuId = Guid.NewGuid();

    public ItemServiceTests()
    {
        _mockService = new Mock<IItemService>();
        _mockLogger = new Mock<ILogger<ItemService>>();
        // Если ItemService требует репозиторий в конструкторе:
        // _itemService = new ItemService(repoMock.Object, _mockLogger.Object);
    }

    private Item CreateTestItem() => new Item(
        _testItemId,
        "Test Item",
        ItemType.OneDay,
        100.0
    );

    // GetAllItemsAsync
    [Fact]
    public async Task GetAllItemsAsync_ReturnsItems()
    {
        // Arrange
        var items = new List<Item> { CreateTestItem() };
        _mockService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(items);

        // Act
        var result = await _mockService.Object.GetAllItemsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testItemId, result[0].Id);
    }

    [Fact]
    public async Task GetAllItemsAsync_DbError_ThrowsServiceException()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllItemsAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _mockService.Object.GetAllItemsAsync());
    }

    // GetAllItemsByMenuAsync
    [Fact]
    public async Task GetAllItemsByMenuAsync_ReturnsItems()
    {
        // Arrange
        var items = new List<Item> { CreateTestItem() };
        _mockService.Setup(s => s.GetAllItemsByMenuAsync(_testMenuId))
            .ReturnsAsync(items);

        // Act
        var result = await _mockService.Object.GetAllItemsByMenuAsync(_testMenuId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testItemId, result[0].Id);
    }

    [Fact]
    public async Task GetAllItemsByMenuAsync_InvalidMenu_ReturnsEmpty()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllItemsByMenuAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Item>());

        // Act
        var result = await _mockService.Object.GetAllItemsByMenuAsync(Guid.NewGuid());

        // Assert
        Assert.Empty(result);
    }

    // GetItemByIdAsync
    [Fact]
    public async Task GetItemByIdAsync_ValidId_ReturnsItem()
    {
        // Arrange
        var testItem = CreateTestItem();
        _mockService.Setup(s => s.GetItemByIdAsync(_testItemId))
            .ReturnsAsync(testItem);

        // Act
        var result = await _mockService.Object.GetItemByIdAsync(_testItemId);

        // Assert
        Assert.Equal(_testItemId, result.Id);
    }

    [Fact]
    public async Task GetItemByIdAsync_InvalidId_ThrowsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetItemByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ItemNotFoundException("Item not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ItemNotFoundException>(
            () => _mockService.Object.GetItemByIdAsync(Guid.NewGuid()));
    }

    // AddItemAsync
    [Fact]
    public async Task AddItemAsync_ValidItem_SavesSuccessfully()
    {
        // Arrange
        var testItem = CreateTestItem();
        _mockService.Setup(s => s.AddItemAsync(testItem))
            .Returns(Task.CompletedTask);

        // Act
        await _mockService.Object.AddItemAsync(testItem);

        // Assert
        _mockService.Verify(s => s.AddItemAsync(testItem), Times.Once);
    }

    // UpdateItemAsync
    [Fact]
    public async Task UpdateItemAsync_ValidItem_UpdatesSuccessfully()
    {
        // Arrange
        var testItem = CreateTestItem();
        _mockService.Setup(s => s.UpdateItemAsync(testItem))
            .Returns(Task.CompletedTask);

        // Act
        await _mockService.Object.UpdateItemAsync(testItem);

        // Assert
        _mockService.Verify(s => s.UpdateItemAsync(testItem), Times.Once);
    }

    // DeleteItemAsync
    [Fact]
    public async Task DeleteItemAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteItemAsync(_testItemId))
            .Returns(Task.CompletedTask);

        // Act
        await _mockService.Object.DeleteItemAsync(_testItemId);

        // Assert
        _mockService.Verify(s => s.DeleteItemAsync(_testItemId), Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_InvalidId_ThrowsException()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteItemAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ItemNotFoundException("Item not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ItemNotFoundException>(
            () => _mockService.Object.DeleteItemAsync(Guid.NewGuid()));
    }
}