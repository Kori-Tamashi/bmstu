using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventor.Services;

public class OAuthService : IOauthService
{
    private readonly IUserService _userService;
    private readonly ILogger<OAuthService> _logger;

    public OAuthService(IUserService userService, ILogger<OAuthService> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Registrate(User user, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        if (!IsValidPhone(user.Phone))
            throw new ArgumentException("Invalid phone format", nameof(user.Phone));

        try
        {
            try
            {
                var existingUser = await _userService.GetUserByPhoneAsync(user.Phone);
                throw new UserLoginAlreadyExistsException($"Phone {user.Phone} already registered");
            }
            catch (UserNotFoundException)
            {
                // Продолжаем регистрацию
            }

            user.CreateHash(password);
            await _userService.AddUserAsync(user);

            _logger.LogInformation("User registered successfully | Phone: {Phone}", user.Phone);
        }
        catch (UserLoginAlreadyExistsException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error during registration | Phone: {Phone}", user.Phone);
            throw new OAuthServiceException("Database error during registration", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration | Phone: {Phone}", user.Phone);
            throw new OAuthServiceException("Registration failed", ex);
        }
    }

    // Валидация номера телефона с поддержкой международных форматов
    private bool IsValidPhone(string phone)
    {
        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var numberProto = phoneNumberUtil.Parse(phone, null);
            return phoneNumberUtil.IsValidNumber(numberProto);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }

    public async Task<User> Login(string phone, string password)
    {
        try
        {
            var user = await _userService.GetUserByPhoneAsync(phone);

            if (!user.VerifyPassword(password))
            {
                _logger.LogWarning("Invalid password for phone: {Phone}", phone);
                throw new IncorrectPasswordException("Invalid credentials");
            }

            return user;
        }
        catch (UserNotFoundException ex)
        {
            _logger.LogWarning("Login attempt for non-existent phone: {Phone}", phone);
            throw new UserLoginNotFoundException($"Phone {phone} not registered", ex);
        }
        catch (IncorrectPasswordException) 
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error during login | Phone: {Phone}", phone);
            throw new OAuthServiceException("Database error during login", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login | Phone: {Phone}", phone);
            throw new OAuthServiceException("Login failed", ex);
        }
    }
}