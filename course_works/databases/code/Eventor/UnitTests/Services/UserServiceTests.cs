using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Database.Core;
using Eventor.Services;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Eventor.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;
    private readonly Guid _testUserId = Guid.NewGuid();
    private const string ValidPhone = "+1234567890";

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockRepo.Object, _mockLogger.Object);
    }

    private User CreateTestUser() => new User(
        _testUserId,
        "Test User",
        ValidPhone,
        Gender.Male,
        "hashed_password",
        UserRole.User
    );

    [Fact]
    public async Task GetAllUserAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User> { CreateTestUser() };
        _mockRepo.Setup(r => r.GetAllUserAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUserAsync();

        // Assert
        Assert.Single(result);
        _mockRepo.Verify(r => r.GetAllUserAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllUserAsync_DbException_LogsAndThrows()
    {
        // Arrange
        var exception = new DbUpdateException("Database error");
        _mockRepo.Setup(r => r.GetAllUserAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<UserServiceException>(() => _userService.GetAllUserAsync());
        VerifyLog(LogLevel.Error, "Database error retrieving users", exception);
    }

    [Fact]
    public async Task GetUserByIdAsync_InvalidId_ThrowsValidationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _userService.GetUserByIdAsync(Guid.Empty));
    }

    [Fact]
    public async Task GetUserByPhoneAsync_InvalidPhone_ThrowsValidationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _userService.GetUserByPhoneAsync("123")); // Невалидный короткий номер
    }

    [Fact]
    public async Task GetUserByPhoneAsync_ValidPhoneButNotFound_ThrowsNotFoundException()
    {
        // Arrange
        const string validPhone = "+1234567890";
        _mockRepo.Setup(r => r.GetUserByPhoneAsync(validPhone))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _userService.GetUserByPhoneAsync(validPhone));
    }

    [Fact]
    public async Task AddUserAsync_InvalidUser_ThrowsValidationException()
    {
        // Arrange
        var invalidUser = new User(Guid.NewGuid(), "", "123", Gender.Male, "", UserRole.User);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _userService.AddUserAsync(invalidUser));
    }

    [Fact]
    public async Task UpdateUserAsync_ConcurrentModification_ThrowsException()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);
        _mockRepo.Setup(r => r.UpdateUserAsync(testUser))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UserUpdateException>(
            () => _userService.UpdateUserAsync(testUser));

        // Проверка сообщения и типа внутреннего исключения
        Assert.IsType<DbUpdateConcurrencyException>(ex.InnerException);
        Assert.Contains("Concurrent modification", ex.Message);
    }

    [Fact]
    public async Task UpdateUserNameAsync_OnlyNameChanged()
    {
        // Arrange
        var originalUser = CreateTestUser();
        var newName = "New Name";

        _mockRepo.Setup(r => r.GetUserByPhoneAsync(ValidPhone))
            .ReturnsAsync(originalUser);

        // Act
        await _userService.UpdateUserNameAsync(ValidPhone, newName);

        // Assert
        _mockRepo.Verify(r => r.UpdateUserAsync(It.Is<User>(u =>
            u.Name == newName &&
            u.Phone == originalUser.Phone &&
            u.Gender == originalUser.Gender &&
            u.Role == originalUser.Role
        )), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_VerifyCascadingOperations()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);

        // Act
        await _userService.DeleteUserAsync(_testUserId);

        // Assert
        // Проверяем все вызовы репозитория
        _mockRepo.Verify(r => r.GetUserByIdAsync(_testUserId), Times.Once);
        _mockRepo.Verify(r => r.DeleteUserAsync(_testUserId), Times.Once);

        // Убеждаемся, что других вызовов не было
        _mockRepo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateUserRoleAsync_UnauthorizedAccess_ThrowsException()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByPhoneAsync(ValidPhone))
            .ReturnsAsync(testUser);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _userService.UpdateUserRoleAsync(ValidPhone, (UserRole)999));
    }

    private void VerifyLog(LogLevel level, string message, Exception ex = null)
    {
        _mockLogger.Verify(x => x.Log(
            level,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
            ex,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ), Times.AtLeastOnce);
    }
}