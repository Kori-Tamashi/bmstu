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

namespace Eventor.Tests.Services;

public class DayServiceTests
{
    private readonly Mock<IDayRepository> _mockRepo;
    private readonly Mock<ILogger<DayService>> _mockLogger;
    private readonly DayService _dayService;
    private readonly Guid _testEventId = Guid.NewGuid();
    private readonly Guid _testDayId = Guid.NewGuid();

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

    // GetAllDaysAsync
    [Fact]
    public async Task GetAllDaysAsync_ReturnsDays()
    {
        // Arrange
        var days = new List<Day> { CreateTestDay() };
        _mockRepo.Setup(r => r.GetAllDaysAsync()).ReturnsAsync(days);

        // Act
        var result = await _dayService.GetAllDaysAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(_testDayId, result[0].Id);
    }

    [Fact]
    public async Task GetAllDaysAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllDaysAsync())
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<DayServiceException>(
            () => _dayService.GetAllDaysAsync());
    }

    // GetAllDaysByEventAsync
    [Fact]
    public async Task GetAllDaysByEventAsync_ReturnsDays()
    {
        // Arrange
        var days = new List<Day> { CreateTestDay() };
        _mockRepo.Setup(r => r.GetAllDaysByEventAsync(_testEventId))
            .ReturnsAsync(days);

        // Act
        var result = await _dayService.GetAllDaysByEventAsync(_testEventId);

        // Assert
        Assert.Single(result);
        Assert.Equal(_testDayId, result[0].Id);
    }

    [Fact]
    public async Task GetAllDaysByEventAsync_DbException_ThrowsServiceException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllDaysByEventAsync(_testEventId))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<DayServiceException>(
            () => _dayService.GetAllDaysByEventAsync(_testEventId));
    }

    // GetDayByIdAsync
    [Fact]
    public async Task GetDayByIdAsync_ValidId_ReturnsDay()
    {
        // Arrange
        var testDay = CreateTestDay();
        _mockRepo.Setup(r => r.GetDayByIdAsync(_testDayId))
            .ReturnsAsync(testDay);

        // Act
        var result = await _dayService.GetDayByIdAsync(_testDayId);

        // Assert
        Assert.Equal(_testDayId, result.Id);
    }

    [Fact]
    public async Task GetDayByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetDayByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<DayNotFoundException>(
            () => _dayService.GetDayByIdAsync(Guid.NewGuid()));
    }

    // AddDayAsync
    [Fact]
    public async Task AddDayAsync_ValidDay_SavesSuccessfully()
    {
        // Arrange
        var testDay = CreateTestDay();
        _mockRepo.Setup(r => r.InsertDayAsync(testDay)).Returns(Task.CompletedTask);

        // Act
        await _dayService.AddDayAsync(testDay);

        // Assert
        _mockRepo.Verify(r => r.InsertDayAsync(testDay), Times.Once);
    }

    [Fact]
    public async Task AddDayAsync_DbException_ThrowsCreateException()
    {
        // Arrange
        var testDay = CreateTestDay();
        _mockRepo.Setup(r => r.InsertDayAsync(testDay))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<DayCreateException>(
            () => _dayService.AddDayAsync(testDay));
    }

    // UpdateDayAsync
    [Fact]
    public async Task UpdateDayAsync_ValidDay_UpdatesSuccessfully()
    {
        // Arrange
        var testDay = CreateTestDay();
        _mockRepo.Setup(r => r.GetDayByIdAsync(_testDayId))
            .ReturnsAsync(testDay);

        // Act
        await _dayService.UpdateDayAsync(testDay);

        // Assert
        _mockRepo.Verify(r => r.UpdateDayAsync(testDay), Times.Once);
    }

    [Fact]
    public async Task UpdateDayAsync_NonExistingDay_ThrowsNotFoundException()
    {
        // Arrange
        var testDay = CreateTestDay();
        _mockRepo.Setup(r => r.GetDayByIdAsync(_testDayId))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<DayNotFoundException>(
            () => _dayService.UpdateDayAsync(testDay));
    }

    // DeleteDayAsync
    [Fact]
    public async Task DeleteDayAsync_ValidId_DeletesSuccessfully()
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
    public async Task DeleteDayAsync_NonExistingDay_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteDayAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<DayNotFoundException>(
            () => _dayService.DeleteDayAsync(Guid.NewGuid()));
    }
}