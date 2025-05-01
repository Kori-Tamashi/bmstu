using Eventor.Common.Core;
namespace Eventor.Services;

/// <summary>
/// Интерфейс сервиса участников
/// </summary>
public interface IPersonService
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
    /// Получить всех участников пользователя
    /// </summary>
    /// <returns>Список всех участников пользователя</returns>
    Task<List<Person>> GetAllPersonsByUserAsync(Guid userId);

    /// <summary>
    /// Получить участника мероприятия по идентификатору
    /// </summary>
    /// <returns>Участник</returns>
    Task<Person> GetPersonByIdAsync(Guid personId);

    /// <summary>
    /// Получить участника мероприятия по идентификаторам пользователя и мероприятия
    /// </summary>
    /// <returns>Участник</returns>
    Task<Person> GetPersonByUserAndEventAsync(Guid userId, Guid eventId);

    /// <summary>
    /// Создать участника
    /// </summary>
    /// <returns></returns>
    Task AddPersonAsync(Person person);

    /// <summary>
    /// Создать участника для пользователя
    /// </summary>
    /// <returns>Id участника</returns>
    Task<Person> AddPersonForUserAsync(string personName, Guid userId);

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

    /// <summary>
    /// Удалить участника для пользователя
    /// </summary>
    /// <returns></returns>
    Task DeletePersonForUserAsync(Guid eventId, Guid userId);
}

