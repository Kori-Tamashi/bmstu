using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _logger.LogError(ex, "Database error while retrieving locations");
            throw new LocationServiceException("Failed to retrieve locations due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving locations");
            throw new LocationServiceException("Failed to retrieve locations due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving locations");
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
                _logger.LogWarning("Location {LocationId} not found", locationId);
                throw new LocationNotFoundException($"Location {locationId} not found");
            }
            return location;
        }
        catch (LocationNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid location ID: {LocationId}", locationId);
            throw new LocationServiceException($"Invalid location ID: {locationId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving location {LocationId}", locationId);
            throw new LocationServiceException($"Failed to retrieve location {locationId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving location {LocationId}", locationId);
            throw new LocationServiceException($"Failed to retrieve location {locationId}", ex);
        }
    }

    public async Task AddLocationAsync(Location location)
    {
        try
        {
            await _locationRepository.InsertLocationAsync(location);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error while creating location");
            throw new LocationCreateException($"Failed to create location: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while creating location");
            throw new LocationCreateException("Failed to create location due to database constraints", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data conflict while creating location");
            throw new LocationCreateException("Failed to create location due to data conflict", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating location");
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
                _logger.LogWarning("Location {LocationId} not found for update", updateLocation.Id);
                throw new LocationNotFoundException($"Location {updateLocation.Id} not found");
            }

            await _locationRepository.UpdateLocationAsync(updateLocation);
        }
        catch (LocationNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid data while updating location {LocationId}", updateLocation.Id);
            throw new LocationUpdateException($"Failed to update location {updateLocation.Id}: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating location {LocationId}", updateLocation.Id);
            throw new LocationUpdateException($"Failed to update location {updateLocation.Id} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while updating location {LocationId}", updateLocation.Id);
            throw new LocationUpdateException($"Concurrency conflict while updating location {updateLocation.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating location {LocationId}", updateLocation.Id);
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
                _logger.LogWarning("Location {LocationId} not found for deletion", locationId);
                throw new LocationNotFoundException($"Location {locationId} not found");
            }

            await _locationRepository.DeleteLocationAsync(locationId);
        }
        catch (LocationNotFoundException)
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid location ID: {LocationId}", locationId);
            throw new LocationDeleteException($"Invalid location ID: {locationId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting location {LocationId}", locationId);
            throw new LocationDeleteException($"Failed to delete location {locationId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while deleting location {LocationId}", locationId);
            throw new LocationDeleteException($"Concurrency conflict while deleting location {locationId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting location {LocationId}", locationId);
            throw new LocationDeleteException($"Failed to delete location {locationId}", ex);
        }
    }
}