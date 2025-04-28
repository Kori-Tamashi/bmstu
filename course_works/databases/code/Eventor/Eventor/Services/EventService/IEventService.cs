using Eventor.Common.Core;

namespace Eventor.Services;

/// <summary>
/// Интерфейс сервиса мероприятия
/// </summary>
public interface IEventService
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
    /// Создать мероприятие
    /// </summary>
    /// <returns></returns>
    Task AddEventAsync(Event _event);

    /// <summary>
    /// Добавить пользователя на мероприятие
    /// </summary>
    /// <returns></returns>
    Task AddUserForEventAsync(Guid userId, Guid eventId);

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
