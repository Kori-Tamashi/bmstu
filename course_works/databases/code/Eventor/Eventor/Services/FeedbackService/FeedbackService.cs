using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eventor.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger)
    {
        _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Feedback>> GetAllFeedbackAsync()
    {
        try
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbackAsync();

            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogInformation("No feedback found for event");
                return new List<Feedback>(); // Возвращаем пустой список вместо null
            }
            return feedbacks;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving feedback");
            throw new FeedbackServiceException("Failed to retrieve feedback due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data inconsistency error while retrieving feedback");
            throw new FeedbackServiceException("Failed to retrieve feedback due to data inconsistency", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving feedback");
            throw new FeedbackServiceException("Failed to retrieve feedback", ex);
        }
    }

    public async Task<List<Feedback>> GetAllFeedbacksByEventAsync(Guid eventId)
    {
        try
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksByEventAsync(eventId);

            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogInformation("No feedback found for event {EventId}", eventId);
                return new List<Feedback>(); // Возвращаем пустой список вместо null
            }
            return feedbacks;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid event ID: {EventId}", eventId);
            throw new FeedbackServiceException($"Invalid event ID: {eventId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving feedback for event {EventId}", eventId);
            throw new FeedbackServiceException($"Failed to retrieve feedback for event {eventId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving feedback for event {EventId}", eventId);
            throw new FeedbackServiceException($"Failed to retrieve feedback for event {eventId}", ex);
        }
    }

    public async Task<List<Feedback>> GetAllFeedbacksByUserAsync(Guid userId)
    {
        try
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksByUserAsync(userId);

            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogInformation("No feedback found for user {UserId}", userId);
                return new List<Feedback>(); // Возвращаем пустой список вместо null
            }
            return feedbacks;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid user ID: {UserId}", userId);
            throw new FeedbackServiceException($"Invalid user ID: {userId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving feedback for user {UserId}", userId);
            throw new FeedbackServiceException($"Failed to retrieve feedback for user {userId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving feedback for user {UserId}", userId);
            throw new FeedbackServiceException($"Failed to retrieve feedback for user {userId}", ex);
        }
    }

    public async Task<Feedback> GetFeedbackByIdAsync(Guid feedbackId)
    {
        try
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);

            if (feedback == null)
            {
                _logger.LogWarning("Feedback {FeedbackId} not found", feedbackId);
                throw new FeedbackNotFoundException($"Feedback {feedbackId} not found");
            }
            return feedback;
        }
        catch (FeedbackNotFoundException) // Позволяем пройти собственным исключениям
        {
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid feedback ID: {FeedbackId}", feedbackId);
            throw new FeedbackServiceException($"Invalid feedback ID: {feedbackId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while retrieving feedback {FeedbackId}", feedbackId);
            throw new FeedbackServiceException($"Failed to retrieve feedback {feedbackId} due to database error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving feedback {FeedbackId}", feedbackId);
            throw new FeedbackServiceException($"Failed to retrieve feedback {feedbackId}", ex);
        }
    }

    public async Task AddFeedbackAsync(Feedback feedback)
    {
        try
        {
            await _feedbackRepository.InsertFeedbackAsync(feedback);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error while creating feedback");
            throw new FeedbackCreateException($"Failed to create feedback: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while creating feedback");
            throw new FeedbackCreateException("Failed to create feedback due to database constraints", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Data conflict while creating feedback");
            throw new FeedbackCreateException("Failed to create feedback due to data conflict", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating feedback");
            throw new FeedbackCreateException("Failed to create feedback", ex);
        }
    }

    public async Task UpdateFeedbackAsync(Feedback updateFeedback)
    {
        try
        {
            var existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(updateFeedback.Id);

            if (existingFeedback == null)
            {
                _logger.LogWarning("Feedback {FeedbackId} not found for update", updateFeedback.Id);
                throw new FeedbackNotFoundException($"Feedback {updateFeedback.Id} not found");
            }

            await _feedbackRepository.UpdateFeedbackAsync(updateFeedback);
        }
        catch (FeedbackNotFoundException)
        {
            throw; // Пробрасываем исключение без изменений
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid data while updating feedback {FeedbackId}", updateFeedback.Id);
            throw new FeedbackUpdateException($"Failed to update feedback {updateFeedback.Id}: {ex.Message}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating feedback {FeedbackId}", updateFeedback.Id);
            throw new FeedbackUpdateException($"Failed to update feedback {updateFeedback.Id} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while updating feedback {FeedbackId}", updateFeedback.Id);
            throw new FeedbackUpdateException($"Concurrency conflict while updating feedback {updateFeedback.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating feedback {FeedbackId}", updateFeedback.Id);
            throw new FeedbackUpdateException($"Failed to update feedback {updateFeedback.Id}", ex);
        }
    }

    public async Task DeleteFeedbackAsync(Guid feedbackId)
    {
        try
        {
            var existingFeedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);

            if (existingFeedback == null)
            {
                _logger.LogWarning("Feedback {FeedbackId} not found for deletion", feedbackId);
                throw new FeedbackNotFoundException($"Feedback {feedbackId} not found");
            }

            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid feedback ID: {FeedbackId}", feedbackId);
            throw new FeedbackDeleteException($"Invalid feedback ID: {feedbackId}", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting feedback {FeedbackId}", feedbackId);
            throw new FeedbackDeleteException($"Failed to delete feedback {feedbackId} due to database error", ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while deleting feedback {FeedbackId}", feedbackId);
            throw new FeedbackDeleteException($"Concurrency conflict while deleting feedback {feedbackId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting feedback {FeedbackId}", feedbackId);
            throw new FeedbackDeleteException($"Failed to delete feedback {feedbackId}", ex);
        }
    }
}