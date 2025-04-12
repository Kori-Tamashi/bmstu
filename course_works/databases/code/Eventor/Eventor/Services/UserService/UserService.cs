using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Eventor.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        try
        {
            return await _userRepository.GetAllUserAsync();
        }
        catch (DbUpdateException ex)
        {
            LogAndThrow(ex, "Database error retrieving users", "Failed to retrieve users");
            throw;
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, "Unexpected error retrieving users", "Unexpected error occurred");
            throw;
        }
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        ValidateUserId(userId);

        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user ?? throw new UserNotFoundException($"User {userId} not found");
        }
        catch (DbUpdateException ex)
        {
            LogAndThrow(ex, $"Database error retrieving user {userId}", $"Failed to retrieve user {userId}");
            throw;
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, $"Unexpected error retrieving user {userId}", "Unexpected error occurred");
            throw;
        }
    }

    public async Task<User> GetUserByPhoneAsync(string phone)
    {
        ValidatePhone(phone);

        try
        {
            var user = await _userRepository.GetUserByPhoneAsync(phone);
            return user ?? throw new UserNotFoundException($"User with phone {phone} not found");
        }
        catch (UserNotFoundException)
        {
            throw; 
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error retrieving user by phone {Phone}", phone);
            throw new UserServiceException("Failed to retrieve user", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving user by phone {Phone}", phone);
            throw new UserServiceException("Unexpected error occurred", ex);
        }
    }

    public async Task AddUserAsync(User user)
    {
        ValidateUser(user);

        try
        {
            await _userRepository.InsertUserAsync(user);
        }
        catch (DbUpdateException ex)
        {
            LogAndThrow(ex, $"Database error creating user {user.Phone}",
                $"Failed to create user {user.Phone}");
            throw new UserCreateException("User creation failed", ex);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, $"Unexpected error creating user {user.Phone}",
                "Unexpected error occurred");
            throw new UserCreateException("User creation failed", ex);
        }
    }

    public async Task UpdateUserAsync(User updateUser)
    {
        ValidateUser(updateUser);

        try
        {
            await GetExistingUser(updateUser.Id);
            await _userRepository.UpdateUserAsync(updateUser);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrent modification error for user {UserId}", updateUser.Id);
            throw new UserUpdateException("Concurrent modification detected", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating user {UserId}", updateUser.Id);
            throw new UserUpdateException("Failed to update user", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating user {UserId}", updateUser.Id);
            throw new UserUpdateException("Unexpected error occurred", ex);
        }
    }

    public async Task UpdateUserNameAsync(string userPhone, string newName)
    {
        ValidatePhone(userPhone);
        ValidateName(newName);

        try
        {
            var user = await GetExistingUserByPhone(userPhone);
            user.Name = newName;
            await _userRepository.UpdateUserAsync(user);
        }
        catch (DbUpdateException ex)
        {
            LogAndThrow(ex, $"Database error updating name for {userPhone}",
                $"Failed to update name for {userPhone}");
            throw new UserUpdateException("Name update failed", ex);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, $"Unexpected error updating name for {userPhone}",
                "Unexpected error occurred");
            throw new UserUpdateException("Name update failed", ex);
        }
    }

    public async Task UpdateUserGenderAsync(string userPhone, Gender gender)
    {
        ValidatePhone(userPhone);

        try
        {
            var user = await GetExistingUserByPhone(userPhone);
            user.Gender = gender;
            await _userRepository.UpdateUserAsync(user);
        }
        catch (DbUpdateException ex)
        {
            LogAndThrow(ex, $"Database error updating gender for {userPhone}",
                $"Failed to update gender for {userPhone}");
            throw new UserUpdateException("Gender update failed", ex);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, $"Unexpected error updating gender for {userPhone}",
                "Unexpected error occurred");
            throw new UserUpdateException("Gender update failed", ex);
        }
    }

    public async Task UpdateUserRoleAsync(string userPhone, UserRole role)
    {
        ValidatePhone(userPhone);

        // Новая проверка валидности роли
        if (!Enum.IsDefined(typeof(UserRole), role))
        {
            throw new ValidationException($"Invalid role value: {(int)role}");
        }

        try
        {
            var user = await GetExistingUserByPhone(userPhone);
            user.Role = role;
            await _userRepository.UpdateUserAsync(user);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating role for user {Phone}", userPhone);
            throw new UserUpdateException($"Failed to update role for user {userPhone}", ex);
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        ValidateUserId(userId);

        try
        {
            await GetExistingUser(userId);
            await _userRepository.DeleteUserAsync(userId);
        }
        catch (DbUpdateException ex)
        {
            LogAndThrow(ex, $"Database error deleting user {userId}",
                $"Failed to delete user {userId}");
            throw new UserDeleteException("User deletion failed", ex);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, $"Unexpected error deleting user {userId}",
                "Unexpected error occurred");
            throw new UserDeleteException("User deletion failed", ex);
        }
    }

    #region Private Helpers
    private async Task<User> GetExistingUser(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user ?? throw new UserNotFoundException($"User {userId} not found");
    }

    private async Task<User> GetExistingUserByPhone(string phone)
    {
        var user = await _userRepository.GetUserByPhoneAsync(phone);
        return user ?? throw new UserNotFoundException($"User with phone {phone} not found");
    }

    private void ValidateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        ValidatePhone(user.Phone);
        ValidateName(user.Name);
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new ValidationException("Password hash is required");
    }

    private void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("Invalid user ID");
    }

    private void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length < 10) // Пример для международного формата
            throw new ValidationException("Invalid phone number");

        // Дополнительная проверка формата, если необходимо
        if (!phone.StartsWith("+"))
            throw new ValidationException("Phone number must start with '+'");
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            throw new ValidationException("Invalid user name");
    }

    private void LogAndThrow(Exception ex, string logMessage, string exceptionMessage)
    {
        _logger.LogError(ex, logMessage);
        throw new UserServiceException(exceptionMessage, ex);
    }
    #endregion
}