using ItemCore = Eventor.Common.Core.Item;
using ItemDTO = Eventor.Common.DTO.ItemDTOModel;
using ItemDB = Eventor.Database.Models.ItemDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели предмета
/// </summary>
public class ItemConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static ItemCore ConvertDBToCore(ItemDB itemDBModel)
    {
        return new ItemCore(id: itemDBModel.Id,
                            name: itemDBModel.Name,
                            type: itemDBModel.Type,
                            price: itemDBModel.Price);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static ItemDB ConvertCoreToDB(ItemCore itemCoreModel)
    {
        return new ItemDB(id: itemCoreModel.Id,
                          name: itemCoreModel.Name,
                          type: itemCoreModel.Type,
                          price: itemCoreModel.Price);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static ItemDTO ConvertCoreToDTO(ItemCore itemCoreModel)
    {
        return new ItemDTO(id: itemCoreModel.Id,
                           name: itemCoreModel.Name,
                           type: itemCoreModel.Type,
                           price: itemCoreModel.Price);
    }
}
