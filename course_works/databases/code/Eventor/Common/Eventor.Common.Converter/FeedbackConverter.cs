using FeedbackCore = Eventor.Common.Core.Feedback;
using FeedbackDTO = Eventor.Common.DTO.FeedbackDTOModel;
using FeedbackDB = Eventor.Database.Models.FeedbackDBModel;
namespace Eventor.Common.Converter;

/// <summary>
/// Конвертатор модели отзыва
/// </summary>
public class FeedbackConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <returns>Модель бизнес логики</returns>
    public static FeedbackCore ConvertDBToCore(FeedbackDB feedbackDBModel)
    {
        return new FeedbackCore(id: feedbackDBModel.Id,
                                eventId: feedbackDBModel.EventId,
                                personId: feedbackDBModel.PersonId,
                                comment: feedbackDBModel.Comment,
                                rating: feedbackDBModel.Rating);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель базы данных
    /// </summary>
    /// <returns>Модель базы данных</returns>
    public static FeedbackDB ConvertCoreToDB(FeedbackCore feedbackCoreModel)
    {
        return new FeedbackDB(id: feedbackCoreModel.Id,
                              eventId: feedbackCoreModel.EventId,
                              personId: feedbackCoreModel.PersonId,
                              comment: feedbackCoreModel.Comment,
                              rating: feedbackCoreModel.Rating);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики приложения в модель DTO
    /// </summary>
    /// <returns>Модель DTO</returns>
    public static FeedbackDTO ConvertCoreToDTO(FeedbackCore feedbackCoreModel)
    {
        return new FeedbackDTO(id: feedbackCoreModel.Id,
                               eventId: feedbackCoreModel.EventId,
                               personId: feedbackCoreModel.PersonId,
                               comment: feedbackCoreModel.Comment,
                               rating: feedbackCoreModel.Rating);
    }
}
