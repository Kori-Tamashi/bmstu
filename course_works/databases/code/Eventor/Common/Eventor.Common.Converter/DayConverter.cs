using DayCore = Eventor.Common.Core.Day;
using DayDTO = Eventor.Common.DTO.DayDTOModel;
using DayDB = Eventor.Database.Models.DayDBModel;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели конкретного дня мероприятия
/// </summary>
public class DayConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static DayCore ConvertDBToCore(DayDB dayDBModel)
    {
        return new DayCore(id: dayDBModel.Id,
                           eventId: dayDBModel.EventId,
                           name: dayDBModel.Name,
                           sequenceNumber: dayDBModel.SequenceNumber,
                           description: dayDBModel.Description,
                           price: dayDBModel.Price);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static DayDB ConvertCoreToDB(DayCore dayCoreModel)
    {
        return new DayDB(id: dayCoreModel.Id,
                         eventId: dayCoreModel.EventId,
                         name: dayCoreModel.Name,
                         sequenceNumber: dayCoreModel.SequenceNumber,
                         description: dayCoreModel.Description,
                         price: dayCoreModel.Price);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static DayDTO ConvertCoreToDTO(DayCore dayCoreModel)
    {
        return new DayDTO(id: dayCoreModel.Id,
                          eventId: dayCoreModel.EventId,
                          name: dayCoreModel.Name,
                          sequenceNumber: dayCoreModel.SequenceNumber,
                          description: dayCoreModel.Description,
                          price: dayCoreModel.Price);
    }
}

