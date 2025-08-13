using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;
using Microsoft.Extensions.Logging;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с локациями мероприятий
/// </summary>
public class LocationRepository : BaseRepository, ILocationRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<LocationRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория локаций
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public LocationRepository(
        EventorDBContext dbContext,
        ILogger<LocationRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все локации
    /// </summary>
    public async Task<List<Location>> GetAllLocationsAsync()
    {
        try
        {
            return await _dbContext.Locations
                .Select(l => LocationConverter.ConvertDBToCore(l))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка локаций");
            throw new InvalidOperationException("Не удалось получить список локаций", ex);
        }
    }

    /// <summary>
    /// Получить локацию по идентификатору
    /// </summary>
    public async Task<Location> GetLocationByIdAsync(Guid locationId)
    {
        try
        {
            var locationEntity = await _dbContext.Locations
                .FirstOrDefaultAsync(l => l.Id == locationId);

            return locationEntity != null
                ? LocationConverter.ConvertDBToCore(locationEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения локации {LocationId}", locationId);
            throw new InvalidOperationException($"Не удалось получить локацию {locationId}", ex);
        }
    }

    /// <summary>
    /// Добавить новую локацию
    /// </summary>
    public async Task InsertLocationAsync(Location location)
    {
        try
        {
            var locationEntity = LocationConverter.ConvertCoreToDB(location);
            await _dbContext.Locations.AddAsync(locationEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания локации");
            throw new InvalidOperationException("Не удалось создать локацию", ex);
        }
    }

    /// <summary>
    /// Обновить существующую локацию
    /// </summary>
    public async Task UpdateLocationAsync(Location location)
    {
        try
        {
            var existingLocation = await _dbContext.Locations
                .FirstOrDefaultAsync(l => l.Id == location.Id);

            if (existingLocation == null)
            {
                _logger.LogWarning("Локация {LocationId} не найдена", location.Id);
                return;
            }

            existingLocation.Name = location.Name;
            existingLocation.Description = location.Description;
            existingLocation.Price = location.Price;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления локации {LocationId}", location.Id);
            throw new InvalidOperationException($"Не удалось обновить локацию {location.Id}", ex);
        }
    }

    /// <summary>
    /// Удалить локацию по идентификатору
    /// </summary>
    public async Task DeleteLocationAsync(Guid locationId)
    {
        try
        {
            var locationToDelete = await _dbContext.Locations
                .FirstOrDefaultAsync(l => l.Id == locationId);

            if (locationToDelete == null)
            {
                _logger.LogWarning("Локация {LocationId} не найдена", locationId);
                return;
            }

            _dbContext.Locations.Remove(locationToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления локации {LocationId}", locationId);
            throw new InvalidOperationException($"Не удалось удалить локацию {locationId}", ex);
        }
    }
}