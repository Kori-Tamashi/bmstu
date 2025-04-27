using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Common.Enums;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Eventor.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly Mock<ILogger<UserRepository>> _mockLogger;
    private readonly UserRepository _repository;
    private readonly Mock<DbSet<UserDBModel>> _mockUsersDbSet;

    public UserRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockLogger = new Mock<ILogger<UserRepository>>();
        _mockUsersDbSet = new Mock<DbSet<UserDBModel>>();

        _mockContext.Setup(c => c.Users).Returns(_mockUsersDbSet.Object);
        _repository = new UserRepository(_mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllUserAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<UserDBModel>
        {
            new UserDBModel(Guid.NewGuid(), "User1", "+79111111111", Gender.Male, "hash1", UserRole.User),
            new UserDBModel(Guid.NewGuid(), "User2", "+79222222222", Gender.Female, "hash2", UserRole.Admin)
        };

        var mockSet = SetupMockDbSet(users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAllUserAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Name == "User1");
        Assert.Contains(result, u => u.Name == "User2");
    }

    [Fact]
    public async Task GetAllUserAsync_ThrowsOnDatabaseError()
    {
        // Arrange
        _mockContext.Setup(c => c.Users).Throws<Exception>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetAllUserAsync());
        _mockLogger.VerifyLog(LogLevel.Error, "Ошибка получения списка пользователей");
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDBModel(userId, "TestUser", "+79000000000", Gender.Male, "hash", UserRole.User);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { user }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsNullForNonExistingUser()
    {
        // Arrange
        var mockSet = SetupMockDbSet(new List<UserDBModel>().AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetUserByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ThrowsOnDatabaseError()
    {
        // Arrange
        _mockContext.Setup(c => c.Users).Throws<Exception>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetUserByIdAsync(Guid.NewGuid()));
        _mockLogger.VerifyLog(LogLevel.Error, "Ошибка получения пользователя");
    }

    [Fact]
    public async Task GetUserByPhoneAsync_ReturnsUser()
    {
        // Arrange
        var phone = "+79000000000";
        var user = new UserDBModel(Guid.NewGuid(), "TestUser", phone, Gender.Male, "hash", UserRole.User);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { user }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetUserByPhoneAsync(phone);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phone, result.Phone);
    }

    [Fact]
    public async Task GetUserByPhoneAsync_ThrowsOnDatabaseError()
    {
        // Arrange
        _mockContext.Setup(c => c.Users).Throws<Exception>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetUserByPhoneAsync("+79000000000"));
        _mockLogger.VerifyLog(LogLevel.Error, "Ошибка поиска пользователя по телефону");
    }

    [Fact]
    public async Task InsertUserAsync_AddsUserToContext()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "NewUser", "+79111111111", Gender.Male, "hash", UserRole.User);

        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.InsertUserAsync(user);

        // Assert
        _mockUsersDbSet.Verify(m => m.AddAsync(It.IsAny<UserDBModel>(), default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task InsertUserAsync_ThrowsOnDatabaseError()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "NewUser", "+79111111111", Gender.Male, "hash", UserRole.User);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).Throws<DbUpdateException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.InsertUserAsync(user));
        _mockLogger.VerifyLog(LogLevel.Error, "Ошибка создания пользователя");
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUserProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new UserDBModel(userId, "OldName", "+79000000000", Gender.Male, "oldHash", UserRole.User);
        var updatedUser = new User(userId, "NewName", "+79111111111", Gender.Female, "newHash", UserRole.Admin);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { existingUser }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.UpdateUserAsync(updatedUser);

        // Assert
        Assert.Equal("NewName", existingUser.Name);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_DoesNothingWhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updatedUser = new User(userId, "NewName", "+79111111111", Gender.Female, "newHash", UserRole.Admin);

        var mockSet = SetupMockDbSet(new List<UserDBModel>().AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        await _repository.UpdateUserAsync(updatedUser);

        // Assert
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_ThrowsOnDatabaseError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new UserDBModel(userId, "OldName", "+79000000000", Gender.Male, "oldHash", UserRole.User);
        var updatedUser = new User(userId, "NewName", "+79111111111", Gender.Female, "newHash", UserRole.Admin);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { existingUser }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).Throws<DbUpdateException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateUserAsync(updatedUser));
        _mockLogger.VerifyLog(LogLevel.Error, "Ошибка обновления пользователя");
    }

    [Fact]
    public async Task DeleteUserAsync_RemovesUserFromContext()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDBModel(userId, "TestUser", "+79000000000", Gender.Male, "hash", UserRole.User);

        // Настраиваем мок для FirstOrDefaultAsync
        var mockSet = new Mock<DbSet<UserDBModel>>();
        mockSet.As<IQueryable<UserDBModel>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<UserDBModel>(new List<UserDBModel> { user }.AsQueryable().Provider));

        mockSet.As<IQueryable<UserDBModel>>()
            .Setup(m => m.Expression)
            .Returns(new List<UserDBModel> { user }.AsQueryable().Expression);

        mockSet.As<IQueryable<UserDBModel>>()
            .Setup(m => m.ElementType)
            .Returns(new List<UserDBModel> { user }.AsQueryable().ElementType);

        mockSet.As<IQueryable<UserDBModel>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => new List<UserDBModel> { user }.GetEnumerator());

        mockSet.As<IAsyncEnumerable<UserDBModel>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<UserDBModel>(new List<UserDBModel> { user }.GetEnumerator()));

        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.DeleteUserAsync(userId);

        // Assert
        mockSet.Verify(m => m.Remove(It.Is<UserDBModel>(u => u.Id == userId)), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }


    [Fact]
    public async Task DeleteUserAsync_DoesNothingWhenUserNotFound()
    {
        // Arrange
        var mockSet = SetupMockDbSet(new List<UserDBModel>().AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        await _repository.DeleteUserAsync(Guid.NewGuid());

        // Assert
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_ThrowsOnDatabaseError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDBModel(userId, "TestUser", "+79000000000", Gender.Male, "hash", UserRole.User);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { user }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).Throws<DbUpdateException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.DeleteUserAsync(userId));
        _mockLogger.VerifyLog(LogLevel.Error, "Ошибка удаления пользователя");
    }

    private Mock<DbSet<T>> SetupMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(data.Provider));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Expression).Returns(data.Expression);

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.ElementType).Returns(data.ElementType);

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        return mockSet;
    }
}

public static class LoggerExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce);
    }
}