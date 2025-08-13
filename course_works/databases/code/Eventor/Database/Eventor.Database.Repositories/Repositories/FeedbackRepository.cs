using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventor.Database.Repositories;

/// <summary>
/// Репозиторий для работы с отзывами о мероприятиях
/// </summary>
public class FeedbackRepository : BaseRepository, IFeedbackRepository
{
    private readonly EventorDBContext _dbContext;
    private readonly ILogger<FeedbackRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория отзывов
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <exception cref="ArgumentNullException">Если dbContext равен null</exception>
    public FeedbackRepository(
        EventorDBContext dbContext,
        ILogger<FeedbackRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все отзывы
    /// </summary>
    public async Task<List<Feedback>> GetAllFeedbackAsync()
    {
        try
        {
            return await _dbContext.Feedbacks
                .Include(f => f.Event)
                .Include(f => f.Person)
                .Select(f => FeedbackConverter.ConvertDBToCore(f))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения списка отзывов");
            throw new InvalidOperationException("Не удалось получить список отзывов", ex);
        }
    }

    /// <summary>
    /// Получить все отзывы конкретного мероприятия
    /// </summary>
    public async Task<List<Feedback>> GetAllFeedbacksByEventAsync(Guid eventId)
    {
        try
        {
            return await _dbContext.Feedbacks
                .Where(f => f.EventId == eventId)
                .Include(f => f.Person)
                .Select(f => FeedbackConverter.ConvertDBToCore(f))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения отзывов для мероприятия {EventId}", eventId);
            throw new InvalidOperationException($"Не удалось получить отзывы для мероприятия {eventId}", ex);
        }
    }

    /// <summary>
    /// Получить все отзывы конкретного участника
    /// </summary>
    public async Task<List<Feedback>> GetAllFeedbacksByUserAsync(Guid personId)
    {
        try
        {
            return await _dbContext.Feedbacks
                .Where(f => f.PersonId == personId)
                .Include(f => f.Event)
                .Select(f => FeedbackConverter.ConvertDBToCore(f))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения отзывов участника {PersonId}", personId);
            throw new InvalidOperationException($"Не удалось получить отзывы участника {personId}", ex);
        }
    }

    /// <summary>
    /// Получить отзыв по идентификатору
    /// </summary>
    public async Task<Feedback> GetFeedbackByIdAsync(Guid feedbackId)
    {
        try
        {
            var feedbackEntity = await _dbContext.Feedbacks
                .Include(f => f.Event)
                .Include(f => f.Person)
                .FirstOrDefaultAsync(f => f.Id == feedbackId);

            return feedbackEntity != null
                ? FeedbackConverter.ConvertDBToCore(feedbackEntity)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения отзыва {FeedbackId}", feedbackId);
            throw new InvalidOperationException($"Не удалось получить отзыв {feedbackId}", ex);
        }
    }

    /// <summary>
    /// Добавить новый отзыв
    /// </summary>
    public async Task InsertFeedbackAsync(Feedback feedback)
    {
        try
        {
            // Проверяем существование связанного мероприятия
            var eventExists = await _dbContext.Events
                .AnyAsync(e => e.Id == feedback.EventId);

            if (!eventExists)
            {
                throw new ArgumentException($"Мероприятие с ID {feedback.EventId} не найдено");
            }

            // Проверяем существование связанного участника
            var personExists = await _dbContext.Persons
                .AnyAsync(p => p.Id == feedback.PersonId);

            if (!personExists)
            {
                throw new ArgumentException($"Участник с ID {feedback.PersonId} не найден");
            }

            // Создаем и сохраняем отзыв
            var feedbackEntity = FeedbackConverter.ConvertCoreToDB(feedback);
            await _dbContext.Feedbacks.AddAsync(feedbackEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка создания отзыва");
            throw new InvalidOperationException("Не удалось создать отзыв", ex);
        }
    }

    /// <summary>
    /// Обновить существующий отзыв
    /// </summary>
    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        try
        {
            var existingFeedback = await _dbContext.Feedbacks
                .FirstOrDefaultAsync(f => f.Id == feedback.Id);

            if (existingFeedback == null)
            {
                _logger.LogWarning("Отзыв {FeedbackId} не найден", feedback.Id);
                return;
            }

            existingFeedback.Comment = feedback.Comment;
            existingFeedback.Rating = feedback.Rating;

            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка обновления отзыва {FeedbackId}", feedback.Id);
            throw new InvalidOperationException($"Не удалось обновить отзыв {feedback.Id}", ex);
        }
    }

    /// <summary>
    /// Удалить отзыв по идентификатору
    /// </summary>
    public async Task DeleteFeedbackAsync(Guid feedbackId)
    {
        try
        {
            var feedbackToDelete = await _dbContext.Feedbacks
                .FirstOrDefaultAsync(f => f.Id == feedbackId);

            if (feedbackToDelete == null)
            {
                _logger.LogWarning("Отзыв {FeedbackId} не найден", feedbackId);
                return;
            }

            _dbContext.Feedbacks.Remove(feedbackToDelete);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка удаления отзыва {FeedbackId}", feedbackId);
            throw new InvalidOperationException($"Не удалось удалить отзыв {feedbackId}", ex);
        }
    }
}