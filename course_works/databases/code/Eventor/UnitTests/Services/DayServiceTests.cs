using Eventor.Common.Core;
using Eventor.Services.Exceptions;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventor.Database.Core;
using Eventor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eventor.Tests.Services;

public class DayServiceTests
{
    private readonly Mock<IDayRepository> _mockRepo;
    private readonly Mock<ILogger<DayService>> _mockLogger;
    private readonly DayService _dayService;
    private readonly Guid _testEventId = Guid.NewGuid();
    private readonly Guid _testDayId = Guid.NewGuid();
    private readonly Guid _nonExistingDayId = Guid.NewGuid();

    public DayServiceTests()
    {
        _mockRepo = new Mock<IDayRepository>();
        _mockLogger = new Mock<ILogger<DayService>>();
        _dayService = new DayService(_mockRepo.Object, _mockLogger.Object);
    }

    private Day CreateTestDay() => new Day(
        _testDayId,
        Guid.NewGuid(),
        "Test Day",
        1,
        "Test Description",
        100.0
    );

    [Fact]
    public async Task GetAllDaysAsync_ReturnsDays_WhenRepositoryReturnsData()
    {
        // Arrange
        var expectedDays = new List<Day> { CreateTestDay() };
        _mockRepo.Setup(r => r.GetAllDaysAsync()).ReturnsAsync(expectedDays);

        // Act
        var result = await _dayService.GetAllDaysAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testDayId, result[0].Id);
        _mockRepo.Verify(r => r.GetAllDaysAsync(), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    [InlineData(typeof(Exception))]
    public async Task GetAllDaysAsync_ThrowsServiceException_OnAnyError(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test exception");
        _mockRepo.Setup(r => r.GetAllDaysAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<DayServiceException>(
            () => _dayService.GetAllDaysAsync());
    }

    [Fact]
    public async Task GetAllDaysByEventAsync_ReturnsDays_ForValidEventId()
    {
        // Arrange
        var expectedDays = new List<Day> { CreateTestDay() };
        _mockRepo.Setup(r => r.GetAllDaysByEventAsync(_testEventId))
            .ReturnsAsync(expectedDays);

        // Act
        var result = await _dayService.GetAllDaysByEventAsync(_testEventId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testDayId, result[0].Id);
        _mockRepo.Verify(r => r.GetAllDaysByEventAsync(_testEventId), Times.Once);
    }

    [Fact]
    public async Task GetDayByIdAsync_ReturnsDay_WhenExists()
    {
        // Arrange
        var expectedDay = CreateTestDay();
        _mockRepo.Setup(r => r.GetDayByIdAsync(_testDayId))
            .ReturnsAsync(expectedDay);

        // Act
        var result = await _dayService.GetDayByIdAsync(_testDayId);

        // Assert
        Assert.Equal(expectedDay.Id, result.Id);
        _mockRepo.Verify(r => r.GetDayByIdAsync(_testDayId), Times.Once);
    }

    [Fact]
    public async Task GetDayByIdAsync_ThrowsNotFoundException_WhenDayNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetDayByIdAsync(_nonExistingDayId))
            .ThrowsAsync(new ArgumentException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<DayNotFoundException>(
            () => _dayService.GetDayByIdAsync(_nonExistingDayId));
    }

    [Fact]
    public async Task AddDayAsync_SavesDay_WhenValid()
    {
        // Arrange
        var newDay = CreateTestDay();
        _mockRepo.Setup(r => r.InsertDayAsync(newDay))
            .Returns(Task.CompletedTask);

        // Act
        await _dayService.AddDayAsync(newDay);

        // Assert
        _mockRepo.Verify(r => r.InsertDayAsync(It.Is<Day>(d =>
            d.Id == newDay.Id &&
            d.Name == newDay.Name)),
            Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task AddDayAsync_ThrowsCreateException_OnFailure(Type exceptionType)
    {
        // Arrange
        var newDay = CreateTestDay();
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test exception");
        _mockRepo.Setup(r => r.InsertDayAsync(newDay)).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<DayCreateException>(
            () => _dayService.AddDayAsync(newDay));
    }

    [Fact]
    public async Task AddPersonToDayAsync_CallsRepositoryWithValidParameters()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var testDay = CreateTestDay();

        _mockRepo.Setup(r => r.GetDayByIdAsync(dayId)).ReturnsAsync(testDay);
        _mockRepo.Setup(r => r.InsertPersonToDayAsync(personId, dayId)).Returns(Task.CompletedTask);

        // Act
        await _dayService.AddPersonToDayAsync(personId, dayId);

        // Assert
        _mockRepo.Verify(r => r.GetDayByIdAsync(dayId), Times.Once);
        _mockRepo.Verify(r => r.InsertPersonToDayAsync(personId, dayId), Times.Once);
    }

    [Fact]
    public async Task AddPersonToDayAsync_ThrowsPersonNotFound_OnFkViolation()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var testDay = CreateTestDay();
        var dbEx = new DbUpdateException("FK error", new Exception("FK_PersonsDays_Persons"));

        _mockRepo.Setup(r => r.GetDayByIdAsync(dayId)).ReturnsAsync(testDay);
        _mockRepo.Setup(r => r.InsertPersonToDayAsync(personId, dayId)).ThrowsAsync(dbEx);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _dayService.AddPersonToDayAsync(personId, dayId));

        Assert.Contains(personId.ToString(), ex.Message);
    }

