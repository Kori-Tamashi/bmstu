using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Database.Repositories;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            _logger.LogError(ex, "Error retrieving all users");
            throw new UserServiceException("Failed to retrieve users", ex);
        }
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User {userId} not found");
            }
            return user;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", userId);
            throw new UserServiceException($"Failed to retrieve user {userId}", ex);
        }
    }

    public async Task<User> GetUserByPhoneAsync(string phone)
    {
        try
        {
            var user = await _userRepository.GetUserByPhoneAsync(phone);
            if (user == null)
            {
                throw new UserNotFoundException($"User with phone {phone} not found");
            }
            return user;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving user by phone {Phone}", phone);
            throw new UserServiceException($"Failed to retrieve user with phone {phone}", ex);
        }
    }

    public async Task AddUserAsync(User user)
    {
        try
        {
            await _userRepository.InsertUserAsync(user);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating user");
            throw new UserCreateException("Failed to create user", ex);
        }
    }

    public async Task UpdateUserAsync(User updateUser)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByIdAsync(updateUser.Id);
            if (existingUser == null)
            {
                throw new UserNotFoundException($"User {updateUser.Id} not found");
            }

            await _userRepository.UpdateUserAsync(updateUser);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", updateUser.Id);
            throw new UserUpdateException($"Failed to update user {updateUser.Id}", ex);
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException($"User {userId} not found");
            }

            await _userRepository.DeleteUserAsync(userId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", userId);
            throw new UserDeleteException($"Failed to delete user {userId}", ex);
        }
    }
}