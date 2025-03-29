using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с участниками мероприятий
/// </summary>
public class PersonRepository : BaseRepository, IPersonRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public PersonRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить всех участников
    /// </summary>
    /// <returns>Список всех участников в формате Core-модели</returns>
    public async Task<List<Person>> GetAllPersonsAsync()
    {
        return await _dbContext.Persons
            .Select(p => PersonConverter.ConvertDBToCore(p))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить участников конкретного дня мероприятия
    /// </summary>
    /// <param name="dayId">Идентификатор дня мероприятия</param>
    /// <returns>Список участников, привязанных к указанному дню</returns>
    public async Task<List<Person>> GetAllPersonsByDayAsync(Guid dayId)
    {
        return await _dbContext.PersonsDays
            .Where(pd => pd.DayId == dayId)
            .Include(pd => pd.Person)
            .Select(pd => PersonConverter.ConvertDBToCore(pd.Person))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить всех участников мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns>Уникальный список участников, связанных с любым из дней мероприятия</returns>
    public async Task<List<Person>> GetAllPersonsByEventAsync(Guid eventId)
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

    /// <summary>
    /// Получить участника по идентификатору
    /// </summary>
    /// <param name="personId">Идентификатор участника</param>
    /// <returns>Найденный участник или null</returns>
    public async Task<Person> GetPersonByIdAsync(Guid personId)
    {
        var personEntity = await _dbContext.Persons
            .FirstOrDefaultAsync(p => p.Id == personId);

        return personEntity != null
            ? PersonConverter.ConvertDBToCore(personEntity)
            : null;
    }

    /// <summary>
    /// Добавить нового участника
    /// </summary>
    /// <param name="person">Core-модель участника для добавления</param>
    public async Task InsertPersonAsync(Person person)
    {
        var personEntity = PersonConverter.ConvertCoreToDB(person);
        await _dbContext.Persons.AddAsync(personEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить данные участника
    /// </summary>
    /// <param name="updatePerson">Core-модель с обновленными данными</param>
    public async Task UpdatePersonAsync(Person updatePerson)
    {
        var existingPerson = await _dbContext.Persons
            .FirstOrDefaultAsync(p => p.Id == updatePerson.Id);

        if (existingPerson == null) return;

        existingPerson.Name = updatePerson.Name;
        existingPerson.Type = updatePerson.Type;
        existingPerson.Paid = updatePerson.Paid;

        _dbContext.Persons.Update(existingPerson);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить участника по идентификатору
    /// </summary>
    /// <param name="personId">Идентификатор удаляемого участника</param>
    public async Task DeletePersonAsync(Guid personId)
    {
        var personToDelete = await _dbContext.Persons
            .FirstOrDefaultAsync(p => p.Id == personId);

        if (personToDelete == null) return;

        _dbContext.Persons.Remove(personToDelete);
        await _dbContext.SaveChangesAsync();
    }
}