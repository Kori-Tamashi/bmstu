using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Eventor.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Eventor.Common.Enums;

namespace Eventor.Tests.Database.Repositories;

/// <summary>
/// Набор тестов для проверки функциональности репозитория пользователей
/// </summary>
public class UserRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly UserRepository _repository;
    private readonly Mock<DbSet<UserDBModel>> _mockUsersDbSet;

    public UserRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockUsersDbSet = new Mock<DbSet<UserDBModel>>();

        _mockContext.Setup(c => c.Users).Returns(_mockUsersDbSet.Object);
        _repository = new UserRepository(_mockContext.Object);
    }

    /// <summary>
    /// Проверяет, что метод GetAllUserAsync возвращает всех пользователей
    /// </summary>
    [Fact]
    public async Task GetAllUserAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<UserDBModel>
        {
            new UserDBModel(Guid.NewGuid(), "User1", "+79111111111", Gender.Male, "hash1", UserRole.User),
            new UserDBModel(Guid.NewGuid(), "User2", "+79222222222", Gender.Female, "hash2", UserRole.Administrator)
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

    /// <summary>
    /// Проверяет, что метод GetUserByIdAsync возвращает пользователя по ID
    /// </summary>
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
        Assert.Equal("TestUser", result.Name);
    }

    /// <summary>
    /// Проверяет, что метод GetUserByIdAsync возвращает null при отсутствии пользователя
    /// </summary>
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

    /// <summary>
    /// Проверяет, что метод GetUserByPhoneAsync возвращает пользователя по номеру телефона
    /// </summary>
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

    /// <summary>
    /// Проверяет, что метод InsertUserAsync добавляет пользователя в контекст
    /// </summary>
    [Fact]
    public async Task InsertUserAsync_AddsUserToContext()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "NewUser", "+79111111111", Gender.Male, "hash", UserRole.User);
        var mockSet = new Mock<DbSet<UserDBModel>>();

        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.InsertUserAsync(user);

        // Assert
        mockSet.Verify(m => m.AddAsync(It.IsAny<UserDBModel>(), default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет, что метод UpdateUserAsync обновляет данные пользователя
    /// </summary>
    [Fact]
    public async Task UpdateUserAsync_UpdatesUserProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new UserDBModel(userId, "OldName", "+79000000000", Gender.Male, "oldHash", UserRole.User);
        var updatedUser = new User(userId, "NewName", "+79111111111", Gender.Female, "newHash", UserRole.Administrator);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { existingUser }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        await _repository.UpdateUserAsync(updatedUser);

        // Assert
        Assert.Equal("NewName", existingUser.Name);
        Assert.Equal("+79111111111", existingUser.Phone);
        Assert.Equal(Gender.Female, existingUser.Gender);
        Assert.Equal("newHash", existingUser.PasswordHash);
        Assert.Equal(UserRole.Administrator, existingUser.Role);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет, что метод DeleteUserAsync удаляет пользователя из контекста
    /// </summary>
    [Fact]
    public async Task DeleteUserAsync_RemovesUserFromContext()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDBModel(userId, "TestUser", "+79000000000", Gender.Male, "hash", UserRole.User);

        var mockSet = SetupMockDbSet(new List<UserDBModel> { user }.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        await _repository.DeleteUserAsync(userId);

        // Assert
        mockSet.Verify(m => m.Remove(user), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    /// <summary>
    /// Проверяет конвертацию из UserDBModel в User
    /// </summary>
    [Fact]
    public void UserConverter_ConvertsDBModelToCoreCorrectly()
    {
        // Arrange
        var dbModel = new UserDBModel(
            Guid.NewGuid(),
            "TestUser",
            "+79000000000",
            Gender.Male,
            "testHash",
            UserRole.Administrator
        );

        // Act
        var result = UserConverter.ConvertDBToCore(dbModel);

        // Assert
        Assert.Equal(dbModel.Id, result.Id);
        Assert.Equal(dbModel.Name, result.Name);
        Assert.Equal(dbModel.Phone, result.Phone);
        Assert.Equal(dbModel.Gender, result.Gender);
        Assert.Equal(dbModel.PasswordHash, result.PasswordHash);
        Assert.Equal(dbModel.Role, result.Role);
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