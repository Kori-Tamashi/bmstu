using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория пользователя
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    Task<List<User>> GetAllUserAsync();

    /// <summary>
    /// Получить пользователя по его идентификатору
    /// </summary>
    /// <returns>Пользователь</returns>
    Task<User> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Получить пользователя по его номеру телефона
    /// </summary>
    /// <returns>Пользователь</returns>
    Task<User> GetUserByPhoneAsync(string phone);

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <returns></returns>
    Task InsertUserAsync(User user);

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <returns></returns>
    Task UpdateUserAsync(User updateUser);

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <returns></returns>
    Task DeleteUserAsync(Guid userId);
}