    [Fact]
    public async Task AddPersonToDayAsync_ThrowsConflict_OnDuplicateEntry()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var testDay = CreateTestDay();
        var conflictEx = new InvalidOperationException("Duplicate key");

        _mockRepo.Setup(r => r.GetDayByIdAsync(dayId)).ReturnsAsync(testDay);
        _mockRepo.Setup(r => r.InsertPersonToDayAsync(personId, dayId)).ThrowsAsync(conflictEx);

        // Act & Assert
        await Assert.ThrowsAsync<DayConflictException>(() =>
            _dayService.AddPersonToDayAsync(personId, dayId));
    }

    [Fact]
    public async Task AddPersonToDayAsync_ThrowsUpdateException_OnGeneralDbError()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var testDay = CreateTestDay();
        var dbEx = new DbUpdateException("Database failure", new Exception());

        _mockRepo.Setup(r => r.GetDayByIdAsync(dayId)).ReturnsAsync(testDay);
        _mockRepo.Setup(r => r.InsertPersonToDayAsync(personId, dayId)).ThrowsAsync(dbEx);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DayUpdateException>(() =>
            _dayService.AddPersonToDayAsync(personId, dayId));

        Assert.Contains(dayId.ToString(), ex.Message);
    }

    [Fact]
    public async Task AddPersonToDayAsync_ThrowsServiceException_OnUnknownError()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var dayId = Guid.NewGuid();
        var testDay = CreateTestDay();
        var unexpectedEx = new Exception("Unknown failure");

        _mockRepo.Setup(r => r.GetDayByIdAsync(dayId)).ReturnsAsync(testDay);
        _mockRepo.Setup(r => r.InsertPersonToDayAsync(personId, dayId)).ThrowsAsync(unexpectedEx);

        // Act & Assert
        await Assert.ThrowsAsync<DayServiceException>(() =>
            _dayService.AddPersonToDayAsync(personId, dayId));
    }

    [Fact]
    public async Task UpdateDayAsync_UpdatesExistingDay_WhenValid()
    {
        // Arrange
        var existingDay = CreateTestDay();
        var updatedDay = existingDay;
        updatedDay.Name = "Updated Name";

        _mockRepo.Setup(r => r.GetDayByIdAsync(existingDay.Id))
            .ReturnsAsync(existingDay);
        _mockRepo.Setup(r => r.UpdateDayAsync(updatedDay))
            .Returns(Task.CompletedTask);

        // Act
        await _dayService.UpdateDayAsync(updatedDay);

        // Assert
        _mockRepo.Verify(r => r.UpdateDayAsync(It.Is<Day>(d =>
            d.Id == updatedDay.Id &&
            d.Name == "Updated Name")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateDayAsync_ThrowsNotFoundException_WhenDayNotExists()
    {
        // Arrange
        var nonExistingDay = CreateTestDay();
        nonExistingDay.Id = _nonExistingDayId;
        _mockRepo.Setup(r => r.GetDayByIdAsync(_nonExistingDayId))
            .ThrowsAsync(new ArgumentException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<DayNotFoundException>(
            () => _dayService.UpdateDayAsync(nonExistingDay));
    }

    [Fact]
    public async Task DeleteDayAsync_DeletesExistingDay_WhenValid()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteDayAsync(_testDayId))
            .Returns(Task.CompletedTask);

        // Act
        await _dayService.DeleteDayAsync(_testDayId);

        // Assert
        _mockRepo.Verify(r => r.DeleteDayAsync(_testDayId), Times.Once);
    }

    [Fact]
    public async Task DeleteDayAsync_ThrowsNotFoundException_WhenDayNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteDayAsync(_nonExistingDayId))
            .ThrowsAsync(new ArgumentException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<DayNotFoundException>(
            () => _dayService.DeleteDayAsync(_nonExistingDayId));
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task DeleteDayAsync_ThrowsDeleteException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test exception");
        _mockRepo.Setup(r => r.DeleteDayAsync(_testDayId)).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<DayDeleteException>(
            () => _dayService.DeleteDayAsync(_testDayId));
    }
}