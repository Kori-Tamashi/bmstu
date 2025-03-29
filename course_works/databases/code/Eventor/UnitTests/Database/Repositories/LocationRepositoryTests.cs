using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

public class LocationRepositoryTests
{
    private readonly DbContextOptions<EventorDBContext> _options;

    public LocationRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllLocationsAsync_ReturnsAllLocations()
    {
        // Arrange
        var testLocations = new List<LocationDBModel>
        {
            new(Guid.NewGuid(), "Location 1", "Desc 1", 1000),
            new(Guid.NewGuid(), "Location 2", "Desc 2", 2000)
        };

        using (var context = new EventorDBContext(_options))
        {
            context.Locations.AddRange(testLocations);
            await context.SaveChangesAsync();
        }

        // Act
        List<Location> result;
        using (var context = new EventorDBContext(_options))
        {
            var repository = new LocationRepository(context);
            result = await repository.GetAllLocationsAsync();
        }

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, l => l.Name == "Location 1");
        Assert.Contains(result, l => l.Price == 2000);
    }

    [Fact]
    public async Task GetLocationByIdAsync_ReturnsCorrectLocation()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var testLocation = new LocationDBModel(locationId, "Test Location", "Test Desc", 1500);

        using (var context = new EventorDBContext(_options))
        {
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
        }

        // Act
        Location result;
        using (var context = new EventorDBContext(_options))
        {
            var repository = new LocationRepository(context);
            result = await repository.GetLocationByIdAsync(locationId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal(locationId, result.Id);
        Assert.Equal("Test Location", result.Name);
    }

    [Fact]
    public async Task InsertLocationAsync_AddsNewLocationToDatabase()
    {
        // Arrange
        var newLocation = new Location(
            Guid.NewGuid(),
            "New Location",
            "New Description",
            3000
        );

        // Act
        using (var context = new EventorDBContext(_options))
        {
            var repository = new LocationRepository(context);
            await repository.InsertLocationAsync(newLocation);
        }

        // Assert
        using (var context = new EventorDBContext(_options))
        {
            var locationInDb = await context.Locations.FirstOrDefaultAsync();
            Assert.NotNull(locationInDb);
            Assert.Equal("New Location", locationInDb.Name);
            Assert.Equal(3000, locationInDb.Price);
        }
    }

    [Fact]
    public async Task UpdateLocationAsync_UpdatesExistingLocation()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var originalLocation = new LocationDBModel(locationId, "Original Name", "Original Desc", 1000);

        using (var context = new EventorDBContext(_options))
        {
            context.Locations.Add(originalLocation);
            await context.SaveChangesAsync();
        }

        var updatedLocation = new Location(
            locationId,
            "Updated Name",
            "Updated Desc",
            2000
        );

        // Act
        using (var context = new EventorDBContext(_options))
        {
            var repository = new LocationRepository(context);
            await repository.UpdateLocationAsync(updatedLocation);
        }

        // Assert
        using (var context = new EventorDBContext(_options))
        {
            var locationInDb = await context.Locations.FindAsync(locationId);
            Assert.NotNull(locationInDb);
            Assert.Equal("Updated Name", locationInDb.Name);
            Assert.Equal("Updated Desc", locationInDb.Description);
            Assert.Equal(2000, locationInDb.Price);
        }
    }

    [Fact]
    public async Task DeleteLocationAsync_RemovesLocationFromDatabase()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var testLocation = new LocationDBModel(locationId, "To Delete", "Delete Desc", 500);

        using (var context = new EventorDBContext(_options))
        {
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new EventorDBContext(_options))
        {
            var repository = new LocationRepository(context);
            await repository.DeleteLocationAsync(locationId);
        }

        // Assert
        using (var context = new EventorDBContext(_options))
        {
            var locationInDb = await context.Locations.FindAsync(locationId);
            Assert.Null(locationInDb);
        }
    }

    [Fact]
    public async Task DeleteLocationAsync_WhenLocationNotFound_DoesNothing()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        using (var context = new EventorDBContext(_options))
        {
            var repository = new LocationRepository(context);
            await repository.DeleteLocationAsync(nonExistentId); // Не должно выбрасывать исключение

            var locationsCount = await context.Locations.CountAsync();
            Assert.Equal(0, locationsCount);
        }
    }
}