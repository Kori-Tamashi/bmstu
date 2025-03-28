using PersonCore = Eventor.Common.Core.Person;
using PersonDTO = Eventor.Common.DTO.PersonDTOModel;
using PersonDB = Eventor.Database.Models.PersonDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели участника мероприятия
/// </summary>
public class PersonConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static PersonCore ConvertDBToCore(PersonDB personDBModel)
    {
        return new PersonCore(id: personDBModel.Id,
                              name: personDBModel.Name,
                              type: personDBModel.Type,
                              paid: personDBModel.Paid);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static PersonDB ConvertCoreToDB(PersonCore personCoreModel)
    {
        return new PersonDB(id: personCoreModel.Id,
                            name: personCoreModel.Name,
                            type: personCoreModel.Type,
                            paid: personCoreModel.Paid);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static PersonDTO ConvertCoreToDTO(PersonCore personCoreModel)
    {
        return new PersonDTO(id: personCoreModel.Id,
                             name: personCoreModel.Name,
                             type: personCoreModel.Type,
                             paid: personCoreModel.Paid);
    }
}

