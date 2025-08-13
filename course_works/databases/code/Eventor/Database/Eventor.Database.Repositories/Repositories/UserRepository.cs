using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventor.Database.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(EventorDBContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        try
        {
            return await _dbContext.Users
                .Select(u => UserConverter.ConvertDBToCore(u))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка пользователей");
            throw new InvalidOperationException("Не удалось получить список пользователей", ex);
        }
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return userEntity != null
                ? UserConverter.ConvertDBToCore(userEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения пользователя {UserId}", userId);
            throw new InvalidOperationException($"Не удалось получить пользователя {userId}", ex);
        }
    }

    public async Task<User> GetUserByPhoneAsync(string phone)
    {
        try
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Phone == phone);

            return userEntity != null
                ? UserConverter.ConvertDBToCore(userEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка поиска пользователя по телефону {Phone}", phone);
            throw new InvalidOperationException($"Не удалось найти пользователя с телефоном {phone}", ex);
        }
    }

    public async Task InsertUserAsync(User user)
    {
        try
        {
            var userEntity = UserConverter.ConvertCoreToDB(user);
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания пользователя {Phone}", user.Phone);
            throw new InvalidOperationException($"Не удалось создать пользователя {user.Phone}", ex);
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                _logger.LogWarning("Пользователь {UserId} не найден", user.Id);
                return;
            }

            existingUser.Name = user.Name;
            existingUser.Phone = user.Phone;
            existingUser.Gender = user.Gender;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Role = user.Role;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления пользователя {UserId}", user.Id);
            throw new InvalidOperationException($"Не удалось обновить пользователя {user.Id}", ex);
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        try
        {
            var userToDelete = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userToDelete == null)
            {
                _logger.LogWarning("Пользователь {UserId} не найден", userId);
                return;
            }

            _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления пользователя {UserId}", userId);
            throw new InvalidOperationException($"Не удалось удалить пользователя {userId}", ex);
        }
    }
}