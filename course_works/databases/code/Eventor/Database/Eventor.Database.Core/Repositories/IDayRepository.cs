using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория дня мероприятия
/// </summary>
public interface IDayRepository
{
    /// <summary>
    /// Получить все дни
    /// </summary>
    /// <returns>Список всех дней</returns>
    Task<List<Common.Core.Day>> GetAllDaysAsync();

    /// <summary>
    /// Получить все дни мероприятия
    /// </summary>
    /// <returns>Список всех дней мероприятия</returns>
    Task<List<Common.Core.Day>> GetAllDaysByEventAsync(Guid eventId);

    /// <summary>
    /// Получить день по его идентификатору
    /// </summary>
    /// <returns>День</returns>
    Task<Common.Core.Day> GetDayByIdAsync(Guid eventId);

    /// <summary>
    /// Создать день
    /// </summary>
    /// <returns></returns>
    Task InsertDayAsync(Common.Core.Day day);

    /// <summary>
    /// Обновить день
    /// </summary>
    /// <returns></returns>
    Task UpdateDayAsync(Common.Core.Day updateDay);

    /// <summary>
    /// Удалить день
    /// </summary>
    /// <returns></returns>
    Task DeleteDayAsync(Guid dayId);
}

