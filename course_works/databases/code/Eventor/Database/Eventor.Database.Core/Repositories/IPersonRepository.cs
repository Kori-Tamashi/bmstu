using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория участников
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Получить всех участников
    /// </summary>
    /// <returns>Список всех участников</returns>
    Task<List<Person>> GetAllPersonsAsync();

    /// <summary>
    /// Получить всех участников конкретного дня мероприятия
    /// </summary>
    /// <returns>Список всех участников конкретного дня мероприятия</returns>
    Task<List<Person>> GetAllPersonsByDayAsync(Guid dayId);

    /// <summary>
    /// Получить всех участников мероприятия
    /// </summary>
    /// <returns>Список всех участников мероприятия</returns>
    Task<List<Person>> GetAllPersonsByEventAsync(Guid eventId);

    /// <summary>
    /// Получить участника мероприятия по идентификатору
    /// </summary>
    /// <returns>Мероприятие</returns>
    Task<Person> GetPersonByIdAsync(Guid personId);

    /// <summary>
    /// Создать участника
    /// </summary>
    /// <returns></returns>
    Task InsertPersonAsync(Person person);

    /// <summary>
    /// Обновить участника
    /// </summary>
    /// <returns></returns>
    Task UpdatePersonAsync(Person updatePerson);

    /// <summary>
    /// Удалить участника
    /// </summary>
    /// <returns></returns>
    Task DeletePersonAsync(Guid personId);
}

