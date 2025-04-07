using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Common.Enums;

namespace Eventor.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;
    private readonly Guid _testUserId = Guid.NewGuid();

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockRepo.Object, _mockLogger.Object);
    }

    private User CreateTestUser() => new User(
        _testUserId,
        "Test User",
        "+123456789",
        Gender.Male,
        "hashed_password",
        UserRole.User
    );

    // GetAllUserAsync
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
        Assert.Equal(_testUserId, result[0].Id);
    }

    [Fact]
    public async Task GetAllUserAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllUserAsync())
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<UserServiceException>(
            () => _userService.GetAllUserAsync());
    }

    // GetUserByIdAsync
    [Fact]
    public async Task GetUserByIdAsync_ValidId_ReturnsUser()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);

        // Act
        var result = await _userService.GetUserByIdAsync(_testUserId);

        // Assert
        Assert.Equal(_testUserId, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _userService.GetUserByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetUserByIdAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<UserServiceException>(
            () => _userService.GetUserByIdAsync(_testUserId));
    }

    [Fact]
    public async Task GetUserByPhoneAsync_ValidPhone_ReturnsUser()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByPhoneAsync(testUser.Phone))
                 .ReturnsAsync(testUser);

        // Act
        var result = await _userService.GetUserByPhoneAsync(testUser.Phone);

        // Assert
        Assert.Equal(testUser.Phone, result.Phone);
    }

    [Fact]
    public async Task GetUserByPhoneAsync_InvalidPhone_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetUserByPhoneAsync(It.IsAny<string>()))
                 .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _userService.GetUserByPhoneAsync("+invalid_phone"));
    }

    // AddUserAsync
    [Fact]
    public async Task AddUserAsync_ValidUser_SavesSuccessfully()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.InsertUserAsync(testUser))
            .Returns(Task.CompletedTask);

        // Act
        await _userService.AddUserAsync(testUser);

        // Assert
        _mockRepo.Verify(r => r.InsertUserAsync(testUser), Times.Once);
    }

    [Fact]
    public async Task AddUserAsync_DbException_ThrowsCreateException()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.InsertUserAsync(testUser))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<UserCreateException>(
            () => _userService.AddUserAsync(testUser));
    }

    // UpdateUserAsync
    [Fact]
    public async Task UpdateUserAsync_ValidUser_UpdatesSuccessfully()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);

        // Act
        await _userService.UpdateUserAsync(testUser);

        // Assert
        _mockRepo.Verify(r => r.UpdateUserAsync(testUser), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_NonExistingUser_ThrowsNotFoundException()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _userService.UpdateUserAsync(testUser));
    }

    [Fact]
    public async Task UpdateUserAsync_DbException_ThrowsUpdateException()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);
        _mockRepo.Setup(r => r.UpdateUserAsync(testUser))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<UserUpdateException>(
            () => _userService.UpdateUserAsync(testUser));
    }

    // DeleteUserAsync
    [Fact]
    public async Task DeleteUserAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);

        // Act
        await _userService.DeleteUserAsync(_testUserId);

        // Assert
        _mockRepo.Verify(r => r.DeleteUserAsync(_testUserId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_NonExistingUser_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _userService.DeleteUserAsync(_testUserId));
    }

    [Fact]
    public async Task DeleteUserAsync_DbException_ThrowsDeleteException()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockRepo.Setup(r => r.GetUserByIdAsync(_testUserId))
            .ReturnsAsync(testUser);
        _mockRepo.Setup(r => r.DeleteUserAsync(_testUserId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<UserDeleteException>(
            () => _userService.DeleteUserAsync(_testUserId));
    }

}