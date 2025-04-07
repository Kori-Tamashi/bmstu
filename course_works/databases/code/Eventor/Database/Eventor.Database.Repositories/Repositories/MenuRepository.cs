using Eventor.Database.Core;
using Eventor.Database.Models;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с меню мероприятий
/// </summary>
public class MenuRepository : BaseRepository, IMenuRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория меню
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public MenuRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить все меню
    /// </summary>
    /// <returns>Список всех меню</returns>
    public async Task<List<Menu>> GetAllMenuAsync()
    {
        return await _dbContext.Menu
            .Include(m => m.MenuItems)
            .ThenInclude(mi => mi.Item)
            .Select(m => MenuConverter.ConvertDBToCore(m))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить меню по идентификатору
    /// </summary>
    /// <param name="menuId">Идентификатор меню</param>
    /// <returns>Найденное меню или null</returns>
    public async Task<Menu> GetMenuByIdAsync(Guid menuId)
    {
        var menuEntity = await _dbContext.Menu
            .Include(m => m.MenuItems)
            .ThenInclude(mi => mi.Item)
            .FirstOrDefaultAsync(m => m.Id == menuId);

        return menuEntity != null
            ? MenuConverter.ConvertDBToCore(menuEntity)
            : null;
    }

    /// <summary>
    /// Получить меню конкретного дня мероприятия
    /// </summary>
    /// <param name="dayId">Идентификатор дня мероприятия</param>
    /// <returns>Меню, привязанное к указанному дню</returns>
    public async Task<Menu> GetMenuByDayAsync(Guid dayId)
    {
        var day = await _dbContext.Days
            .Include(d => d.Menu)
            .ThenInclude(m => m.MenuItems)
            .ThenInclude(mi => mi.Item)
            .FirstOrDefaultAsync(d => d.Id == dayId);

        return day?.Menu != null
            ? MenuConverter.ConvertDBToCore(day.Menu)
            : null;
    }

    /// <summary>
    /// Получить количество предмета в меню
    /// </summary>
    /// <returns>Количество предмета в меню</returns>
    public async Task<int> GetAmountItemAsync(Guid menuId, Guid itemId)
    {
        var menuItem = await _dbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.MenuId == menuId && mi.ItemId == itemId);

        if (menuItem == null)
            return 0;

        return menuItem.Amount;
    }

    /// <summary>
    /// Создать новое меню
    /// </summary>
    /// <param name="menu">Модель меню для создания</param>
    public async Task InsertMenuAsync(Menu menu)
    {
        var menuEntity = MenuConverter.ConvertCoreToDB(menu);
        await _dbContext.Menu.AddAsync(menuEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Добавить предмет в меню
    /// </summary>
    /// <param name="menuId">Идентификатор меню</param>
    /// <param name="itemId">Идентификатор добавляемого предмета</param>
    public async Task InsertItemAsync(Guid menuId, Guid itemId, int amount)
    {
        var menuItem = new MenuItemsDBModel(menuId, itemId, amount);
        await _dbContext.MenuItems.AddAsync(menuItem);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить существующее меню
    /// </summary>
    /// <param name="updateMenu">Модель с обновленными данными меню</param>
    public async Task UpdateMenuAsync(Menu updateMenu)
    {
        var existingMenu = await _dbContext.Menu
            .FirstOrDefaultAsync(m => m.Id == updateMenu.Id);

        if (existingMenu == null) return;

        existingMenu.Name = updateMenu.Name;
        existingMenu.Cost = updateMenu.Cost;

        _dbContext.Menu.Update(existingMenu);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить меню по идентификатору
    /// </summary>
    /// <param name="menuId">Идентификатор удаляемого меню</param>
    public async Task DeleteMenuAsync(Guid menuId)
    {
        var menuToDelete = await _dbContext.Menu
            .FirstOrDefaultAsync(m => m.Id == menuId);

        if (menuToDelete == null) return;

        _dbContext.Menu.Remove(menuToDelete);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить предмет из меню
    /// </summary>
    /// <param name="menuId">Идентификатор меню</param>
    /// <param name="itemId">Идентификатор удаляемого предмета</param>
    public async Task DeleteItemAsync(Guid menuId, Guid itemId)
    {
        var menuItem = await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.MenuId == menuId && mi.ItemId == itemId);

        if (menuItem == null) return;

        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync();
    }
}