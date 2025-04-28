using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Services;

/// <summary>
/// Интерфейс сервиса дня мероприятия
/// </summary>
public interface IDayService
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
    /// Получить все дни участника
    /// </summary>
    Task<List<Common.Core.Day>> GetAllDaysByPersonAsync(Guid personId);

    /// <summary>
    /// Получить день по его идентификатору
    /// </summary>
    /// <returns>День</returns>
    Task<Common.Core.Day> GetDayByIdAsync(Guid eventId);

    /// <summary>
    /// Создать день
    /// </summary>
    /// <returns></returns>
    Task AddDayAsync(Common.Core.Day day);

    /// <summary>
    /// Добавить участника на день
    /// </summary>
    /// <returns></returns>
    Task AddPersonToDayAsync(Guid personId, Guid dayId);

    /// <summary>
    /// Добавить участника на день
    /// </summary>
    /// <returns></returns>
    Task AddPersonToDayAsync(Guid eventId, int daySequenceNumber, Guid personId);

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

    /// <summary>
    /// Удалить участника с дня
    /// </summary>
    /// <returns></returns>
    Task DeletePersonFromDayAsync(Guid personId, Guid dayId);
}
