using Eventor.Database.Core;
using Eventor.Common.Core;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
using Eventor.Common.Converter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с предметами меню
/// </summary>
public class ItemRepository : BaseRepository, IItemRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<ItemRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория предметов
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public ItemRepository(
        EventorDBContext dbContext,
        ILogger<ItemRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все предметы
    /// </summary>
    public async Task<List<Item>> GetAllItemsAsync()
    {
        try
        {
            return await _dbContext.Items
                .Select(i => ItemConverter.ConvertDBToCore(i))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка предметов");
            throw new InvalidOperationException("Не удалось получить список предметов", ex);
        }
    }

    /// <summary>
    /// Получить все предметы конкретного меню
    /// </summary>
    public async Task<List<Item>> GetAllItemsByMenuAsync(Guid menuId)
    {
        try
        {
            return await _dbContext.MenuItems
                .Where(mi => mi.MenuId == menuId)
                .Include(mi => mi.Item)
                .Select(mi => ItemConverter.ConvertDBToCore(mi.Item))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения предметов меню {MenuId}", menuId);
            throw new InvalidOperationException($"Не удалось получить предметы меню {menuId}", ex);
        }
    }

    /// <summary>
    /// Получить предмет по идентификатору
    /// </summary>
    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        try
        {
            var itemEntity = await _dbContext.Items
                .FirstOrDefaultAsync(i => i.Id == itemId);

            return itemEntity != null
                ? ItemConverter.ConvertDBToCore(itemEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения предмета {ItemId}", itemId);
            throw new InvalidOperationException($"Не удалось получить предмет {itemId}", ex);
        }
    }

    /// <summary>
    /// Добавить новый предмет
    /// </summary>
    public async Task InsertItemAsync(Item item)
    {
        try
        {
            var itemEntity = ItemConverter.ConvertCoreToDB(item);
            await _dbContext.Items.AddAsync(itemEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания предмета");
            throw new InvalidOperationException("Не удалось создать предмет", ex);
        }
    }

    /// <summary>
    /// Обновить существующий предмет
    /// </summary>
    public async Task UpdateItemAsync(Item item)
    {
        try
        {
            var existingItem = await _dbContext.Items
                .FirstOrDefaultAsync(i => i.Id == item.Id);

            if (existingItem == null)
            {
                _logger.LogWarning("Предмет {ItemId} не найден", item.Id);
                return;
            }

            existingItem.Name = item.Name;
            existingItem.Type = item.Type;
            existingItem.Price = item.Price;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления предмета {ItemId}", item.Id);
            throw new InvalidOperationException($"Не удалось обновить предмет {item.Id}", ex);
        }
    }

    /// <summary>
    /// Удалить предмет по идентификатору
    /// </summary>
    public async Task DeleteItemAsync(Guid itemId)
    {
        try
        {
            var itemToDelete = await _dbContext.Items
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (itemToDelete == null)
            {
                _logger.LogWarning("Предмет {ItemId} не найден", itemId);
                return;
            }

            _dbContext.Items.Remove(itemToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления предмета {ItemId}", itemId);
            throw new InvalidOperationException($"Не удалось удалить предмет {itemId}", ex);
        }
    }
}