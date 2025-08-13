using EventCore = Eventor.Common.Core.Event;
using EventDTO = Eventor.Common.DTO.EventDTOModel;
using EventDB = Eventor.Database.Models.EventDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели мероприятия
/// </summary>
public static class EventConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static EventCore ConvertDBToCore(EventDB eventDBModel)
    {
        return new EventCore(id: eventDBModel.Id,
                         locationId: eventDBModel.LocationId,
                         name: eventDBModel.Name,
                         description: eventDBModel.Description,
                         date: eventDBModel.Date,
                         personCount: eventDBModel.PersonCount,
                         daysCount: eventDBModel.DaysCount,
                         percent: eventDBModel.Percent,
                         rating: eventDBModel.Rating);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static EventDB ConvertCoreToDB(EventCore eventCoreModel)
    {
        return new EventDB(id: eventCoreModel.Id,
                         locationId: eventCoreModel.LocationId,
                         name: eventCoreModel.Name,
                         description: eventCoreModel.Description,
                         date: eventCoreModel.Date,
                         personCount: eventCoreModel.PersonCount,
                         daysCount: eventCoreModel.DaysCount,
                         percent: eventCoreModel.Percent,
                         rating: eventCoreModel.Rating);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static EventDTO ConvertCoreToDTO(EventCore eventCoreModel)
    {
        return new EventDTO(id: eventCoreModel.Id,
                         locationId: eventCoreModel.LocationId,
                         name: eventCoreModel.Name,
                         description: eventCoreModel.Description,
                         date: eventCoreModel.Date,
                         personCount: eventCoreModel.PersonCount,
                         daysCount: eventCoreModel.DaysCount,
                         percent: eventCoreModel.Percent,
                         rating: eventCoreModel.Rating);
    }

}