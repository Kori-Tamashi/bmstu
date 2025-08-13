using MenuCore = Eventor.Common.Core.Menu;
using MenuDTO = Eventor.Common.DTO.MenuDTOModel;
using MenuDB = Eventor.Database.Models.MenuDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели мероприятия
/// </summary>
public class MenuConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static MenuCore ConvertDBToCore(MenuDB menuDBModel)
    {
        return new MenuCore(id: menuDBModel.Id,
                            name: menuDBModel.Name,
                            cost: menuDBModel.Cost);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static MenuDB ConvertCoreToDB(MenuCore menuCoreModel)
    {
        return new MenuDB(id: menuCoreModel.Id,
                          name: menuCoreModel.Name,
                          cost: menuCoreModel.Cost);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static MenuDTO ConvertCoreToDTO(MenuCore menuCoreModel)
    {
        return new MenuDTO(id: menuCoreModel.Id,
                           name: menuCoreModel.Name,
                           cost: menuCoreModel.Cost);
    }
}
