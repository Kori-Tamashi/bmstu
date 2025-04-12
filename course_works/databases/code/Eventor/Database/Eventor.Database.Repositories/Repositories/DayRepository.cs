using Eventor.Common.Converter;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Day = Eventor.Common.Core.Day;

namespace Eventor.Database.Repositories;

public class DayRepository : BaseRepository, IDayRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<DayRepository> _logger;

    public DayRepository(
        EventorDBContext dbContext,
        ILogger<DayRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<Day>> GetAllDaysAsync()
    {
        try
        {
            return await _dbContext.Days
                .Select(d => DayConverter.ConvertDBToCore(d))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка дней");
            throw new InvalidOperationException("Не удалось получить список дней", ex);
        }
    }

    public async Task<List<Day>> GetAllDaysByEventAsync(Guid eventId)
    {
        try
        {
            return await _dbContext.EventsDays
                .Where(ed => ed.EventId == eventId)
                .Include(ed => ed.Day)
                .Select(ed => DayConverter.ConvertDBToCore(ed.Day))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения дней мероприятия {EventId}", eventId);
            throw new InvalidOperationException($"Не удалось получить дни мероприятия {eventId}", ex);
        }
    }

    public async Task<List<Day>> GetAllDaysByPersonAsync(Guid personId)
    {
        try
        {
            return await _dbContext.PersonsDays
                .Where(pd => pd.PersonId == personId)
                .Include(pd => pd.Day)
                .Select(pd => DayConverter.ConvertDBToCore(pd.Day))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения дней участника {PersonId}", personId);
            throw new InvalidOperationException($"Не удалось получить дни участника {personId}", ex);
        }
    }

    public async Task<Day> GetDayByIdAsync(Guid dayId)
    {
        try
        {
            var day = await _dbContext.Days
                .FirstOrDefaultAsync(d => d.Id == dayId);

            if (day == null)
            {
                throw new ArgumentException($"День {dayId} не найден");
            }

            return DayConverter.ConvertDBToCore(day);
        }
        catch (DbUpdateException ex) // Перехватываем только исключения БД
        {
            _logger.LogError(ex, "Ошибка получения дня {DayId}", dayId);
            throw new InvalidOperationException($"Не удалось получить день {dayId}", ex);
        }
    }

    public async Task InsertDayAsync(Day day)
    {
        try
        {
            var dayDb = DayConverter.ConvertCoreToDB(day);
            await _dbContext.Days.AddAsync(dayDb);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания дня");
            throw new InvalidOperationException("Не удалось создать день", ex);
        }
    }

    public async Task UpdateDayAsync(Day updateDay)
    {
        try
        {
            // Get the existing entity from the database
            var existingDay = await _dbContext.Days
                .FirstOrDefaultAsync(d => d.Id == updateDay.Id);

            if (existingDay == null)
            {
                throw new ArgumentException($"День {updateDay.Id} не найден");
            }

            // Update fields
            existingDay.MenuId = updateDay.MenuId;
            existingDay.Name = updateDay.Name;
            existingDay.SequenceNumber = updateDay.SequenceNumber;
            existingDay.Description = updateDay.Description;
            existingDay.Price = updateDay.Price;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления дня {DayId}", updateDay.Id);
            throw new InvalidOperationException($"Не удалось обновить день {updateDay.Id}", ex);
        }
    }

    public async Task DeleteDayAsync(Guid dayId)
    {
        try
        {
            var day = await _dbContext.Days.FindAsync(dayId);
            if (day == null)
            {
                throw new ArgumentException($"День {dayId} не найден");
            }

            _dbContext.Days.Remove(day);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления дня {DayId}", dayId);
            throw new InvalidOperationException($"Не удалось удалить день {dayId}", ex);
        }
    }
}