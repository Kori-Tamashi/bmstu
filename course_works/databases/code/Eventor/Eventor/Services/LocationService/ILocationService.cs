using Eventor.Common.Core;
namespace Eventor.Services;

/// <summary>
/// Интерфейс репозитория локации
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Получить все локации
    /// </summary>
    /// <returns>Список всех локаций</returns>
    Task<List<Location>> GetAllLocationsAsync();

    /// <summary>
    /// Получить локацию по её идентификатору
    /// </summary>
    /// <returns>Локация</returns>
    Task<Location> GetLocationByIdAsync(Guid locationId);

    /// <summary>
    /// Создать локацию
    /// </summary>
    /// <returns></returns>
    Task AddLocationAsync(Location location);

    /// <summary>
    /// Обновить локацию
    /// </summary>
    /// <returns></returns>
    Task UpdateLocationAsync(Location updateLocation);

    /// <summary>
    /// Удалить локацию
    /// </summary>
    /// <returns></returns>
    Task DeleteLocationAsync(Guid locationId);
}

