using Eventor.Common.Converter;
using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventor.Database.Repositories;

public class EventRepository : BaseRepository, IEventRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<EventRepository> _logger;

    public EventRepository(
        EventorDBContext dbContext,
        ILogger<EventRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        try
        {
            return await _dbContext.Events
                .OrderBy(e => e.Date)
                .Select(e => EventConverter.ConvertDBToCore(e))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка мероприятий");
            throw new InvalidOperationException("Не удалось получить список мероприятий", ex);
        }
    }

    public async Task<List<Event>> GetAllEventsByUserAsync(Guid userId)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения мероприятий пользователя {UserId}", userId);
            throw new InvalidOperationException($"Не удалось получить мероприятия пользователя {userId}", ex);
        }
    }

    public async Task<Event> GetEventByIdAsync(Guid eventId)
    {
        try
        {
            var eventEntity = await _dbContext.Events
                .Include(e => e.Location)
                .Include(e => e.EventDays)
                    .ThenInclude(ed => ed.Day)
                .Include(e => e.UserEvents)
                    .ThenInclude(ue => ue.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventEntity == null)
            {
                throw new ArgumentException($"Мероприятие {eventId} не найдено");
            }

            return EventConverter.ConvertDBToCore(eventEntity);
        }
        catch (DbUpdateException ex) 
        {
            _logger.LogError(ex, "Ошибка получения мероприятия {EventId}", eventId);
            throw new InvalidOperationException($"Не удалось получить мероприятие {eventId}", ex);
        }
    }

    public async Task<Event> GetEventByDayAsync(Guid dayId)
    {
        try
        {
            var eventDay = await _dbContext.EventsDays
                .Include(ed => ed.Event)
                .FirstOrDefaultAsync(ed => ed.DayId == dayId);

            if (eventDay?.Event == null)
                return null;

            return await GetEventByIdAsync(eventDay.Event.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка поиска мероприятия по дню {DayId}", dayId);
            throw new InvalidOperationException($"Не удалось найти мероприятие для дня {dayId}", ex);
        }
    }

    public async Task InsertEventAsync(Event _event)
    {
        try
        {
            var eventEntity = EventConverter.ConvertCoreToDB(_event);
            await _dbContext.Events.AddAsync(eventEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания мероприятия");
            throw new InvalidOperationException("Не удалось создать мероприятие", ex);
        }
    }

    public async Task UpdateEventAsync(Event updateEvent)
    {
        try
        {
            var existingEvent = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == updateEvent.Id);

            if (existingEvent == null)
            {
                throw new ArgumentException($"Мероприятие {updateEvent.Id} не найдено");
            }

            existingEvent.Name = updateEvent.Name;
            existingEvent.Description = updateEvent.Description;
            existingEvent.Date = updateEvent.Date;
            existingEvent.LocationId = updateEvent.LocationId;
            existingEvent.PersonCount = updateEvent.PersonCount;
            existingEvent.DaysCount = updateEvent.DaysCount;
            existingEvent.Percent = updateEvent.Percent;
            existingEvent.Rating = updateEvent.Rating;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления мероприятия {EventId}", updateEvent.Id);
            throw new InvalidOperationException($"Не удалось обновить мероприятие {updateEvent.Id}", ex);
        }
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        try
        {
            var eventToDelete = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventToDelete == null)
            {
                throw new ArgumentException($"Мероприятие {eventId} не найдено");
            }

            _dbContext.Events.Remove(eventToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления мероприятия {EventId}", eventId);
            throw new InvalidOperationException($"Не удалось удалить мероприятие {eventId}", ex);
        }
    }
}