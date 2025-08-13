using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

public class ItemRepositoryTests : IDisposable
{
    private readonly EventorDBContext _context;
    private readonly ILogger<ItemRepository> _logger;
    private readonly ItemRepository _repository;
    private readonly DbContextOptions<EventorDBContext> _options;

    public ItemRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventorDBContext(_options);
        _logger = Mock.Of<ILogger<ItemRepository>>();
        _repository = new ItemRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private ItemDBModel CreateTestItem(
        Guid? id = null,
        string name = "Test Item",
        ItemType type = ItemType.OneDay,
        double price = 100)
    {
        return new ItemDBModel(
            id ?? Guid.NewGuid(),
            name,
            type,
            price);
    }

    [Fact]
    public async Task GetAllItemsAsync_ShouldReturnAllItems()
    {
        // Arrange
        var items = new List<ItemDBModel>
        {
            CreateTestItem(name: "Item 1"),
            CreateTestItem(name: "Item 2")
        };

        await _context.Items.AddRangeAsync(items);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllItemsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Name == "Item 1");
    }

    [Fact]
    public async Task GetAllItemsAsync_ShouldLogErrorOnFailure()
    {
        // Arrange
        var corruptedContext = new Mock<EventorDBContext>(_options);
        corruptedContext.Setup(c => c.Items).Throws<Exception>();
        var repository = new ItemRepository(corruptedContext.Object, _logger);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllItemsAsync());
        Mock.Get(_logger).VerifyLog(LogLevel.Error, "Ошибка получения списка предметов");
    }

    [Fact]
    public async Task GetAllItemsByMenuAsync_ShouldReturnFilteredItems()
    {
        // Arrange
        var menuId = Guid.NewGuid();
        var item = CreateTestItem();

        await _context.MenuItems.AddAsync(new MenuItemsDBModel(menuId, item.Id, 2) { Item = item });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllItemsByMenuAsync(menuId);

        // Assert
        Assert.Single(result);
        Assert.Equal(item.Name, result[0].Name);
    }

    [Fact]
    public async Task GetItemByIdAsync_ShouldReturnCorrectItem()
    {
        // Arrange
        var item = CreateTestItem();
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetItemByIdAsync(item.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(item.Price, result.Price);
    }

    [Fact]
    public async Task GetItemByIdAsync_ShouldReturnNullForNonExistingId()
    {
        // Act
        var result = await _repository.GetItemByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertItemAsync_ShouldAddNewItem()
    {
        // Arrange
        var newItem = new Item(
            Guid.NewGuid(),
            "New Item",
            ItemType.MultiDay,
            200);

        // Act
        await _repository.InsertItemAsync(newItem);
        var result = await _context.Items.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newItem.Name, result.Name);
    }

    [Fact]
    public async Task InsertItemAsync_ShouldThrowOnDatabaseError()
    {
        // Arrange
        var newItem = new Item(Guid.NewGuid(), "Test", ItemType.OneDay, 100);

        // Настраиваем мок DbSet
        var mockSet = new Mock<DbSet<ItemDBModel>>();
        mockSet.Setup(m => m.AddAsync(It.IsAny<ItemDBModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ItemDBModel model, CancellationToken token) => null);

        // Настраиваем контекст
        var corruptedContext = new Mock<EventorDBContext>(_options);
        corruptedContext.Setup(c => c.Items).Returns(mockSet.Object);
        corruptedContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Test error"));

        var repository = new ItemRepository(corruptedContext.Object, _logger);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.InsertItemAsync(newItem));

        Assert.Contains("Не удалось создать предмет", exception.Message);
        Mock.Get(_logger).VerifyLog(LogLevel.Error, "Ошибка создания предмета");
    }

    [Fact]
    public async Task UpdateItemAsync_ShouldUpdateExistingItem()
    {
        // Arrange
        var item = CreateTestItem();
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();

        var updatedItem = new Item(
            item.Id,
            "Updated Name",
            ItemType.MultiDay,
            150);

        // Act
        await _repository.UpdateItemAsync(updatedItem);
        var result = await _context.Items.FindAsync(item.Id);

        // Assert
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal(ItemType.MultiDay, result.Type);
    }

    [Fact]
    public async Task UpdateItemAsync_ShouldNotUpdateNonExistingItem()
    {
        // Arrange
        var nonExistingItem = new Item(
            Guid.NewGuid(),
            "Non-existing",
            ItemType.OneDay,
            50);

        // Act
        await _repository.UpdateItemAsync(nonExistingItem);
        var count = await _context.Items.CountAsync();

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task DeleteItemAsync_ShouldRemoveItem()
    {
        // Arrange
        var item = CreateTestItem();
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteItemAsync(item.Id);
        var result = await _context.Items.FindAsync(item.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteItemAsync_ShouldLogWarningForNonExistingId()
    {
        // Act
        await _repository.DeleteItemAsync(Guid.NewGuid());

        // Assert
        Mock.Get(_logger).VerifyLog(LogLevel.Warning, "не найден");
    }
}