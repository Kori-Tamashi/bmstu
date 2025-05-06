using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.ApplicationServices;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _logger.LogError(ex, "Database error while retrieving events");
            throw new EventServiceException("Failed to retrieve events due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving events");
            throw new EventServiceException("Failed to retrieve events due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving events");
            throw new EventServiceException("Failed to retrieve events", ex);
        }
    }

    public async Task<List<Event>> GetAllEventsByUserAsync(Guid userId)
    {
        try
        {
            return await _eventRepository.GetAllEventsByUserAsync(userId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "User {UserId} not found", userId);
            throw new EventNotFoundException($"User {userId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve events for user {userId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve events for user {userId} due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve events for user {userId}", ex);
        }
    }

    public async Task<List<Event>> GetAllOrganizedEventsByUserAsync(Guid userId)
    {
        try
        {
            return await _eventRepository.GetAllOrganizedEventsByUserAsync(userId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid user ID {UserId} for organized events", userId);
            throw new EventNotFoundException($"User {userId} is invalid", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving organized events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve organized events for user {userId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving organized events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve organized events for user {userId} due to data issues", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching organized events for user {UserId}", userId);
            throw new EventServiceException($"Failed to retrieve organized events for user {userId}", ex);
        }
    }

    public async Task<Event> GetEventByIdAsync(Guid eventId)
    {
        try
        {
            return await _eventRepository.GetEventByIdAsync(eventId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Event {EventId} not found", eventId);
            throw new EventNotFoundException($"Event {eventId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving event {EventId}", eventId);
            throw new EventServiceException($"Failed to retrieve event {eventId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving event {EventId}", eventId);
            throw new EventServiceException($"Failed to retrieve event {eventId} due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving event {EventId}", eventId);
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
            _logger.LogError(ex, "Database error while creating event");
            throw new EventCreateException("Failed to create event due to database constraints violation", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data conflict while creating event");
            throw new EventCreateException("Failed to create event due to data conflict", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating event");
            throw new EventCreateException("Failed to create event", ex);
        }
    }

    public async Task AddUserForEventAsync(Guid userId, Guid eventId)
    {
        try
        {
            await _eventRepository.InsertUserForEventAsync(userId, eventId);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            _logger.LogWarning("Пользователь {UserId} уже привязан к мероприятию {EventId}", userId, eventId);
            throw new EventServiceException($"Пользователь {userId} уже участвует в мероприятии", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка базы данных при добавлении пользователя {UserId} на мероприятие {EventId}", userId, eventId);
            throw new EventServiceException($"Не удалось добавить пользователя {userId} на мероприятие", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при добавлении пользователя {UserId} на мероприятие {EventId}", userId, eventId);
            throw new EventServiceException("Внутренняя ошибка сервиса", ex);
        }
    }

    public async Task UpdateEventAsync(Event updateEvent)
    {
        try
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(updateEvent.Id);
            await _eventRepository.UpdateEventAsync(updateEvent);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Event {EventId} not found for update", updateEvent.Id);
            throw new EventNotFoundException($"Event {updateEvent.Id} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating event {EventId}", updateEvent.Id);
            throw new EventUpdateException($"Failed to update event {updateEvent.Id} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while updating event {EventId}", updateEvent.Id);
            throw new EventUpdateException($"Concurrency conflict while updating event {updateEvent.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating event {EventId}", updateEvent.Id);
            throw new EventUpdateException($"Failed to update event {updateEvent.Id}", ex);
        }
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        try
        {
            await _eventRepository.DeleteEventAsync(eventId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Event {EventId} not found for deletion", eventId);
            throw new EventNotFoundException($"Event {eventId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting event {EventId}", eventId);
            throw new EventDeleteException($"Failed to delete event {eventId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while deleting event {EventId}", eventId);
            throw new EventDeleteException($"Concurrency conflict while deleting event {eventId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting event {EventId}", eventId);
            throw new EventDeleteException($"Failed to delete event {eventId}", ex);
        }
    }

    public async Task DeleteUserFromEventAsync(Guid userId, Guid eventId)
    {
        try
        {
            await _eventRepository.DeleteUserFromEventAsync(userId, eventId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Ошибка при удалении пользователя {UserId} с мероприятия {EventId}", userId, eventId);
            throw new EventServiceException($"Пользователь {userId} не найден в мероприятии", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при удалении пользователя {UserId} с мероприятия {EventId}", userId, eventId);
            throw new EventServiceException("Внутренняя ошибка сервиса", ex);
        }
    }
}