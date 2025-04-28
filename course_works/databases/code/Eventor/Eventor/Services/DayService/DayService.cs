using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Database.Repositories;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Eventor.Database.Core;
using Day = Eventor.Common.Core.Day;
using Npgsql;

namespace Eventor.Services;

public class DayService : IDayService
{
    private readonly IDayRepository _dayRepository;
    private readonly ILogger<DayService> _logger;

    public DayService(IDayRepository dayRepository, ILogger<DayService> logger)
    {
        _dayRepository = dayRepository ?? throw new ArgumentNullException(nameof(dayRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Day>> GetAllDaysAsync()
    {
        try
        {
            return await _dayRepository.GetAllDaysAsync();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Database error while retrieving days");
            throw new DayServiceException("Failed to retrieve days due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving days");
            throw new DayServiceException("Failed to retrieve days", ex);
        }
    }

    public async Task<List<Day>> GetAllDaysByEventAsync(Guid eventId)
    {
        try
        {
            return await _dayRepository.GetAllDaysByEventAsync(eventId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Database error while retrieving days for event {EventId}", eventId);
            throw new DayServiceException($"Failed to retrieve days for event {eventId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving days for event {EventId}", eventId);
            throw new DayServiceException($"Failed to retrieve days for event {eventId}", ex);
        }
    }

    public async Task<List<Day>> GetAllDaysByPersonAsync(Guid personId)
    {
        try
        {
            return await _dayRepository.GetAllDaysByPersonAsync(personId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Database error while retrieving days for person {personId}", personId);
            throw new DayServiceException($"Failed to retrieve days for person {personId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving days for person {EventId}", personId);
            throw new DayServiceException($"Failed to retrieve days for person {personId}", ex);
        }
    }

    public async Task<Day> GetDayByIdAsync(Guid dayId)
    {
        try
        {
            return await _dayRepository.GetDayByIdAsync(dayId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Day {DayId} not found", dayId);
            throw new DayNotFoundException($"Day {dayId} not found", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Database error while retrieving day {DayId}", dayId);
            throw new DayServiceException($"Failed to retrieve day {dayId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving day {DayId}", dayId);
            throw new DayServiceException($"Failed to retrieve day {dayId}", ex);
        }
    }

    public async Task AddDayAsync(Day day)
    {
        try
        {
            await _dayRepository.InsertDayAsync(day);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while creating day");
            throw new DayCreateException("Failed to create day due to database constraint violation", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Database error while creating day");
            throw new DayCreateException("Failed to create day due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating day");
            throw new DayCreateException("Failed to create day", ex);
        }
    }

    public async Task AddPersonToDayAsync(Guid personId, Guid dayId)
    {
        try
        {
            await _dayRepository.GetDayByIdAsync(dayId);
            await _dayRepository.InsertPersonToDayAsync(personId, dayId);
        }
        catch (ArgumentException ex) when (ex.ParamName == "dayId")
        {
            _logger.LogError(ex, "Day {DayId} not found", dayId);
            throw new DayNotFoundException($"Day {dayId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding person {PersonId} to day {DayId}", personId, dayId);

            if (ex.InnerException?.Message.Contains("FK_PersonsDays_Persons") == true)
            {
                throw new PersonNotFoundException($"Person {personId} not found", ex);
            }

            throw new DayUpdateException($"Failed to add person {personId} to day {dayId}", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Conflict while adding person {PersonId} to day {DayId}", personId, dayId);
            throw new DayConflictException($"Person-day relation already exists: {personId}-{dayId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding person {PersonId} to day {DayId}", personId, dayId);
            throw new DayServiceException($"Failed to add person {personId} to day {dayId}", ex);
        }
    }

    public async Task AddPersonToDayAsync(Guid eventId, int daySequenceNumber, Guid personId)
    {
        try
        {
            var eventDays = await _dayRepository.GetAllDaysByEventAsync(eventId);

            var targetDay = eventDays.FirstOrDefault(d => d.SequenceNumber == daySequenceNumber)
                ?? throw new DayNotFoundException($"Day with sequence {daySequenceNumber} not found in event {eventId}");

            await _dayRepository.InsertPersonToDayAsync(personId, targetDay.Id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding person {PersonId} to day {DaySequence} in event {EventId}",
                personId, daySequenceNumber, eventId);
            throw new DayUpdateException($"Failed to add person {personId} to day {daySequenceNumber}", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error retrieving days for event {EventId}", eventId);
            throw new DayServiceException($"Failed to process event {eventId} days", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding person {PersonId} to day {DaySequence} in event {EventId}",
                personId, daySequenceNumber, eventId);
            throw new DayServiceException($"Failed to add person to day", ex);
        }
    }

    public async Task UpdateDayAsync(Day updateDay)
    {
        try
        {
            var existingDay = await _dayRepository.GetDayByIdAsync(updateDay.Id);
            await _dayRepository.UpdateDayAsync(updateDay);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Day {DayId} not found for update", updateDay.Id);
            throw new DayNotFoundException($"Day {updateDay.Id} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating day {DayId}", updateDay.Id);
            throw new DayUpdateException($"Failed to update day {updateDay.Id} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating day {DayId}", updateDay.Id);
            throw new DayUpdateException($"Concurrency conflict while updating day {updateDay.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating day {DayId}", updateDay.Id);
            throw new DayUpdateException($"Failed to update day {updateDay.Id}", ex);
        }
    }

    public async Task DeleteDayAsync(Guid dayId)
    {
        try
        {
            await _dayRepository.DeleteDayAsync(dayId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Day {DayId} not found for deletion", dayId);
            throw new DayNotFoundException($"Day {dayId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting day {DayId}", dayId);
            throw new DayDeleteException($"Failed to delete day {dayId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency error while deleting day {DayId}", dayId);
            throw new DayDeleteException($"Concurrency conflict while deleting day {dayId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting day {DayId}", dayId);
            throw new DayDeleteException($"Failed to delete day {dayId}", ex);
        }
    }

    public async Task DeletePersonFromDayAsync(Guid personId, Guid dayId)
    {
        try
        {
            await _dayRepository.DeletePersonFromDayAsync(personId, dayId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex,
                "Ошибка при удалении участника {PersonId} с дня {DayId}",
                personId, dayId);
            throw new DayServiceException(
                $"Не удалось удалить участника {personId} с дня {dayId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Неизвестная ошибка при удалении участника {PersonId} с дня {DayId}",
                personId, dayId);
            throw new DayServiceException(
                "Внутренняя ошибка сервиса при удалении участника с дня", ex);
        }
    }
}