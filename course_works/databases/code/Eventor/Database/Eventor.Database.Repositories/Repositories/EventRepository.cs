using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Core;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий мероприятия
/// </summary>
public class EventRepository : BaseRepository, IEventRepository
{
    private readonly EventorDBContext _dbContext;

    public EventRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns>Список всех мероприятий</returns>
    public Task<List<Event>> GetAllEventsAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить все мероприятия пользователя
    /// </summary>
    /// <returns>Список всех мероприятий пользователя</returns>
    public Task<List<Event>> GetAllEventsByUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить мероприятие по его идентификатору
    /// </summary>
    /// <returns>Мероприятие</returns>
    public Task<Event> GetEventByIdAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Создать мероприятие
    /// </summary>
    /// <returns></returns>
    public Task InsertEventAsync(Event _event)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Обновить мероприятие
    /// </summary>
    /// <returns></returns>
    public Task UpdateEventAsync(Event updateEvent)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удалить мероприятие
    /// </summary>
    /// <returns></returns>
    public Task DeleteEventAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }
}