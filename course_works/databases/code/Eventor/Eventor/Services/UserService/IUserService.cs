using Eventor.Common.Core;
using Eventor.Common.Enums;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория пользователя
/// </summary>
public interface IUserService
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
    Task AddUserAsync(User user);

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <returns></returns>
    Task UpdateUserAsync(User updateUser);

    /// <summary>
    /// Обновить имя пользователя 
    /// </summary>
    /// <returns></returns>
    Task UpdateUserNameAsync(string userPhone, string newName);

    /// <summary>
    /// Обновить гендер пользователя 
    /// </summary>
    /// <returns></returns>
    Task UpdateUserGenderAsync(string userPhone, Gender gender);

    /// <summary>
    /// Обновить права доступа пользователя
    /// </summary>
    /// <returns></returns>
    Task UpdateUserRoleAsync(string userPhone, UserRole role);

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <returns></returns>
    Task DeleteUserAsync(Guid userId);
}
