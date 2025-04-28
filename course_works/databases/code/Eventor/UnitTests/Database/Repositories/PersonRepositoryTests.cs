using Eventor.Common.Core;
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

public class PersonRepositoryTests
{
    private readonly Mock<EventorDBContext> _mockContext;
    private readonly Mock<ILogger<PersonRepository>> _mockLogger;
    private readonly PersonRepository _repository;
    private readonly Mock<DbSet<PersonDBModel>> _mockPersonsDbSet;

    public PersonRepositoryTests()
    {
        _mockContext = new Mock<EventorDBContext>();
        _mockLogger = new Mock<ILogger<PersonRepository>>();
        _mockPersonsDbSet = new Mock<DbSet<PersonDBModel>>();

        _mockContext.Setup(c => c.Persons).Returns(_mockPersonsDbSet.Object);
        _repository = new PersonRepository(_mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllPersonsAsync_ReturnsAllPersons()
    {
        // Arrange
        var persons = new List<PersonDBModel>
        {
            new(Guid.NewGuid(), "John Doe", PersonType.VIP, true),
            new(Guid.NewGuid(), "Jane Smith", PersonType.Standart, false)
        };

        var mockSet = SetupMockDbSet(persons.AsQueryable());
        _mockContext.Setup(c => c.Persons).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAllPersonsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "John Doe");
    }

    [Fact]
    public async Task GetPersonByIdAsync_ReturnsCorrectPerson()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var testPerson = new PersonDBModel(personId, "Test Person", PersonType.VIP, true);

        var mockSet = SetupMockDbSet(new List<PersonDBModel> { testPerson }.AsQueryable());
        _mockContext.Setup(c => c.Persons).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetPersonByIdAsync(personId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personId, result.Id);
        Assert.Equal("Test Person", result.Name);
    }

    [Fact]
    public async Task GetAllPersonsByUserAsync_ThrowsException_OnDatabaseError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var brokenQuery = new List<UserPersonDBModel>().AsQueryable();

        var mockSet = new Mock<DbSet<UserPersonDBModel>>();
        mockSet.As<IQueryable<UserPersonDBModel>>()
            .Setup(m => m.Provider)
            .Throws(new Exception("Database failure"));

        _mockContext.Setup(c => c.UsersPersons).Returns(mockSet.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.GetAllPersonsByUserAsync(userId));

        _mockLogger.Verify(log => log.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(userId.ToString())),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPersonByUserAndEventAsync_ThrowsException_OnDatabaseError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var brokenSet = new Mock<DbSet<UserPersonDBModel>>();
        brokenSet.As<IQueryable<UserPersonDBModel>>()
            .Setup(m => m.Provider)
            .Throws(new Exception("Database failure"));

        _mockContext.Setup(c => c.UsersPersons).Returns(brokenSet.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.GetPersonByUserAndEventAsync(userId, eventId));
    }

    [Fact]
    public async Task InsertPersonAsync_AddsNewPersonToDatabase()
    {
        // Arrange
        var newPerson = new Person(Guid.NewGuid(), "New Person", PersonType.Standart, false);

        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        // Act
        await _repository.InsertPersonAsync(newPerson);

        // Assert
        _mockPersonsDbSet.Verify(m => m.AddAsync(
            It.Is<PersonDBModel>(p => p.Id == newPerson.Id),
            It.IsAny<CancellationToken>()  
        ), Times.Once);  

        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdatePersonAsync_UpdatesExistingPerson()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var existingPerson = new PersonDBModel(personId, "Old Name", PersonType.Standart, false);
        var updatedPerson = new Person(personId, "New Name", PersonType.VIP, true);

        var mockSet = SetupMockDbSet(new List<PersonDBModel> { existingPerson }.AsQueryable());
        _mockContext.Setup(c => c.Persons).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        // Act
        await _repository.UpdatePersonAsync(updatedPerson);

        // Assert
        Assert.Equal("New Name", existingPerson.Name);
        Assert.Equal(PersonType.VIP, existingPerson.Type);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task DeletePersonAsync_RemovesPersonFromDatabase()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var testPerson = new PersonDBModel(personId, "Test Person", PersonType.VIP, true);

        var mockSet = SetupMockDbSet(new List<PersonDBModel> { testPerson }.AsQueryable());
        _mockContext.Setup(c => c.Persons).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        // Act
        await _repository.DeletePersonAsync(personId);

        // Assert
        mockSet.Verify(m => m.Remove(
            It.Is<PersonDBModel>(p => p.Id == personId)),
            Times.Once
        );
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    private Mock<DbSet<T>> SetupMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();

        // Настройка для IAsyncEnumerable
        mockSet.As<IAsyncEnumerable<T>>()
               .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
               .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

        // Настройка для IQueryable
        mockSet.As<IQueryable<T>>()
               .Setup(m => m.Provider)
               .Returns(new TestAsyncQueryProvider<T>(data.Provider));

        mockSet.As<IQueryable<T>>()
               .Setup(m => m.Expression)
               .Returns(data.Expression);

        mockSet.As<IQueryable<T>>()
               .Setup(m => m.ElementType)
               .Returns(data.ElementType);

        mockSet.As<IQueryable<T>>()
               .Setup(m => m.GetEnumerator())
               .Returns(data.GetEnumerator());

        return mockSet;
    }
}