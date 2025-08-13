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
using Eventor.Common.Enums;
using Eventor.Database.Models;

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
    /// Получить всех участников пользователя
    /// </summary>
    public async Task<List<Person>> GetAllPersonsByUserAsync(Guid userId)
    {
        try
        {
            return await _dbContext.UsersPersons
                .Where(up => up.UserId == userId)
                .Include(up => up.Person)
                .Select(up => PersonConverter.ConvertDBToCore(up.Person))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения участников пользователя {UserId}", userId);
            throw new InvalidOperationException($"Не удалось получить участников пользователя {userId}", ex);
        }
    }


    /// <summary>
    /// Получить всех участников конкретного дня, исключая некоторые роли
    /// </summary>
    /// <returns>Список всех участников конкретного дня, исключая некоторые роли</returns>
    public async Task<List<Person>> GetAllPersonsByDayExcludingTypesAsync(Guid dayId, List<PersonType> excludedTypes)
    {
        try
        {
            return await _dbContext.PersonsDays
                .Where(pd => pd.DayId == dayId)
                .Include(pd => pd.Person)
                .Where(pd => !excludedTypes.Contains(pd.Person.Type))
                .Select(pd => PersonConverter.ConvertDBToCore(pd.Person))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения участников дня {DayId} с исключением типов", dayId);
            throw new InvalidOperationException(
                $"Не удалось получить участников дня {dayId} с фильтрацией", ex);
        }
    }

    /// <summary>
    /// Получить всех участников конкретных дней, исключая некоторые роли
    /// </summary>
    /// <returns>Список всех участников конкретного дня, исключая некоторые роли</returns>
    public async Task<List<Person>> GetAllPersonsByDaysExcludingTypesAsync(List<Guid> dayIds, List<PersonType> excludedTypes)
    {
        try
        {
            return await _dbContext.PersonsDays
                .Where(pd => dayIds.Contains(pd.DayId))
                .Include(pd => pd.Person)
                .Where(pd => !excludedTypes.Contains(pd.Person.Type))
                .GroupBy(pd => pd.PersonId)
                .Where(g => g.Count() == dayIds.Count())
                .Select(g => PersonConverter.ConvertDBToCore(g.First().Person))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения участников дней с исключением типов");
            throw new InvalidOperationException(
                "Не удалось получить участников с фильтрацией по нескольким дням", ex);
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
    /// Получить участника мероприятия по идентификаторам пользователя и мероприятия
    /// </summary>
    public async Task<Person> GetPersonByUserAndEventAsync(Guid userId, Guid eventId)
    {
        try
        {
            var personEntity = await _dbContext.UsersPersons
                .Where(up => up.UserId == userId)
                .Include(up => up.Person)
                    .ThenInclude(p => p.SelectedDays)
                        .ThenInclude(pd => pd.Day)
                            .ThenInclude(d => d.EventDays)
                .Select(up => up.Person)
                .FirstOrDefaultAsync(p =>
                    p.SelectedDays.Any(pd =>
                        pd.Day.EventDays.Any(ed => ed.EventId == eventId)
                    ));

            return personEntity != null
                ? PersonConverter.ConvertDBToCore(personEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Ошибка получения участника для пользователя {UserId} и мероприятия {EventId}",
                userId, eventId);
            throw new InvalidOperationException(
                $"Не удалось найти участника для пользователя {userId} в мероприятии {eventId}", ex);
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

    public async Task<Person> InsertPersonForUserAsync(string personName, Guid userId)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // 1. Создание участника
            var newPerson = new PersonDBModel(
                id: Guid.NewGuid(),
                name: personName,
                type: PersonType.Standart,
                paid: false
            );

            await _dbContext.Persons.AddAsync(newPerson);
            await _dbContext.SaveChangesAsync();

            // 2. Связывание с пользователем
            var userPerson = new UserPersonDBModel(userId, newPerson.Id);
            await _dbContext.UsersPersons.AddAsync(userPerson);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return newPerson != null
                ? PersonConverter.ConvertDBToCore(newPerson)
                : null; ;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Ошибка создания участника для пользователя {UserId}", userId);
            throw new InvalidOperationException($"Не удалось создать участника для пользователя {userId}", ex);
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

    public async Task DeletePersonForUserAsync(Guid eventId, Guid userId)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // 1. Находим участника пользователя для конкретного мероприятия
            var person = await _dbContext.UsersPersons
                .Where(up => up.UserId == userId)
                .Include(up => up.Person)
                    .ThenInclude(p => p.SelectedDays)
                .Select(up => up.Person)
                .FirstOrDefaultAsync(p =>
                    p.SelectedDays.Any(pd =>
                        pd.Day.EventDays.Any(ed => ed.EventId == eventId)
                    ));

            if (person == null)
            {
                throw new InvalidOperationException("Участник не найден");
            }

            // 2. Удаляем связи участника с днями мероприятия
            var personDays = await _dbContext.PersonsDays
                .Where(pd => pd.PersonId == person.Id)
                .ToListAsync();

            _dbContext.PersonsDays.RemoveRange(personDays);

            // 3. Удаляем связь пользователь-участник
            var userPerson = await _dbContext.UsersPersons
                .FirstOrDefaultAsync(up =>
                    up.UserId == userId &&
                    up.PersonId == person.Id);

            if (userPerson != null)
            {
                _dbContext.UsersPersons.Remove(userPerson);
            }

            // 4. Удаляем самого участника
            _dbContext.Persons.Remove(person);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Ошибка удаления участника для пользователя {UserId} в мероприятии {EventId}", userId, eventId);
            throw new InvalidOperationException($"Не удалось удалить участника для пользователя {userId}", ex);
        }
    }
}

