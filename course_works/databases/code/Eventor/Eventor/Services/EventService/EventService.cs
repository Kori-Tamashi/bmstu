using Eventor.Common.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Eventor.Database.Core;

namespace Eventor.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<EventService> _logger;

    public EventService(IEventRepository eventRepository, ILogger<EventService> logger)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        try
        {
           return await _eventRepository.GetAllEventsAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            throw new EventServiceException("Failed to retrieve events", ex);
        }
    }

    public async Task<List<Event>> GetAllEventsByUserAsync(Guid userId)
    {
        try
        {
            return await _eventRepository.GetAllEventsByUserAsync(userId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve events for user {userId}", ex);
        }
    }

    public async Task<Event> GetEventByIdAsync(Guid eventId)
    {
        try
        {
            return await _eventRepository.GetEventByIdAsync(eventId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Event {EventId} not found", eventId);
            throw new EventNotFoundException($"Event {eventId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving event {EventId}", eventId);
            throw new EventServiceException($"Failed to retrieve event {eventId}", ex);
        }
    }

    public async Task AddEventAsync(Event @event)
    {
        try
        {
            await _eventRepository.InsertEventAsync(@event);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating event");
            throw new EventCreateException("Failed to create event", ex);
        }
    }

    public async Task UpdateEventAsync(Event updateEvent)
    {
        try
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(updateEvent.Id);
            await _eventRepository.UpdateEventAsync(existingEvent);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Event {EventId} not found for update", updateEvent.Id);
            throw new EventNotFoundException($"Event {updateEvent.Id} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", updateEvent.Id);
            throw new EventUpdateException($"Failed to update event {updateEvent.Id}", ex);
        }
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        try
        {
            await _eventRepository.DeleteEventAsync(eventId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Event {EventId} not found for deletion", eventId);
            throw new EventNotFoundException($"Event {eventId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", eventId);
            throw new EventDeleteException($"Failed to delete event {eventId}", ex);
        }
    }
}