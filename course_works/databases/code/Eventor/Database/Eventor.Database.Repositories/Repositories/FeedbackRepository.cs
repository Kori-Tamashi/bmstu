using Eventor.Database.Core;
using Eventor.Common.Core;
using Eventor.Common.Converter;
using Microsoft.EntityFrameworkCore;
using Eventor.Database.Context;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с отзывами о мероприятиях
/// </summary>
public class FeedbackRepository : BaseRepository, IFeedbackRepository
{
    private readonly EventorDBContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория отзывов
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public FeedbackRepository(EventorDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получить все отзывы
    /// </summary>
    /// <returns>Список всех отзывов</returns>
    public async Task<List<Feedback>> GetAllFeedbackAsync()
    {
        return await _dbContext.Feedbacks
            .Include(f => f.Event)
            .Include(f => f.Person)
            .Select(f => FeedbackConverter.ConvertDBToCore(f))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить все отзывы конкретного мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns>Список отзывов для указанного мероприятия</returns>
    public async Task<List<Feedback>> GetAllFeedbacksByEventAsync(Guid eventId)
    {
        return await _dbContext.Feedbacks
            .Where(f => f.EventId == eventId)
            .Include(f => f.Person)
            .Select(f => FeedbackConverter.ConvertDBToCore(f))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить все отзывы конкретного участника
    /// </summary>
    /// <param name="personId">Идентификатор участника</param>
    /// <returns>Список отзывов указанного участника</returns>
    public async Task<List<Feedback>> GetAllFeedbacksByUserAsync(Guid personId)
    {
        return await _dbContext.Feedbacks
            .Where(f => f.PersonId == personId)
            .Include(f => f.Event)
            .Select(f => FeedbackConverter.ConvertDBToCore(f))
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Получить отзыв по идентификатору
    /// </summary>
    /// <param name="feedbackId">Идентификатор отзыва</param>
    /// <returns>Найденный отзыв или null</returns>
    public async Task<Feedback> GetFeedbackByIdAsync(Guid feedbackId)
    {
        var feedbackEntity = await _dbContext.Feedbacks
            .Include(f => f.Event)
            .Include(f => f.Person)
            .FirstOrDefaultAsync(f => f.Id == feedbackId);

        return feedbackEntity != null
            ? FeedbackConverter.ConvertDBToCore(feedbackEntity)
            : null;
    }

    /// <summary>
    /// Добавить новый отзыв
    /// </summary>
    /// <param name="feedback">Модель отзыва для добавления</param>
    public async Task InsertFeedbackAsync(Feedback feedback)
    {
        var feedbackEntity = FeedbackConverter.ConvertCoreToDB(feedback);
        await _dbContext.Feedbacks.AddAsync(feedbackEntity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить существующий отзыв
    /// </summary>
    /// <param name="feedback">Модель с обновленными данными отзыва</param>
    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        var existingFeedback = await _dbContext.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == feedback.Id);

        if (existingFeedback == null) return;

        existingFeedback.Comment = feedback.Comment;
        existingFeedback.Rating = feedback.Rating;

        _dbContext.Feedbacks.Update(existingFeedback);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить отзыв по идентификатору
    /// </summary>
    /// <param name="feedbackId">Идентификатор удаляемого отзыва</param>
    public async Task DeleteFeedbackAsync(Guid feedbackId)
    {
        var feedbackToDelete = await _dbContext.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == feedbackId);

        if (feedbackToDelete == null) return;

        _dbContext.Feedbacks.Remove(feedbackToDelete);
        await _dbContext.SaveChangesAsync();
    }
}