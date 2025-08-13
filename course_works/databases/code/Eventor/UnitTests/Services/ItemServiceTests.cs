using Eventor.Common.Core;
using Eventor.Common.Enums;
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

public class ItemServiceTests
{
    private readonly Mock<IItemRepository> _mockRepo;
    private readonly ItemService _itemService;
    private readonly Guid _testItemId = Guid.NewGuid();
    private readonly Guid _testMenuId = Guid.NewGuid();
    private readonly Guid _nonExistingItemId = Guid.NewGuid();

    public ItemServiceTests()
    {
        _mockRepo = new Mock<IItemRepository>();
        var logger = Mock.Of<ILogger<ItemService>>();
        _itemService = new ItemService(_mockRepo.Object, logger);
    }

    private Item CreateTestItem() => new Item(
        _testItemId,
        "Test Item",
        ItemType.OneDay,
        100.0
    );

    [Fact]
    public async Task GetAllItemsAsync_ReturnsItems_WhenDataExists()
    {
        // Arrange
        var expectedItems = new List<Item> { CreateTestItem() };
        _mockRepo.Setup(r => r.GetAllItemsAsync()).ReturnsAsync(expectedItems);

        // Act
        var result = await _itemService.GetAllItemsAsync();

        // Assert
        Assert.Equal(expectedItems, result);
        _mockRepo.Verify(r => r.GetAllItemsAsync(), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task GetAllItemsAsync_ThrowsServiceException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.GetAllItemsAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<ItemServiceException>(
            () => _itemService.GetAllItemsAsync());
    }

    [Fact]
    public async Task GetAllItemsByMenuAsync_ReturnsItems_ForValidMenu()
    {
        // Arrange
        var items = new List<Item> { CreateTestItem() };
        _mockRepo.Setup(r => r.GetAllItemsByMenuAsync(_testMenuId)).ReturnsAsync(items);

        // Act
        var result = await _itemService.GetAllItemsByMenuAsync(_testMenuId);

        // Assert
        Assert.Single(result);
        _mockRepo.Verify(r => r.GetAllItemsByMenuAsync(_testMenuId), Times.Once);
    }

    [Fact]
    public async Task GetItemByIdAsync_ReturnsItem_WhenExists()
    {
        // Arrange
        var expectedItem = CreateTestItem();
        _mockRepo.Setup(r => r.GetItemByIdAsync(_testItemId)).ReturnsAsync(expectedItem);

        // Act
        var result = await _itemService.GetItemByIdAsync(_testItemId);

        // Assert
        Assert.Equal(expectedItem.Price, result.Price);
        _mockRepo.Verify(r => r.GetItemByIdAsync(_testItemId), Times.Once);
    }

    [Fact]
    public async Task GetItemByIdAsync_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetItemByIdAsync(_nonExistingItemId)).ReturnsAsync((Item)null);

        // Act & Assert
        await Assert.ThrowsAsync<ItemNotFoundException>(
            () => _itemService.GetItemByIdAsync(_nonExistingItemId));
    }

    [Fact]
    public async Task AddItemAsync_SavesItem_WithCorrectData()
    {
        // Arrange
        var newItem = CreateTestItem();
        _mockRepo.Setup(r => r.InsertItemAsync(newItem)).Returns(Task.CompletedTask);

        // Act
        await _itemService.AddItemAsync(newItem);

        // Assert
        _mockRepo.Verify(r => r.InsertItemAsync(It.Is<Item>(i =>
            i.Id == newItem.Id &&
            i.Name == "Test Item")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateItemAsync_UpdatesExistingItem_WhenValid()
    {
        // Arrange
        var existingItem = CreateTestItem();
        var updatedItem = existingItem;
        updatedItem.Price = 150;

        _mockRepo.Setup(r => r.GetItemByIdAsync(_testItemId)).ReturnsAsync(existingItem);
        _mockRepo.Setup(r => r.UpdateItemAsync(updatedItem)).Returns(Task.CompletedTask);

        // Act
        await _itemService.UpdateItemAsync(updatedItem);

        // Assert
        _mockRepo.Verify(r => r.UpdateItemAsync(It.Is<Item>(i =>
            i.Id == _testItemId &&
            i.Price == 150.0)),
            Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_DeletesItem_WhenExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetItemByIdAsync(_testItemId)).ReturnsAsync(CreateTestItem());

        // Act
        await _itemService.DeleteItemAsync(_testItemId);

        // Assert
        _mockRepo.Verify(r => r.DeleteItemAsync(_testItemId), Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetItemByIdAsync(_nonExistingItemId)).ReturnsAsync((Item)null);

        // Act & Assert
        await Assert.ThrowsAsync<ItemNotFoundException>(
            () => _itemService.DeleteItemAsync(_nonExistingItemId));
    }
}