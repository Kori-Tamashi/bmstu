using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория меню
/// </summary>
public interface IMenuRepository
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
    /// Получить количество предмета в меню
    /// </summary>
    /// <returns>Количество предмета в меню</returns>
    Task<int> GetAmountItemAsync(Guid menu_id, Guid itemId);

    /// <summary>
    /// Создать меню
    /// </summary>
    /// <returns></returns>
    Task InsertMenuAsync(Menu menu);

    /// <summary>
    /// Добавить предмет в меню
    /// </summary>
    /// <returns></returns>
    Task InsertItemAsync(Guid menuId, Guid itemId, int amount);

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

