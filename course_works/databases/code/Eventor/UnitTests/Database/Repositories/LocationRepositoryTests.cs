using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
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

public class LocationRepositoryTests : IDisposable
{
    private readonly EventorDBContext _context;
    private readonly ILogger<LocationRepository> _logger;
    private readonly LocationRepository _repository;
    private readonly DbContextOptions<EventorDBContext> _options;

    public LocationRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventorDBContext(_options);
        _logger = Mock.Of<ILogger<LocationRepository>>();
        _repository = new LocationRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private LocationDBModel CreateTestLocation(
        Guid? id = null,
        string name = "Test Location",
        string description = "Test Description",
        double price = 1000)
    {
        return new LocationDBModel(
            id ?? Guid.NewGuid(),
            name,
            description,
            price);
    }

    [Fact]
    public async Task GetAllLocationsAsync_ShouldReturnAllLocations()
    {
        // Arrange
        var locations = new List<LocationDBModel>
        {
            CreateTestLocation(name: "Location 1"),
            CreateTestLocation(name: "Location 2")
        };

        await _context.Locations.AddRangeAsync(locations);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllLocationsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, l => l.Name == "Location 1");
    }

    [Fact]
    public async Task GetAllLocationsAsync_ShouldLogErrorOnFailure()
    {
        // Arrange
        var corruptedContext = new Mock<EventorDBContext>(_options);
        corruptedContext.Setup(c => c.Locations).Throws<Exception>();
        var repository = new LocationRepository(corruptedContext.Object, _logger);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllLocationsAsync());
        Mock.Get(_logger).VerifyLog(LogLevel.Error, "Ошибка получения списка локаций");
    }

    [Fact]
    public async Task GetLocationByIdAsync_ShouldReturnCorrectLocation()
    {
        // Arrange
        var location = CreateTestLocation();
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetLocationByIdAsync(location.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(location.Description, result.Description);
    }

    [Fact]
    public async Task GetLocationByIdAsync_ShouldReturnNullForNonExistingId()
    {
        // Act
        var result = await _repository.GetLocationByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertLocationAsync_ShouldAddNewLocation()
    {
        // Arrange
        var newLocation = new Location(
            Guid.NewGuid(),
            "New Location",
            "Description",
            3000);

        // Act
        await _repository.InsertLocationAsync(newLocation);
        var result = await _context.Locations.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newLocation.Name, result.Name);
    }

    [Fact]
    public async Task InsertLocationAsync_ShouldThrowOnDatabaseError()
    {
        // Arrange
        var newLocation = new Location(Guid.NewGuid(), "Test", "Desc", 1000);

        // Настраиваем мок DbSet
        var mockSet = new Mock<DbSet<LocationDBModel>>();
        mockSet.Setup(m => m.AddAsync(It.IsAny<LocationDBModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LocationDBModel model, CancellationToken token) => null);

        // Настраиваем контекст
        var corruptedContext = new Mock<EventorDBContext>(_options);
        corruptedContext.Setup(c => c.Locations).Returns(mockSet.Object);
        corruptedContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Test error"));

        var repository = new LocationRepository(corruptedContext.Object, _logger);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.InsertLocationAsync(newLocation));

        Assert.Contains("Не удалось создать локацию", exception.Message);
        Mock.Get(_logger).VerifyLog(LogLevel.Error, "Ошибка создания локации");
    }

    [Fact]
    public async Task UpdateLocationAsync_ShouldUpdateExistingLocation()
    {
        // Arrange
        var location = CreateTestLocation();
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();

        var updatedLocation = new Location(
            location.Id,
            "Updated Name",
            "Updated Desc",
            2000);

        // Act
        await _repository.UpdateLocationAsync(updatedLocation);
        var result = await _context.Locations.FindAsync(location.Id);

        // Assert
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal(2000, result.Price);
    }

    [Fact]
    public async Task UpdateLocationAsync_ShouldNotUpdateNonExistingLocation()
    {
        // Arrange
        var nonExistingLocation = new Location(
            Guid.NewGuid(),
            "Non-existing",
            "Desc",
            500);

        // Act
        await _repository.UpdateLocationAsync(nonExistingLocation);
        var count = await _context.Locations.CountAsync();

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task DeleteLocationAsync_ShouldRemoveLocation()
    {
        // Arrange
        var location = CreateTestLocation();
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteLocationAsync(location.Id);
        var result = await _context.Locations.FindAsync(location.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteLocationAsync_ShouldLogWarningForNonExistingId()
    {
        // Act
        await _repository.DeleteLocationAsync(Guid.NewGuid());

        // Assert
        Mock.Get(_logger).VerifyLog(LogLevel.Warning, "не найдена");
    }
}