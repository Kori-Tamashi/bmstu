using Eventor.Database.Core;
using Eventor.Common.Core;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
using Eventor.Common.Converter;
namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с предметами меню
/// </summary>
public class ItemRepository : BaseRepository, IItemRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория предметов
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public ItemRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить все предметы
    /// </summary>
    /// <returns>Список всех предметов</returns>
    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _dbContext.Items
            .Select(i => ItemConverter.ConvertDBToCore(i))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить все предметы конкретного меню
    /// </summary>
    /// <param name="menuId">Идентификатор меню</param>
    /// <returns>Список предметов в указанном меню</returns>
    public async Task<List<Item>> GetAllItemsByMenuAsync(Guid menuId)
    {
        return await _dbContext.MenuItems
            .Where(mi => mi.MenuId == menuId)
            .Include(mi => mi.Item)
            .Select(mi => ItemConverter.ConvertDBToCore(mi.Item))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить предмет по идентификатору
    /// </summary>
    /// <param name="itemId">Идентификатор предмета</param>
    /// <returns>Найденный предмет или null</returns>
    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        var itemEntity = await _dbContext.Items
            .FirstOrDefaultAsync(i => i.Id == itemId);

        return itemEntity != null
            ? ItemConverter.ConvertDBToCore(itemEntity)
            : null;
    }

    /// <summary>
    /// Добавить новый предмет
    /// </summary>
    /// <param name="item">Модель предмета для добавления</param>
    public async Task InsertItemAsync(Item item)
    {
        var itemEntity = ItemConverter.ConvertCoreToDB(item);
        await _dbContext.Items.AddAsync(itemEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить существующий предмет
    /// </summary>
    /// <param name="item">Модель с обновленными данными предмета</param>
    public async Task UpdateItemAsync(Item item)
    {
        var existingItem = await _dbContext.Items
            .FirstOrDefaultAsync(i => i.Id == item.Id);

        if (existingItem == null) return;

        existingItem.Name = item.Name;
        existingItem.Type = item.Type;
        existingItem.Price = item.Price;

        _dbContext.Items.Update(existingItem);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить предмет по идентификатору
    /// </summary>
    /// <param name="itemId">Идентификатор удаляемого предмета</param>
    public async Task DeleteItemAsync(Guid itemId)
    {
        var itemToDelete = await _dbContext.Items
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (itemToDelete == null) return;

        _dbContext.Items.Remove(itemToDelete);
        await _dbContext.SaveChangesAsync();
    }
}