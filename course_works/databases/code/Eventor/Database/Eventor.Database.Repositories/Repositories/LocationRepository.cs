using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с локациями мероприятий
/// </summary>
public class LocationRepository : BaseRepository, ILocationRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория локаций
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public LocationRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить все локации
    /// </summary>
    /// <returns>Список всех локаций</returns>
    public async Task<List<Location>> GetAllLocationsAsync()
    {
        return await _dbContext.Locations
            .Select(l => LocationConverter.ConvertDBToCore(l))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить локацию по идентификатору
    /// </summary>
    /// <param name="locationId">Идентификатор локации</param>
    /// <returns>Найденная локация или null</returns>
    public async Task<Location> GetLocationByIdAsync(Guid locationId)
    {
        var locationEntity = await _dbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == locationId);

        return locationEntity != null
            ? LocationConverter.ConvertDBToCore(locationEntity)
            : null;
    }

    /// <summary>
    /// Добавить новую локацию
    /// </summary>
    /// <param name="location">Модель локации для добавления</param>
    public async Task InsertLocationAsync(Location location)
    {
        var locationEntity = LocationConverter.ConvertCoreToDB(location);
        await _dbContext.Locations.AddAsync(locationEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить существующую локацию
    /// </summary>
    /// <param name="location">Модель с обновленными данными локации</param>
    public async Task UpdateLocationAsync(Location location)
    {
        var existingLocation = await _dbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == location.Id);

        if (existingLocation == null) return;

        existingLocation.Name = location.Name;
        existingLocation.Description = location.Description;
        existingLocation.Price = location.Price;

        _dbContext.Locations.Update(existingLocation);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить локацию по идентификатору
    /// </summary>
    /// <param name="locationId">Идентификатор удаляемой локации</param>
    public async Task DeleteLocationAsync(Guid locationId)
    {
        var locationToDelete = await _dbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == locationId);

        if (locationToDelete == null) return;

        _dbContext.Locations.Remove(locationToDelete);
        await _dbContext.SaveChangesAsync();
    }
}