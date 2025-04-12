using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

    private Person CreateValidTestPerson() => new Person(
        _testPersonId,
        "John Doe",
        PersonType.Standart, // Исправлено с Standart на Standard
        true
    );

    // GetAllPersonsAsync
    [Fact]
    public async Task GetAllPersonsAsync_ReturnsPersons()
    {
        // Arrange
        var persons = new List<Person> { CreateValidTestPerson() };
        _mockRepo.Setup(r => r.GetAllPersonsAsync()).ReturnsAsync(persons);

        // Act
        var result = await _personService.GetAllPersonsAsync();

        // Assert
        Assert.Single(result);
        _mockRepo.Verify(r => r.GetAllPersonsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllPersonsAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllPersonsAsync())
            .ThrowsAsync(new DbUpdateException("Database error", new Exception()));

        // Act & Assert
        await Assert.ThrowsAsync<PersonServiceException>(() => _personService.GetAllPersonsAsync());
        _mockLogger.VerifyLog(LogLevel.Error, "Error retrieving persons");
    }

    // GetPersonByIdAsync
    [Fact]
    public async Task GetPersonByIdAsync_ValidId_ReturnsPerson()
    {
        // Arrange
        var testPerson = CreateValidTestPerson();
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId)).ReturnsAsync(testPerson);

        // Act
        var result = await _personService.GetPersonByIdAsync(_testPersonId);

        // Assert
        Assert.Equal(_testPersonId, result.Id);
        _mockRepo.Verify(r => r.GetPersonByIdAsync(_testPersonId), Times.Once);
    }

    [Fact]
    public async Task GetPersonByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId))
            .ReturnsAsync((Person)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _personService.GetPersonByIdAsync(_testPersonId));

        // Проверка логов
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Person {_testPersonId} not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }

    // AddPersonAsync
    [Fact]
    public async Task AddPersonAsync_InvalidData_ThrowsCreateException()
    {
        // Arrange
        var invalidPerson = new Person(Guid.NewGuid(), "", PersonType.Standart, false);

        // Act & Assert
        await Assert.ThrowsAsync<PersonCreateException>(
            () => _personService.AddPersonAsync(invalidPerson));

        _mockLogger.VerifyLog(LogLevel.Error, "Validation error creating person");
    }

    [Fact]
    public async Task AddPersonAsync_DuplicateId_ThrowsCreateException()
    {
        // Arrange
        var testPerson = CreateValidTestPerson();
        _mockRepo.Setup(r => r.InsertPersonAsync(testPerson))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<PersonCreateException>(
            () => _personService.AddPersonAsync(testPerson));
    }

    // UpdatePersonAsync
    [Fact]
    public async Task UpdatePersonAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        // Arrange
        var testPerson = CreateValidTestPerson();
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _personService.UpdatePersonAsync(testPerson));
    }

    // DeletePersonAsync
    [Fact]
    public async Task DeletePersonAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetPersonByIdAsync(_testPersonId)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _personService.DeletePersonAsync(_testPersonId));
    }
}