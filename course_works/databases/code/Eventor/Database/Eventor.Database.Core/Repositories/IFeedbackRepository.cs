using Eventor.Common.Core;
namespace Eventor.Database.Core;

/// <summary>
/// Интерфейс репозитория отзыва
/// </summary>
public interface IFeedbackRepository
{
    /// <summary>
    /// Получить все отзывы
    /// </summary>
    /// <returns>Список всех отзывов</returns>
    Task<List<Feedback>> GetAllFeedbackAsync();

    /// <summary>
    /// Получить все отзывы мероприятия
    /// </summary>
    /// <returns>Список всех отзывов мероприятия</returns>
    Task<List<Feedback>> GetAllFeedbacksByEventAsync(Guid eventId);

    /// <summary>
    /// Получить все отзывы пользователя
    /// </summary>
    /// <returns>Список всех отзывов пользователя</returns>
    Task<List<Feedback>> GetAllFeedbacksByUserAsync(Guid userId);

    /// <summary>
    /// Получить все отзывы пользователя
    /// </summary>
    /// <returns>Список всех отзывов пользователя</returns>
    Task<Feedback> GetFeedbackByIdAsync(Guid feedbackId);

    /// <summary>
    /// Создать отзыв
    /// </summary>
    /// <returns></returns>
    Task InsertFeedbackAsync(Feedback feedback);

    /// <summary>
    /// Обновить отзыв
    /// </summary>
    /// <returns></returns>
    Task UpdateFeedbackAsync(Feedback updateFeedback);

    /// <summary>
    /// Удалить отзыв
    /// </summary>
    /// <returns></returns>
    Task DeleteFeedbackAsync(Guid feedbackId);
}
