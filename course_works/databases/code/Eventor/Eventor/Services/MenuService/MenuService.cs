using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            _logger.LogError(ex, "Database error while retrieving menus");
            throw new MenuServiceException("Failed to retrieve menus due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving menus");
            throw new MenuServiceException("Failed to retrieve menus due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving menus");
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
                _logger.LogWarning("Menu {MenuId} not found", menuId);
                throw new MenuNotFoundException($"Menu {menuId} not found");
            }
            return menu;
        }
        catch (MenuNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid menu ID: {MenuId}", menuId);
            throw new MenuServiceException($"Invalid menu ID: {menuId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving menu {MenuId}", menuId);
            throw new MenuServiceException($"Failed to retrieve menu {menuId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving menu {MenuId}", menuId);
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
                _logger.LogWarning("Menu for day {DayId} not found", dayId);
                throw new MenuNotFoundException($"Menu for day {dayId} not found");
            }
            return menu;
        }
        catch (MenuNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid day ID: {DayId}", dayId);
            throw new MenuServiceException($"Invalid day ID: {dayId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving menu for day {DayId}", dayId);
            throw new MenuServiceException($"Failed to retrieve menu for day {dayId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving menu for day {DayId}", dayId);
            throw new MenuServiceException($"Failed to retrieve menu for day {dayId}", ex);
        }
    }

    public async Task<int> GetAmountForItemAsync(Guid menuId, Guid itemId)
    {
        try
        {
            return await _menuRepository.GetAmountItemAsync(menuId, itemId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid item ID: {ItemID}", itemId);
            throw new MenuServiceException($"Invalid item ID: {itemId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving item amount for menu {MenuId}", menuId);
            throw new MenuServiceException($"Failed to retrieve item amount for menu {menuId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving item amount for menu {MenuId}", menuId);
            throw new MenuServiceException($"Failed to retrieve item amount for menu {menuId}", ex);
        }
    }

    public async Task AddMenuAsync(Menu menu)
    {
        try
        {
            await _menuRepository.InsertMenuAsync(menu);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error while creating menu");
            throw new MenuCreateException($"Failed to create menu: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while creating menu");
            throw new MenuCreateException("Failed to create menu due to database constraints", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data conflict while creating menu");
            throw new MenuCreateException("Failed to create menu due to data conflict", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating menu");
            throw new MenuCreateException("Failed to create menu", ex);
        }
    }

    public async Task AddItemAsync(Guid menuId, Guid itemId, int amount)
    {
        try
        {
            await _menuRepository.InsertItemAsync(menuId, itemId, amount);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error while adding item {ItemId} to menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to add item: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding item {ItemId} to menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to add item {itemId} to menu {menuId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while adding item {ItemId} to menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Concurrency conflict while adding item {itemId} to menu {menuId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding item {ItemId} to menu {MenuId}", itemId, menuId);
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
                _logger.LogWarning("Menu {MenuId} not found for update", updateMenu.Id);
                throw new MenuNotFoundException($"Menu {updateMenu.Id} not found");
            }

            await _menuRepository.UpdateMenuAsync(updateMenu);
        }
        catch (MenuNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid data while updating menu {MenuId}", updateMenu.Id);
            throw new MenuUpdateException($"Failed to update menu {updateMenu.Id}: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating menu {MenuId}", updateMenu.Id);
            throw new MenuUpdateException($"Failed to update menu {updateMenu.Id} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while updating menu {MenuId}", updateMenu.Id);
            throw new MenuUpdateException($"Concurrency conflict while updating menu {updateMenu.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating menu {MenuId}", updateMenu.Id);
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
                _logger.LogWarning("Menu {MenuId} not found for deletion", menuId);
                throw new MenuNotFoundException($"Menu {menuId} not found");
            }

            await _menuRepository.DeleteMenuAsync(menuId);
        }
        catch (MenuNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid menu ID: {MenuId}", menuId);
            throw new MenuDeleteException($"Invalid menu ID: {menuId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting menu {MenuId}", menuId);
            throw new MenuDeleteException($"Failed to delete menu {menuId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while deleting menu {MenuId}", menuId);
            throw new MenuDeleteException($"Concurrency conflict while deleting menu {menuId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting menu {MenuId}", menuId);
            throw new MenuDeleteException($"Failed to delete menu {menuId}", ex);
        }
    }

    public async Task DeleteItemAsync(Guid menuId, Guid itemId)
    {
        try
        {
            await _menuRepository.DeleteItemAsync(menuId, itemId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid data while deleting item {ItemId} from menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to delete item: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting item {ItemId} from menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to delete item {itemId} from menu {menuId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while deleting item {ItemId} from menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Concurrency conflict while deleting item {itemId} from menu {menuId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting item {ItemId} from menu {MenuId}", itemId, menuId);
            throw new MenuUpdateException($"Failed to delete item {itemId} from menu {menuId}", ex);
        }
    }
}