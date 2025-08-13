using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventor.Database.Models;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с меню мероприятий
/// </summary>
public class MenuRepository : BaseRepository, IMenuRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<MenuRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория меню
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public MenuRepository(
        EventorDBContext dbContext,
        ILogger<MenuRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все меню
    /// </summary>
    public async Task<List<Menu>> GetAllMenuAsync()
    {
        try
        {
            return await _dbContext.Menu
                .Include(m => m.MenuItems)
                .ThenInclude(mi => mi.Item)
                .Select(m => MenuConverter.ConvertDBToCore(m))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка меню");
            throw new InvalidOperationException("Не удалось получить список меню", ex);
        }
    }

    /// <summary>
    /// Получить меню по идентификатору
    /// </summary>
    public async Task<Menu> GetMenuByIdAsync(Guid menuId)
    {
        try
        {
            var menuEntity = await _dbContext.Menu
                .Include(m => m.MenuItems)
                .ThenInclude(mi => mi.Item)
                .FirstOrDefaultAsync(m => m.Id == menuId);

            return menuEntity != null
                ? MenuConverter.ConvertDBToCore(menuEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения меню {MenuId}", menuId);
            throw new InvalidOperationException($"Не удалось получить меню {menuId}", ex);
        }
    }

    /// <summary>
    /// Получить меню конкретного дня мероприятия
    /// </summary>
    public async Task<Menu> GetMenuByDayAsync(Guid dayId)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения меню для дня {DayId}", dayId);
            throw new InvalidOperationException($"Не удалось получить меню для дня {dayId}", ex);
        }
    }

    /// <summary>
    /// Получить количество предмета в меню
    /// </summary>
    public async Task<int> GetAmountItemAsync(Guid menuId, Guid itemId)
    {
        try
        {
            var menuItem = await _dbContext.MenuItems
                .AsNoTracking()
                .FirstOrDefaultAsync(mi => mi.MenuId == menuId && mi.ItemId == itemId);

            return menuItem?.Amount ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения количества предмета {ItemId} в меню {MenuId}", itemId, menuId);
            throw new InvalidOperationException(
                $"Не удалось получить количество предмета {itemId} в меню {menuId}", ex);
        }
    }

    /// <summary>
    /// Создать новое меню
    /// </summary>
    public async Task InsertMenuAsync(Menu menu)
    {
        try
        {
            var menuEntity = MenuConverter.ConvertCoreToDB(menu);
            await _dbContext.Menu.AddAsync(menuEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания меню");
            throw new InvalidOperationException("Не удалось создать меню", ex);
        }
    }

    /// <summary>
    /// Добавить предмет в меню
    /// </summary>
    public async Task InsertItemAsync(Guid menuId, Guid itemId, int amount)
    {
        try
        {
            // Проверка существования меню и предмета
            var menuExists = await _dbContext.Menu.AnyAsync(m => m.Id == menuId);
            var itemExists = await _dbContext.Items.AnyAsync(i => i.Id == itemId);

            if (!menuExists || !itemExists)
            {
                var errorMessage = !menuExists
                    ? $"Меню {menuId} не найдено"
                    : $"Предмет {itemId} не найден";

                _logger.LogWarning(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            var menuItem = new MenuItemsDBModel(menuId, itemId, amount);
            await _dbContext.MenuItems.AddAsync(menuItem);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка добавления предмета {ItemId} в меню {MenuId}", itemId, menuId);
            throw new InvalidOperationException(
                $"Не удалось добавить предмет {itemId} в меню {menuId}", ex);
        }
    }

    /// <summary>
    /// Обновить существующее меню
    /// </summary>
    public async Task UpdateMenuAsync(Menu updateMenu)
    {
        try
        {
            var existingMenu = await _dbContext.Menu
                .FirstOrDefaultAsync(m => m.Id == updateMenu.Id);

            if (existingMenu == null)
            {
                _logger.LogWarning("Меню {MenuId} не найдено", updateMenu.Id);
                return;
            }

            existingMenu.Name = updateMenu.Name;
            existingMenu.Cost = updateMenu.Cost;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления меню {MenuId}", updateMenu.Id);
            throw new InvalidOperationException($"Не удалось обновить меню {updateMenu.Id}", ex);
        }
    }

    /// <summary>
    /// Удалить меню по идентификатору
    /// </summary>
    public async Task DeleteMenuAsync(Guid menuId)
    {
        try
        {
            var menuToDelete = await _dbContext.Menu
                .FirstOrDefaultAsync(m => m.Id == menuId);

            if (menuToDelete == null)
            {
                _logger.LogWarning("Меню {MenuId} не найдено", menuId);
                return;
            }

            _dbContext.Menu.Remove(menuToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления меню {MenuId}", menuId);
            throw new InvalidOperationException($"Не удалось удалить меню {menuId}", ex);
        }
    }

    /// <summary>
    /// Удалить предмет из меню
    /// </summary>
    public async Task DeleteItemAsync(Guid menuId, Guid itemId)
    {
        try
        {
            var menuItem = await _dbContext.MenuItems
                .FirstOrDefaultAsync(mi => mi.MenuId == menuId && mi.ItemId == itemId);

            if (menuItem == null)
            {
                _logger.LogWarning("Предмет {ItemId} не найден в меню {MenuId}", itemId, menuId);
                return;
            }

            _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления предмета {ItemId} из меню {MenuId}", itemId, menuId);
            throw new InvalidOperationException(
                $"Не удалось удалить предмет {itemId} из меню {menuId}", ex);
        }
    }
}