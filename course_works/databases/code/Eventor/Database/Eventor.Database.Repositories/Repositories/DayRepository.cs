using Eventor.Common.Converter;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Microsoft.EntityFrameworkCore;
namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий дней мероприятий
/// </summary>
public class DayRepository : BaseRepository, IDayRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Конструктор репозитория дней
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    public DayRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все дни мероприятий
    /// </summary>
    /// <returns>Список всех дней</returns>
    public async Task<List<Common.Core.Day>> GetAllDaysAsync()
    {
        return await _dbContext.Days
            .Select(d => DayConverter.ConvertDBToCore(d))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить дни конкретного мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns>Список дней мероприятия</returns>
    public async Task<List<Common.Core.Day>> GetAllDaysByEventAsync(Guid eventId)
    {
        return await _dbContext.EventsDays
            .Where(ed => ed.EventId == eventId)
            .Include(ed => ed.Day)
            .Select(ed => DayConverter.ConvertDBToCore(ed.Day))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить день по идентификатору
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <returns>Объект дня</returns>
    public async Task<Common.Core.Day> GetDayByIdAsync(Guid dayId)
    {
        var day = await _dbContext.Days
            .FirstOrDefaultAsync(d => d.Id == dayId);

        return DayConverter.ConvertDBToCore(day);
    }

    /// <summary>
    /// Создать новый день
    /// </summary>
    /// <param name="day">Объект дня для создания</param>
    public async Task InsertDayAsync(Common.Core.Day day)
    {
        var dayDb = DayConverter.ConvertCoreToDB(day);
        await _dbContext.Days.AddAsync(dayDb);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить существующий день
    /// </summary>
    /// <param name="updateDay">Обновленный объект дня</param>
    public async Task UpdateDayAsync(Common.Core.Day updateDay)
    {
        var dayDb = DayConverter.ConvertCoreToDB(updateDay);
        _dbContext.Days.Update(dayDb);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить день
    /// </summary>
    /// <param name="dayId">Идентификатор дня для удаления</param>
    public async Task DeleteDayAsync(Guid dayId)
    {
        var day = await _dbContext.Days.FindAsync(dayId);

        if (day == null)
        {
            throw new ArgumentException($"День с ID {dayId} не найден.");
        }

        _dbContext.Days.Remove(day);
        await _dbContext.SaveChangesAsync();
    }
}