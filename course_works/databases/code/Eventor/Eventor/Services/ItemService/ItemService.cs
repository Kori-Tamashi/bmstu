using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventor.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IItemRepository itemRepository, ILogger<ItemService> logger)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        try
        {
            return await _itemRepository.GetAllItemsAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving items");
            throw new ItemServiceException("Failed to retrieve items due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving items");
            throw new ItemServiceException("Failed to retrieve items due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving items");
            throw new ItemServiceException("Failed to retrieve items", ex);
        }
    }

    public async Task<List<Item>> GetAllItemsByMenuAsync(Guid menuId)
    {
        try
        {
            var items = await _itemRepository.GetAllItemsByMenuAsync(menuId);

            if (items == null || items.Count == 0)
            {
                _logger.LogInformation("No items found for menu {MenuId}", menuId);
                return new List<Item>();
            }
            return items;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid menu ID: {MenuId}", menuId);
            throw new ItemServiceException($"Invalid menu ID: {menuId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving items for menu {MenuId}", menuId);
            throw new ItemServiceException($"Failed to retrieve items for menu {menuId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving items for menu {MenuId}", menuId);
            throw new ItemServiceException($"Failed to retrieve items for menu {menuId}", ex);
        }
    }

    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        try
        {
            var item = await _itemRepository.GetItemByIdAsync(itemId);

            if (item == null)
            {
                _logger.LogWarning("Item {ItemId} not found", itemId);
                throw new ItemNotFoundException($"Item {itemId} not found");
            }
            return item;
        }
        catch (ItemNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid item ID: {ItemId}", itemId);
            throw new ItemServiceException($"Invalid item ID: {itemId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving item {ItemId}", itemId);
            throw new ItemServiceException($"Failed to retrieve item {itemId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving item {ItemId}", itemId);
            throw new ItemServiceException($"Failed to retrieve item {itemId}", ex);
        }
    }

    public async Task AddItemAsync(Item item)
    {
        try
        {
            await _itemRepository.InsertItemAsync(item);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error while creating item");
            throw new ItemCreateException($"Failed to create item: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while creating item");
            throw new ItemCreateException("Failed to create item due to database constraints", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data conflict while creating item");
            throw new ItemCreateException("Failed to create item due to data conflict", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating item");
            throw new ItemCreateException("Failed to create item", ex);
        }
    }

    public async Task UpdateItemAsync(Item item)
    {
        try
        {
            var existingItem = await _itemRepository.GetItemByIdAsync(item.Id);

            if (existingItem == null)
            {
                _logger.LogWarning("Item {ItemId} not found for update", item.Id);
                throw new ItemNotFoundException($"Item {item.Id} not found");
            }

            await _itemRepository.UpdateItemAsync(item);
        }
        catch (ItemNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid data while updating item {ItemId}", item.Id);
            throw new ItemUpdateException($"Failed to update item {item.Id}: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating item {ItemId}", item.Id);
            throw new ItemUpdateException($"Failed to update item {item.Id} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while updating item {ItemId}", item.Id);
            throw new ItemUpdateException($"Concurrency conflict while updating item {item.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating item {ItemId}", item.Id);
            throw new ItemUpdateException($"Failed to update item {item.Id}", ex);
        }
    }

    public async Task DeleteItemAsync(Guid itemId)
    {
        try
        {
            var existingItem = await _itemRepository.GetItemByIdAsync(itemId);

            if (existingItem == null)
            {
                _logger.LogWarning("Item {ItemId} not found for deletion", itemId);
                throw new ItemNotFoundException($"Item {itemId} not found");
            }

            await _itemRepository.DeleteItemAsync(itemId);
        }
        catch (ItemNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid item ID: {ItemId}", itemId);
            throw new ItemDeleteException($"Invalid item ID: {itemId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting item {ItemId}", itemId);
            throw new ItemDeleteException($"Failed to delete item {itemId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while deleting item {ItemId}", itemId);
            throw new ItemDeleteException($"Concurrency conflict while deleting item {itemId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting item {ItemId}", itemId);
            throw new ItemDeleteException($"Failed to delete item {itemId}", ex);
        }
    }
}