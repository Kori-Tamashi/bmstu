using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.Extensions.Logging;

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
        try
        {
            var existingUser = await _userService.GetUserByPhoneAsync(user.Phone);
            throw new UserLoginAlreadyExistsException($"Phone {user.Phone} already registered");
        }
        catch (UserNotFoundException) { }

        user.CreateHash(password);

        try
        {
            await _userService.AddUserAsync(user);
        }
        catch (UserCreateException ex)
        {
            _logger.LogError(ex, "Registration failed | Phone: {Phone}", user.Phone);
            throw new UserCreateException("Registration failed", ex);
        }
    }

    public async Task<User> Login(string phone, string password)
    {
        User user;
        try
        {
            user = await _userService.GetUserByPhoneAsync(phone);
        }
        catch (UserNotFoundException ex)
        {
            _logger.LogWarning("Login attempt for non-existent phone: {Phone}", phone);
            throw new UserLoginNotFoundException($"Phone {phone} not registered", ex);
        }

        if (!user.VerifyPassword(password))
        {
            _logger.LogWarning("Invalid password for phone: {Phone}", phone);
            throw new IncorrectPasswordException("Invalid credentials");
        }

        return user;
    }
}