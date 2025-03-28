using UserCore = Eventor.Common.Core.User;
using UserDTO = Eventor.Common.DTO.UserDTOModel;
using UserDB = Eventor.Database.Models.UserDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели пользователя
/// </summary>
public class UserConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static UserCore ConvertDBToCore(UserDB userDBModel)
    {
        return new UserCore(id: userDBModel.Id,
                            name: userDBModel.Name,
                            phone: userDBModel.Phone,
                            gender: userDBModel.Gender,
                            passwordHash: userDBModel.PasswordHash,
                            role: userDBModel.Role);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static UserDB ConvertCoreToDB(UserCore userCoreModel)
    {
        return new UserDB(id: userCoreModel.Id,
                          name: userCoreModel.Name,
                          phone: userCoreModel.Phone,
                          gender: userCoreModel.Gender,
                          passwordHash: userCoreModel.PasswordHash,
                          role: userCoreModel.Role);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static UserDTO ConvertCoreToDTO(UserCore userCoreModel)
    {
        return new UserDTO(id: userCoreModel.Id,
                          name: userCoreModel.Name,
                          phone: userCoreModel.Phone,
                          gender: userCoreModel.Gender,
                          passwordHash: userCoreModel.PasswordHash,
                          role: userCoreModel.Role);
    }
}
