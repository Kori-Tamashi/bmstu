using LocationCore = Eventor.Common.Core.Location;
using LocationDTO = Eventor.Common.DTO.LocationDTOModel;
using LocationDB = Eventor.Database.Models.LocationDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели локации
/// </summary>
public class LocationConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static LocationCore ConvertDBToCore(LocationDB locationDBModel)
    {
        return new LocationCore(id: locationDBModel.Id,
                                name: locationDBModel.Name,
                                description: locationDBModel.Description,
                                price: locationDBModel.Price);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static LocationDB ConvertCoreToDB(LocationCore locationCoreModel)
    {
        return new LocationDB(id: locationCoreModel.Id,
                              name: locationCoreModel.Name,
                              description: locationCoreModel.Description,
                              price: locationCoreModel.Price);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static LocationDTO ConvertCoreToDTO(LocationCore locationCoreModel)
    {
        return new LocationDTO(id: locationCoreModel.Id,
                              name: locationCoreModel.Name,
                              description: locationCoreModel.Description,
                              price: locationCoreModel.Price);
    }
}
