using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория предмета
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Получить все предметы
    /// </summary>
    /// <returns>Список всех предметов</returns>
    Task<List<Item>> GetAllItemsAsync();

    /// <summary>
    /// Получить все предметы меню
    /// </summary>
    /// <returns>Предмет</returns>
    Task<List<Item>> GetAllItemsByMenuAsync(Guid menuId);

    /// <summary>
    /// Получить предмет по его идентификатору
    /// </summary>
    /// <returns>Предмет</returns>
    Task<Item> GetItemByIdAsync(Guid itemId);

    /// <summary>
    /// Создать предмет
    /// </summary>
    /// <returns></returns>
    Task InsertItemAsync(Item updateItem);

    /// <summary>
    /// Обновить предмет
    /// </summary>
    /// <returns></returns>
    Task UpdateItemAsync(Item item);

    /// <summary>
    /// Удалить предмет
    /// </summary>
    /// <returns></returns>
    Task DeleteItemAsync(Guid itemId);
}

