using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eventor.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<LocationService> _logger;

    public LocationService(ILocationRepository locationRepository, ILogger<LocationService> logger)
    {
        _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        try
        {
            return await _locationRepository.GetAllLocationsAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving locations");
            throw new LocationServiceException("Failed to retrieve locations", ex);
        }
    }

    public async Task<Location> GetLocationByIdAsync(Guid locationId)
    {
        try
        {
            var location = await _locationRepository.GetLocationByIdAsync(locationId);
            if (location == null)
            {
                throw new LocationNotFoundException($"Location {locationId} not found");
            }
            return location;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving location {LocationId}", locationId);
            throw new LocationServiceException($"Failed to retrieve location {locationId}", ex);
        }
    }

    public async Task AddLocationAsync(Location location)
    {
        try
        {
            await _locationRepository.InsertLocationAsync(location);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating location");
            throw new LocationCreateException("Failed to create location", ex);
        }
    }

    public async Task UpdateLocationAsync(Location updateLocation)
    {
        try
        {
            var existingLocation = await _locationRepository.GetLocationByIdAsync(updateLocation.Id);
            if (existingLocation == null)
            {
                throw new LocationNotFoundException($"Location {updateLocation.Id} not found");
            }

            await _locationRepository.UpdateLocationAsync(updateLocation);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating location {LocationId}", updateLocation.Id);
            throw new LocationUpdateException($"Failed to update location {updateLocation.Id}", ex);
        }
    }

    public async Task DeleteLocationAsync(Guid locationId)
    {
        try
        {
            var existingLocation = await _locationRepository.GetLocationByIdAsync(locationId);
            if (existingLocation == null)
            {
                throw new LocationNotFoundException($"Location {locationId} not found");
            }

            await _locationRepository.DeleteLocationAsync(locationId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting location {LocationId}", locationId);
            throw new LocationDeleteException($"Failed to delete location {locationId}", ex);
        }
    }
}