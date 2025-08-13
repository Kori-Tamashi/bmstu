using Eventor.Common.Core;
using Eventor.Services.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.Services;
using Microsoft.EntityFrameworkCore;

namespace Eventor.Tests.Services;

public class OAuthServiceTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<OAuthService>> _mockLogger;
    private readonly OAuthService _oauthService;
    private readonly User _testUser;

    public OAuthServiceTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<OAuthService>>();
        _oauthService = new OAuthService(_mockUserService.Object, _mockLogger.Object);

        _testUser = new User(
            Guid.NewGuid(),
            "Test User",
            "+12025550123", 
            Gender.Male,
            "hashed_password",
            UserRole.User
        );
    }

    [Fact]
    public async Task Registrate_NewUser_SuccessfullyRegisters()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUserByPhoneAsync(_testUser.Phone))
            .ThrowsAsync(new UserNotFoundException(""));
        _mockUserService.Setup(s => s.AddUserAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        await _oauthService.Registrate(_testUser, "password123");

        // Assert
        _mockUserService.Verify(s => s.AddUserAsync(It.Is<User>(u => u.Phone == _testUser.Phone)), Times.Once);
    }

    [Fact]
    public async Task Registrate_ExistingUser_ThrowsAlreadyExistsException()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUserByPhoneAsync(_testUser.Phone))
            .ReturnsAsync(_testUser);

        // Act & Assert
        await Assert.ThrowsAsync<UserLoginAlreadyExistsException>(
            () => _oauthService.Registrate(_testUser, "password123"));
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var testPassword = "correct_password";
        _testUser.CreateHash(testPassword);

        _mockUserService.Setup(s => s.GetUserByPhoneAsync(_testUser.Phone))
            .ReturnsAsync(_testUser);

        // Act
        var result = await _oauthService.Login(_testUser.Phone, testPassword);

        // Assert
        Assert.Equal(_testUser.Phone, result.Phone);
    }

    [Fact]
    public async Task Login_NonExistentUser_ThrowsNotFoundExceptionAndLogs()
    {
        // Arrange
        const string testPhone = "+000000000";
        _mockUserService.Setup(s => s.GetUserByPhoneAsync(It.IsAny<string>()))
            .ThrowsAsync(new UserNotFoundException(""));

        // Act & Assert
        await Assert.ThrowsAsync<UserLoginNotFoundException>(
            () => _oauthService.Login(testPhone, "anypass"));

        // Проверка логов через LogLevel и сообщение
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Login attempt for non-existent phone: {testPhone}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task Login_WrongPassword_ThrowsIncorrectPasswordExceptionAndLogs()
    {
        // Arrange
        _testUser.CreateHash("correct_password");
        _mockUserService.Setup(s => s.GetUserByPhoneAsync(_testUser.Phone))
            .ReturnsAsync(_testUser);

        // Act & Assert
        await Assert.ThrowsAsync<IncorrectPasswordException>(
            () => _oauthService.Login(_testUser.Phone, "wrong_password"));

        // Проверка логов через LogLevel и сообщение
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Invalid password for phone: {_testUser.Phone}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }


    [Fact]
    public async Task Registrate_DatabaseError_ThrowsOAuthServiceException()
    {
        // Arrange
        var validUser = new User(
            Guid.NewGuid(),
            "Test User",
            "+12025550123",  // Валидный международный номер (пример для США)
            Gender.Male,
            "hashed_password",
            UserRole.User
        );

        _mockUserService.Setup(s => s.GetUserByPhoneAsync(validUser.Phone))
            .ThrowsAsync(new UserNotFoundException(""));

        _mockUserService.Setup(s => s.AddUserAsync(It.IsAny<User>()))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<OAuthServiceException>(
            () => _oauthService.Registrate(validUser, "password123"));
    }

    [Fact]
    public async Task Registrate_EmptyPassword_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _oauthService.Registrate(_testUser, ""));
    }

    [Fact]
    public async Task Registrate_InvalidPhoneFormat_ThrowsArgumentException()
    {
        // Arrange
        var invalidUser = new User(
            Guid.NewGuid(),
            "Invalid User",
            "invalid_phone", // Не соответствует формату
            Gender.Male,
            "hashed_password",
            UserRole.User
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _oauthService.Registrate(invalidUser, "password123"));
    }

    [Fact]
    public async Task Registrate_GeneralError_ThrowsOAuthServiceException()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUserByPhoneAsync(_testUser.Phone))
            .ThrowsAsync(new InvalidOperationException("Some error"));

        // Act & Assert
        await Assert.ThrowsAsync<OAuthServiceException>(
            () => _oauthService.Registrate(_testUser, "password123"));
    }

    [Fact]
    public async Task Registrate_Success_LogsInformation()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUserByPhoneAsync(_testUser.Phone))
            .ThrowsAsync(new UserNotFoundException(""));
        _mockUserService.Setup(s => s.AddUserAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        await _oauthService.Registrate(_testUser, "password123");

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString().Contains("User registered successfully") &&
                    v.ToString().Contains($"Phone: {_testUser.Phone}")
                ),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }

    [Fact]
    public void VerifyPassword_CorrectHash_ReturnsTrue()
    {
        // Arrange
        var password = "correct_password";
        _testUser.CreateHash(password);

        // Act
        var result = _testUser.VerifyPassword(password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        // Arrange
        var password = "correct_password";
        _testUser.CreateHash(password);

        // Act
        var result = _testUser.VerifyPassword("wrong_password");

        // Assert
        Assert.False(result);
    }
}