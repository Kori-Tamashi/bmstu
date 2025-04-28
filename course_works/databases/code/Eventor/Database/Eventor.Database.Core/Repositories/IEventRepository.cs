using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория мероприятия
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns>Список всех мероприятий</returns>
    Task<List<Event>> GetAllEventsAsync();

    /// <summary>
    /// Получить все мероприятия пользователя
    /// </summary>
    /// <returns>Список всех мероприятий пользователя</returns>
    Task<List<Event>> GetAllEventsByUserAsync(Guid userId);

    /// <summary>
    /// Получить мероприятие по его идентификатору
    /// </summary>
    /// <returns>Мероприятие</returns>
    Task<Event> GetEventByIdAsync(Guid eventId);

    /// <summary>
    /// Получить мероприятие по его дню
    /// </summary>
    /// <returns>Мероприятие</returns>
    Task<Event> GetEventByDayAsync(Guid dayId);

    /// <summary>
    /// Создать мероприятие
    /// </summary>
    /// <returns></returns>
    Task InsertEventAsync(Event _event);

    /// <summary>
    /// Добавить пользователя на мероприятие
    /// </summary>
    /// <returns></returns>
    Task InsertUserForEventAsync(Guid userId, Guid eventId);

    /// <summary>
    /// Обновить мероприятие
    /// </summary>
    /// <returns></returns>
    Task UpdateEventAsync(Event updateEvent);

    /// <summary>
    /// Удалить мероприятие
    /// </summary>
    /// <returns></returns>
    Task DeleteEventAsync(Guid eventId);

    /// <summary>
    /// Удалить пользователя с мероприятия
    /// </summary>
    /// <returns></returns>
    Task DeleteUserFromEventAsync(Guid userId, Guid eventId);
}
