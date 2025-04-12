using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с участниками мероприятий
/// </summary>
public class PersonRepository : BaseRepository, IPersonRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<PersonRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public PersonRepository(
        EventorDBContext dbContext,
        ILogger<PersonRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить всех участников
    /// </summary>
    public async Task<List<Person>> GetAllPersonsAsync()
    {
        try
        {
            return await _dbContext.Persons
                .Select(p => PersonConverter.ConvertDBToCore(p))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка участников");
            throw new InvalidOperationException("Не удалось получить список участников", ex);
        }
    }

    /// <summary>
    /// Получить участников конкретного дня мероприятия
    /// </summary>
    public async Task<List<Person>> GetAllPersonsByDayAsync(Guid dayId)
    {
        try
        {
            return await _dbContext.PersonsDays
                .Where(pd => pd.DayId == dayId)
                .Include(pd => pd.Person)
                .Select(pd => PersonConverter.ConvertDBToCore(pd.Person))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения участников дня {DayId}", dayId);
            throw new InvalidOperationException($"Не удалось получить участников дня {dayId}", ex);
        }
    }

    /// <summary>
    /// Получить всех участников мероприятия
    /// </summary>
    public async Task<List<Person>> GetAllPersonsByEventAsync(Guid eventId)
    {
        try
        {
            var eventDays = await _dbContext.EventsDays
                .Where(ed => ed.EventId == eventId)
                .Select(ed => ed.DayId)
                .ToListAsync();

            return await _dbContext.PersonsDays
                .Where(pd => eventDays.Contains(pd.DayId))
                .Include(pd => pd.Person)
                .Select(pd => PersonConverter.ConvertDBToCore(pd.Person))
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения участников мероприятия {EventId}", eventId);
            throw new InvalidOperationException($"Не удалось получить участников мероприятия {eventId}", ex);
        }
    }

    /// <summary>
    /// Получить участника по идентификатору
    /// </summary>
    public async Task<Person> GetPersonByIdAsync(Guid personId)
    {
        try
        {
            var personEntity = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId);

            return personEntity != null
                ? PersonConverter.ConvertDBToCore(personEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения участника {PersonId}", personId);
            throw new InvalidOperationException($"Не удалось получить участника {personId}", ex);
        }
    }

    /// <summary>
    /// Добавить нового участника
    /// </summary>
    public async Task InsertPersonAsync(Person person)
    {
        try
        {
            var personEntity = PersonConverter.ConvertCoreToDB(person);
            await _dbContext.Persons.AddAsync(personEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания участника");
            throw new InvalidOperationException("Не удалось создать участника", ex);
        }
    }

    /// <summary>
    /// Обновить данные участника
    /// </summary>
    public async Task UpdatePersonAsync(Person updatePerson)
    {
        try
        {
            var existingPerson = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == updatePerson.Id);

            if (existingPerson == null)
            {
                _logger.LogWarning("Участник {PersonId} не найден", updatePerson.Id);
                return;
            }

            existingPerson.Name = updatePerson.Name;
            existingPerson.Type = updatePerson.Type;
            existingPerson.Paid = updatePerson.Paid;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления участника {PersonId}", updatePerson.Id);
            throw new InvalidOperationException($"Не удалось обновить участника {updatePerson.Id}", ex);
        }
    }

    /// <summary>
    /// Удалить участника по идентификатору
    /// </summary>
    public async Task DeletePersonAsync(Guid personId)
    {
        try
        {
            var personToDelete = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (personToDelete == null)
            {
                _logger.LogWarning("Участник {PersonId} не найден", personId);
                return;
            }

            _dbContext.Persons.Remove(personToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления участника {PersonId}", personId);
            throw new InvalidOperationException($"Не удалось удалить участника {personId}", ex);
        }
    }
}