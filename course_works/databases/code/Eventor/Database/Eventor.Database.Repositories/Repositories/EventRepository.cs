using Eventor.Common.Converter;
using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Microsoft.EntityFrameworkCore;
namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий мероприятий
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
    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _dbContext.Events
            .OrderBy(e => e.Date)
            .Select(e => EventConverter.ConvertDBToCore(e))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить все мероприятия пользователя
    /// </summary>
    /// <returns>Список всех мероприятий пользователя</returns>
    public async Task<List<Event>> GetAllEventsByUserAsync(Guid userId)
    {
        return await _dbContext.UsersEvents
                .Where(ue => ue.UserId == userId)
                .Include(ue => ue.Event)
                    .ThenInclude(e => e.Location)
                .Include(ue => ue.Event)
                    .ThenInclude(e => e.EventDays)
                        .ThenInclude(ed => ed.Day)
                .OrderBy(ue => ue.Event.Date)
                .Select(ue => EventConverter.ConvertDBToCore(ue.Event))
                .AsNoTracking()
                .ToListAsync();
    }

    /// <summary>
    /// Получить мероприятие по его идентификатору
    /// </summary>
    /// <returns>Мероприятие</returns>
    public async Task<Event> GetEventByIdAsync(Guid eventId)
    {
        var eventEntity = await _dbContext.Events
                .Include(e => e.Location)
                .Include(e => e.EventDays)
                    .ThenInclude(ed => ed.Day)
                .Include(e => e.UserEvents)
                    .ThenInclude(ue => ue.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventId);

        return EventConverter.ConvertDBToCore(eventEntity);
    }

    /// <summary>
    /// Создать мероприятие
    /// </summary>
    /// <returns></returns>
    public async Task InsertEventAsync(Event _event)
    {
        var eventEntity = EventConverter.ConvertCoreToDB(_event);
        await _dbContext.Events.AddAsync(eventEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить мероприятие
    /// </summary>
    /// <returns></returns>
    public async Task UpdateEventAsync(Event updateEvent)
    {
        var existingEvent = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == updateEvent.Id);

        existingEvent.Name = updateEvent.Name;
        existingEvent.Description = updateEvent.Description;
        existingEvent.Date = updateEvent.Date;
        existingEvent.LocationId = updateEvent.LocationId;
        existingEvent.PersonCount = updateEvent.PersonCount;
        existingEvent.DaysCount = updateEvent.DaysCount;
        existingEvent.Percent = updateEvent.Percent;
        existingEvent.Rating = updateEvent.Rating;

        _dbContext.Events.Update(existingEvent);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить мероприятие
    /// </summary>
    /// <returns></returns>
    public async Task DeleteEventAsync(Guid eventId)
    {
        var eventToDelete = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

        _dbContext.Events.Remove(eventToDelete);
        await _dbContext.SaveChangesAsync();
    }
}