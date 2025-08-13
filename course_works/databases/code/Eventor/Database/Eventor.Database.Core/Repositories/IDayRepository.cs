using Eventor.Common.Core;
using Day = Eventor.Common.Core.Day;
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
    Task<List<Day>> GetAllDaysAsync();

    /// <summary>
    /// Получить все дни мероприятия
    /// </summary>
    /// <returns>Список всех дней мероприятия</returns>
    Task<List<Day>> GetAllDaysByEventAsync(Guid eventId);

    /// <summary>
    /// Получить все дни участника
    /// </summary>
    Task<List<Day>> GetAllDaysByPersonAsync(Guid personId);

    /// <summary>
    /// Получить день по его идентификатору
    /// </summary>
    /// <returns>День</returns>
    Task<Day> GetDayByIdAsync(Guid dayId);

    /// <summary>
    /// Создать день
    /// </summary>
    /// <returns></returns>
    Task InsertDayAsync(Day day);

    /// <summary>
    /// Добавить участника на день 
    /// </summary>
    /// <returns></returns>
    Task InsertPersonToDayAsync(Guid personId, Guid dayId);

    /// <summary>
    /// Обновить день
    /// </summary>
    /// <returns></returns>
    Task UpdateDayAsync(Day updateDay);

    /// <summary>
    /// Удалить день
    /// </summary>
    /// <returns></returns>
    Task DeleteDayAsync(Guid dayId);

    /// <summary>
    /// Удалить участника с дня
    /// </summary>
    /// <returns></returns>
    Task DeletePersonFromDayAsync(Guid personId, Guid dayId);
}

