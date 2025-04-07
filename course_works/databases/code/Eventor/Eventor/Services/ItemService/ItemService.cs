using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            _logger.LogError(ex, "Error retrieving items");
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
                _logger.LogWarning("No items found for menu {MenuId}", menuId);
            }
            return items;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving items for menu {MenuId}", menuId);
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
                throw new ItemNotFoundException($"Item {itemId} not found");
            }
            return item;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving item {ItemId}", itemId);
            throw new ItemServiceException($"Failed to retrieve item {itemId}", ex);
        }
    }

    public async Task AddItemAsync(Item item)
    {
        try
        {
            await _itemRepository.InsertItemAsync(item);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating item");
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
                throw new ItemNotFoundException($"Item {item.Id} not found");
            }

            await _itemRepository.UpdateItemAsync(item);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating item {ItemId}", item.Id);
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
                throw new ItemNotFoundException($"Item {itemId} not found");
            }

            await _itemRepository.DeleteItemAsync(itemId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting item {ItemId}", itemId);
            throw new ItemDeleteException($"Failed to delete item {itemId}", ex);
        }
    }
}