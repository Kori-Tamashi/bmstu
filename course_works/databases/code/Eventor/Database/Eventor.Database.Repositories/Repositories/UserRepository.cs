using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями системы
/// </summary>
public class UserRepository : BaseRepository, IUserRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория пользователей
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public UserRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    public async Task<List<User>> GetAllUserAsync()
    {
        return await _dbContext.Users
            .Select(u => UserConverter.ConvertDBToCore(u))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Найденный пользователь или null</returns>
    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var userEntity = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return userEntity != null
            ? UserConverter.ConvertDBToCore(userEntity)
            : null;
    }

    /// <summary>
    /// Получить пользователя по номеру телефона
    /// </summary>
    /// <param name="phone">Номер телефона</param>
    /// <returns>Найденный пользователь или null</returns>
    public async Task<User> GetUserByPhoneAsync(string phone)
    {
        var userEntity = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Phone == phone);

        return userEntity != null
            ? UserConverter.ConvertDBToCore(userEntity)
            : null;
    }

    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    /// <param name="user">Модель пользователя для создания</param>
    public async Task InsertUserAsync(User user)
    {
        var userEntity = UserConverter.ConvertCoreToDB(user);
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить существующего пользователя
    /// </summary>
    /// <param name="user">Модель с обновленными данными пользователя</param>
    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser == null) return;

        existingUser.Name = user.Name;
        existingUser.Phone = user.Phone;
        existingUser.Gender = user.Gender;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.Role = user.Role;

        _dbContext.Users.Update(existingUser);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить пользователя по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор удаляемого пользователя</param>
    public async Task DeleteUserAsync(Guid userId)
    {
        var userToDelete = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (userToDelete == null) return;

        _dbContext.Users.Remove(userToDelete);
        await _dbContext.SaveChangesAsync();
    }
}
