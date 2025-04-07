using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Database.Repositories;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Eventor.Database.Core;
using Eventor.Services;

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

    public async Task<List<Common.Core.Day>> GetAllDaysAsync()
    {
        try
        {
            return await _dayRepository.GetAllDaysAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving days");
            throw new DayServiceException("Failed to retrieve days", ex);
        }
    }

    public async Task<List<Common.Core.Day>> GetAllDaysByEventAsync(Guid eventId)
    {
        try
        {
            return await _dayRepository.GetAllDaysByEventAsync(eventId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving days for event {EventId}", eventId);
            throw new DayServiceException($"Failed to retrieve days for event {eventId}", ex);
        }
    }

    public async Task<Common.Core.Day> GetDayByIdAsync(Guid dayId)
    {
        try
        {
            return await _dayRepository.GetDayByIdAsync(dayId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Day {DayId} not found", dayId);
            throw new DayNotFoundException($"Day {dayId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving day {DayId}", dayId);
            throw new DayServiceException($"Failed to retrieve day {dayId}", ex);
        }
    }

    public async Task AddDayAsync(Common.Core.Day day)
    {
        try
        {
            await _dayRepository.InsertDayAsync(day);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating day");
            throw new DayCreateException("Failed to create day", ex);
        }
    }

    public async Task UpdateDayAsync(Common.Core.Day updateDay)
    {
        try
        {
            var existingDay = await _dayRepository.GetDayByIdAsync(updateDay.Id);
            await _dayRepository.UpdateDayAsync(existingDay);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Day {DayId} not found for update", updateDay.Id);
            throw new DayNotFoundException($"Day {updateDay.Id} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating day {DayId}", updateDay.Id);
            throw new DayUpdateException($"Failed to update day {updateDay.Id}", ex);
        }
    }

    public async Task DeleteDayAsync(Guid dayId)
    {
        try
        {
            await _dayRepository.DeleteDayAsync(dayId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Day {DayId} not found for deletion", dayId);
            throw new DayNotFoundException($"Day {dayId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting day {DayId}", dayId);
            throw new DayDeleteException($"Failed to delete day {dayId}", ex);
        }
    }
}