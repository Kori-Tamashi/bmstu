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
    /// Создать мероприятие
    /// </summary>
    /// <returns></returns>
    Task InsertEventAsync(Event _event);

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
}
