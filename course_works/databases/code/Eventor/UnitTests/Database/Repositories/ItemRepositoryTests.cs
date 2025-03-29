using Eventor.Common.Core;
using Eventor.Common.Enums;
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

public class ItemRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly ItemRepository _repository;
    private readonly Mock<DbSet<ItemDBModel>> _mockItemsDbSet;

    public ItemRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockItemsDbSet = new Mock<DbSet<ItemDBModel>>();

        _mockContext.Setup(c => c.Items).Returns(_mockItemsDbSet.Object);
        _repository = new ItemRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetAllItemsAsync_ReturnsAllItems()
    {
        // Arrange
        var items = new List<ItemDBModel>
        {
            new(Guid.NewGuid(), "Item 1", ItemType.OneDay, 100),
            new(Guid.NewGuid(), "Item 2", ItemType.MultiDay, 200)
        };

        var mockSet = SetupMockDbSet(items.AsQueryable());
        _mockContext.Setup(c => c.Items).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAllItemsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Name == "Item 1");
    }

    [Fact]
    public async Task GetAllItemsByMenuAsync_ReturnsMenuItems()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var menuId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        // Заполнение тестовыми данными
        using (var context = new EventorDBContext(options))
        {
            context.MenuItems.Add(new MenuItemsDBModel(menuId, itemId, 3)
            {
                Item = new ItemDBModel(itemId, "Menu Item", ItemType.OneDay, 150)
            });
            await context.SaveChangesAsync();
        }

        // Создание нового контекста для теста
        using (var context = new EventorDBContext(options))
        {
            var repository = new ItemRepository(context);

            // Act
            var result = await repository.GetAllItemsByMenuAsync(menuId);

            // Assert
            var item = Assert.Single(result);
            Assert.Equal("Menu Item", item.Name);
            Assert.Equal(ItemType.OneDay, item.Type);
        }
    }

    [Fact]
    public async Task GetItemByIdAsync_ReturnsCorrectItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var testItem = new ItemDBModel(itemId, "Test Item", ItemType.OneDay, 50);

        var mockSet = SetupMockDbSet(new List<ItemDBModel> { testItem }.AsQueryable());
        _mockContext.Setup(c => c.Items).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetItemByIdAsync(itemId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(itemId, result.Id);
    }

    [Fact]
    public async Task InsertItemAsync_AddsNewItemToDatabase()
    {
        // Arrange
        var newItem = new Item(Guid.NewGuid(), "New Item", ItemType.MultiDay, 300);
        ItemDBModel capturedItem = null;

        _mockItemsDbSet.Setup(m => m.AddAsync(It.IsAny<ItemDBModel>(), default))
            .Callback<ItemDBModel, CancellationToken>((i, _) => capturedItem = i)
            .ReturnsAsync(() => null);

        // Act
        await _repository.InsertItemAsync(newItem);

        // Assert
        Assert.NotNull(capturedItem);
        Assert.Equal(newItem.Name, capturedItem.Name);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateItemAsync_UpdatesExistingItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var existingItem = new ItemDBModel(itemId, "Old Name", ItemType.OneDay, 100);
        var updatedItem = new Item(itemId, "New Name", ItemType.MultiDay, 200);

        var mockSet = SetupMockDbSet(new List<ItemDBModel> { existingItem }.AsQueryable());
        _mockContext.Setup(c => c.Items).Returns(mockSet.Object);

        // Act
        await _repository.UpdateItemAsync(updatedItem);

        // Assert
        Assert.Equal("New Name", existingItem.Name);
        Assert.Equal(ItemType.MultiDay, existingItem.Type);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_RemovesItemFromDatabase()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var testItem = new ItemDBModel(itemId, "Test Item", ItemType.OneDay, 75);

        // Настраиваем DbSet с поддержкой асинхронных операций
        var mockSet = SetupMockDbSet(new List<ItemDBModel> { testItem }.AsQueryable());

        // Настраиваем FirstOrDefaultAsync для возврата testItem
        mockSet.As<IAsyncEnumerable<ItemDBModel>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<ItemDBModel>(new List<ItemDBModel> { testItem }.GetEnumerator()));

        _mockContext.Setup(c => c.Items).Returns(mockSet.Object);

        // Act
        await _repository.DeleteItemAsync(itemId);

        // Assert
        mockSet.Verify(m => m.Remove(It.Is<ItemDBModel>(i => i.Id == itemId)), Times.Once);
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