using Eventor.Common.Core;
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

namespace Eventor.Tests.Services;

public class LocationServiceTests
{
    private readonly Mock<ILocationRepository> _mockRepo;
    private readonly LocationService _locationService;
    private readonly Guid _testLocationId = Guid.NewGuid();
    private readonly Guid _nonExistingLocationId = Guid.NewGuid();

    public LocationServiceTests()
    {
        _mockRepo = new Mock<ILocationRepository>();
        var logger = Mock.Of<ILogger<LocationService>>();
        _locationService = new LocationService(_mockRepo.Object, logger);
    }

    private Location CreateTestLocation() => new Location(
        _testLocationId,
        "Test Location",
        "Test Description",
        100.0
    );

    [Fact]
    public async Task GetAllLocationsAsync_ReturnsLocations_WhenDataExists()
    {
        // Arrange
        var expectedLocations = new List<Location> { CreateTestLocation() };
        _mockRepo.Setup(r => r.GetAllLocationsAsync()).ReturnsAsync(expectedLocations);

        // Act
        var result = await _locationService.GetAllLocationsAsync();

        // Assert
        Assert.Equal(expectedLocations, result);
        _mockRepo.Verify(r => r.GetAllLocationsAsync(), Times.Once);
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task GetAllLocationsAsync_ThrowsServiceException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.GetAllLocationsAsync()).ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<LocationServiceException>(
            () => _locationService.GetAllLocationsAsync());
    }

    [Fact]
    public async Task GetLocationByIdAsync_ReturnsLocation_WhenExists()
    {
        // Arrange
        var expectedLocation = CreateTestLocation();
        _mockRepo.Setup(r => r.GetLocationByIdAsync(_testLocationId))
            .ReturnsAsync(expectedLocation);

        // Act
        var result = await _locationService.GetLocationByIdAsync(_testLocationId);

        // Assert
        Assert.Equal(expectedLocation.Price, result.Price);
        _mockRepo.Verify(r => r.GetLocationByIdAsync(_testLocationId), Times.Once);
    }

    [Fact]
    public async Task GetLocationByIdAsync_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetLocationByIdAsync(_nonExistingLocationId))
            .ReturnsAsync((Location)null);

        // Act & Assert
        await Assert.ThrowsAsync<LocationNotFoundException>(
            () => _locationService.GetLocationByIdAsync(_nonExistingLocationId));
    }

    [Fact]
    public async Task AddLocationAsync_SavesLocation_WithCorrectData()
    {
        // Arrange
        var newLocation = CreateTestLocation();
        _mockRepo.Setup(r => r.InsertLocationAsync(newLocation))
            .Returns(Task.CompletedTask);

        // Act
        await _locationService.AddLocationAsync(newLocation);

        // Assert
        _mockRepo.Verify(r => r.InsertLocationAsync(It.Is<Location>(l =>
            l.Id == newLocation.Id &&
            l.Name == "Test Location")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateLocationAsync_UpdatesExistingLocation_WhenValid()
    {
        // Arrange
        var existingLocation = CreateTestLocation();
        var updatedLocation = existingLocation;
        updatedLocation.Price = 150;

        _mockRepo.Setup(r => r.GetLocationByIdAsync(_testLocationId))
            .ReturnsAsync(existingLocation);
        _mockRepo.Setup(r => r.UpdateLocationAsync(updatedLocation))
            .Returns(Task.CompletedTask);

        // Act
        await _locationService.UpdateLocationAsync(updatedLocation);

        // Assert
        _mockRepo.Verify(r => r.UpdateLocationAsync(It.Is<Location>(l =>
            l.Id == _testLocationId &&
            l.Price == 150.0)),
            Times.Once);
    }

    [Fact]
    public async Task UpdateLocationAsync_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        var location = CreateTestLocation();
        location.Id = _nonExistingLocationId;
        _mockRepo.Setup(r => r.GetLocationByIdAsync(_nonExistingLocationId))
            .ReturnsAsync((Location)null);

        // Act & Assert
        await Assert.ThrowsAsync<LocationNotFoundException>(
            () => _locationService.UpdateLocationAsync(location));
    }

    [Fact]
    public async Task DeleteLocationAsync_DeletesLocation_WhenExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetLocationByIdAsync(_testLocationId))
            .ReturnsAsync(CreateTestLocation());

        // Act
        await _locationService.DeleteLocationAsync(_testLocationId);

        // Assert
        _mockRepo.Verify(r => r.DeleteLocationAsync(_testLocationId), Times.Once);
    }

    [Fact]
    public async Task DeleteLocationAsync_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetLocationByIdAsync(_nonExistingLocationId))
            .ReturnsAsync((Location)null);

        // Act & Assert
        await Assert.ThrowsAsync<LocationNotFoundException>(
            () => _locationService.DeleteLocationAsync(_nonExistingLocationId));
    }

    [Theory]
    [InlineData(typeof(DbUpdateException))]
    [InlineData(typeof(InvalidOperationException))]
    public async Task DeleteLocationAsync_ThrowsDeleteException_OnFailure(Type exceptionType)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test error");
        _mockRepo.Setup(r => r.DeleteLocationAsync(_testLocationId))
            .ThrowsAsync(exception);
        _mockRepo.Setup(r => r.GetLocationByIdAsync(_testLocationId))
            .ReturnsAsync(CreateTestLocation());

        // Act & Assert
        await Assert.ThrowsAsync<LocationDeleteException>(
            () => _locationService.DeleteLocationAsync(_testLocationId));
    }
}