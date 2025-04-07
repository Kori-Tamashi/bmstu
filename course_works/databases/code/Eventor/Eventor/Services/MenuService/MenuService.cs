using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Database.Repositories;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eventor.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<MenuService> _logger;

    public MenuService(IMenuRepository menuRepository, ILogger<MenuService> logger)
    {
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Menu>> GetAllMenuAsync()
    {
        try
        {
            return await _menuRepository.GetAllMenuAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving menus");
            throw new MenuServiceException("Failed to retrieve menus", ex);
        }
    }

    public async Task<Menu> GetMenuByIdAsync(Guid menuId)
    {
        try
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
            {
                throw new MenuNotFoundException($"Menu {menuId} not found");
            }
            return menu;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving menu {MenuId}", menuId);
            throw new MenuServiceException($"Failed to retrieve menu {menuId}", ex);
        }
    }

    public async Task<Menu> GetMenuByDayAsync(Guid dayId)
    {
        try
        {
            var menu = await _menuRepository.GetMenuByDayAsync(dayId);
            if (menu == null)
            {
                throw new MenuNotFoundException($"Menu for day {dayId} not found");
            }
            return menu;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving menu for day {DayId}", dayId);
            throw new MenuServiceException($"Failed to retrieve menu for day {dayId}", ex);
        }
    }

    public async Task AddMenuAsync(Menu menu)
    {
        try
        {
            await _menuRepository.InsertMenuAsync(menu);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating menu");
            throw new MenuCreateException("Failed to create menu", ex);
        }
    }

    public async Task AddItemAsync(Guid menuId, Guid itemId, int amount)
    {
        try
        {
            await _menuRepository.InsertItemAsync(menuId, itemId, amount);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error adding item {ItemId} to menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to add item {itemId} to menu {menuId}", ex);
        }
    }

    public async Task UpdateMenuAsync(Menu updateMenu)
    {
        try
        {
            var existingMenu = await _menuRepository.GetMenuByIdAsync(updateMenu.Id);
            if (existingMenu == null)
            {
                throw new MenuNotFoundException($"Menu {updateMenu.Id} not found");
            }

            await _menuRepository.UpdateMenuAsync(updateMenu);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating menu {MenuId}", updateMenu.Id);
            throw new MenuUpdateException($"Failed to update menu {updateMenu.Id}", ex);
        }
    }

    public async Task DeleteMenuAsync(Guid menuId)
    {
        try
        {
            var existingMenu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (existingMenu == null)
            {
                throw new MenuNotFoundException($"Menu {menuId} not found");
            }

            await _menuRepository.DeleteMenuAsync(menuId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting menu {MenuId}", menuId);
            throw new MenuDeleteException($"Failed to delete menu {menuId}", ex);
        }
    }

    public async Task DeleteItemAsync(Guid menuId, Guid itemId)
    {
        try
        {
            await _menuRepository.DeleteItemAsync(menuId, itemId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting item {ItemId} from menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to delete item {itemId} from menu {menuId}", ex);
        }
    }
}