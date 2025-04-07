using Eventor.Common.Core;
namespace Eventor.Services;

/// <summary>
/// Интерфейс сервиса меню
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Получить все меню
    /// </summary>
    /// <returns>Список всех меню</returns>
    Task<List<Menu>> GetAllMenuAsync();

    /// <summary>
    /// Получить меню по его идентификатору
    /// </summary>
    /// <returns>Меню</returns>
    Task<Menu> GetMenuByIdAsync(Guid eventId);

    /// <summary>
    /// Получить меню конкретного дня мероприятия
    /// </summary>
    /// <returns>Меню</returns>
    Task<Menu> GetMenuByDayAsync(Guid dayId);

    /// <summary>
    /// Создать меню
    /// </summary>
    /// <returns></returns>
    Task AddMenuAsync(Menu menu);

    /// <summary>
    /// Добавить предмет в меню
    /// </summary>
    /// <returns></returns>
    Task AddItemAsync(Guid menuId, Guid itemId, int amount);

    /// <summary>
    /// Обновить меню
    /// </summary>
    /// <returns></returns>
    Task UpdateMenuAsync(Menu updateMenu);

    /// <summary>
    /// Удалить меню
    /// </summary>
    /// <returns></returns>
    Task DeleteMenuAsync(Guid menuId);

    /// <summary>
    /// Удалить предмет из меню
    /// </summary>
    /// <returns></returns>
    Task DeleteItemAsync(Guid menuId, Guid itemId);
}
