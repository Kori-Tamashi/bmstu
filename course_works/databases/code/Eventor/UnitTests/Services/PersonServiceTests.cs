using Eventor.Common.Core;
using Eventor.Database.Core;
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

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _mockRepo;
    private readonly Mock<ILogger<PersonService>> _mockLogger;
    private readonly PersonService _personService;
    private readonly Guid _testPersonId = Guid.NewGuid();
    private readonly Guid _testDayId = Guid.NewGuid();
    private readonly Guid _testEventId = Guid.NewGuid();

    public PersonServiceTests()
    {
        _mockRepo = new Mock<IPersonRepository>();
        _mockLogger = new Mock<ILogger<PersonService>>();
        _personService = new PersonService(_mockRepo.Object, _mockLogger.Object);
    }

    private Person CreateTestPerson() => new Person(
        _testPersonId,
        "John Doe",
        PersonType.Standart,
        true
    );

    // GetAllPersonsAsync
    [Fact]
    public async Task GetAllPersonsAsync_ReturnsPersons()
    {
        // Arrange
        var persons = new List<Person> { CreateTestPerson() };
        _mockRepo.Setup(r => r.GetAllPersonsAsync()).ReturnsAsync(persons);

        // Act
        var result = await _personService.GetAllPersonsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testPersonId, result[0].Id);
    }

    [Fact]
    public async Task GetAllPersonsAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllPersonsAsync())
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonServiceException>(
            () => _personService.GetAllPersonsAsync());
    }

    // GetAllPersonsByDayAsync
    [Fact]
    public async Task GetAllPersonsByDayAsync_ReturnsPersons()
    {
        // Arrange
        var persons = new List<Person> { CreateTestPerson() };
        _mockRepo.Setup(r => r.GetAllPersonsByDayAsync(_testDayId))
            .ReturnsAsync(persons);

        // Act
        var result = await _personService.GetAllPersonsByDayAsync(_testDayId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testPersonId, result[0].Id);
    }

    [Fact]
    public async Task GetAllPersonsByDayAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllPersonsByDayAsync(_testDayId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonServiceException>(
            () => _personService.GetAllPersonsByDayAsync(_testDayId));
    }

    // GetAllPersonsByEventAsync
    [Fact]
    public async Task GetAllPersonsByEventAsync_ReturnsPersons()
    {
        // Arrange
        var persons = new List<Person> { CreateTestPerson() };
        _mockRepo.Setup(r => r.GetAllPersonsByEventAsync(_testEventId))
            .ReturnsAsync(persons);

        // Act
        var result = await _personService.GetAllPersonsByEventAsync(_testEventId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testPersonId, result[0].Id);
    }

    [Fact]
    public async Task GetAllPersonsByEventAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllPersonsByEventAsync(_testEventId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonServiceException>(
            () => _personService.GetAllPersonsByEventAsync(_testEventId));
    }

    // GetPersonByIdAsync
    [Fact]
    public async Task GetPersonByIdAsync_ValidId_ReturnsPerson()
    {
        // Arrange
        var testPerson = CreateTestPerson();
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId))
            .ReturnsAsync(testPerson);

        // Act
        var result = await _personService.GetPersonByIdAsync(_testPersonId);

        // Assert
        Assert.Equal(_testPersonId, result.Id);
    }

    [Fact]
    public async Task GetPersonByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetPersonByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _personService.GetPersonByIdAsync(Guid.NewGuid()));
    }

    // AddPersonAsync
    [Fact]
    public async Task AddPersonAsync_ValidPerson_SavesSuccessfully()
    {
        // Arrange
        var testPerson = CreateTestPerson();
        _mockRepo.Setup(r => r.InsertPersonAsync(testPerson)).Returns(Task.CompletedTask);

        // Act
        await _personService.AddPersonAsync(testPerson);

        // Assert
        _mockRepo.Verify(r => r.InsertPersonAsync(testPerson), Times.Once);
    }

    [Fact]
    public async Task AddPersonAsync_DbException_ThrowsCreateException()
    {
        // Arrange
        var testPerson = CreateTestPerson();
        _mockRepo.Setup(r => r.InsertPersonAsync(testPerson))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonCreateException>(
            () => _personService.AddPersonAsync(testPerson));
    }

    // UpdatePersonAsync
    [Fact]
    public async Task UpdatePersonAsync_ValidPerson_UpdatesSuccessfully()
    {
        // Arrange
        var testPerson = CreateTestPerson();
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId))
            .ReturnsAsync(testPerson);

        // Act
        await _personService.UpdatePersonAsync(testPerson);

        // Assert
        _mockRepo.Verify(r => r.UpdatePersonAsync(testPerson), Times.Once);
    }

    [Fact]
    public async Task UpdatePersonAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        // Arrange
        var testPerson = CreateTestPerson();
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _personService.UpdatePersonAsync(testPerson));
    }

    // DeletePersonAsync
    [Fact]
    public async Task DeletePersonAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeletePersonAsync(_testPersonId))
            .Returns(Task.CompletedTask);

        // Act
        await _personService.DeletePersonAsync(_testPersonId);

        // Assert
        _mockRepo.Verify(r => r.DeletePersonAsync(_testPersonId), Times.Once);
    }

    [Fact]
    public async Task DeletePersonAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeletePersonAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _personService.DeletePersonAsync(Guid.NewGuid()));
    }
}