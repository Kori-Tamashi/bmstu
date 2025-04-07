using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Database.Repositories;
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
            return await _feedbackRepository.GetAllFeedbackAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving all feedback");
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
            }
            return feedbacks;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving feedback for event {EventId}", eventId);
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
            }
            return feedbacks;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving feedback for user {UserId}", userId);
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
                throw new FeedbackNotFoundException($"Feedback {feedbackId} not found");
            }
            return feedback;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving feedback {FeedbackId}", feedbackId);
            throw new FeedbackServiceException($"Failed to retrieve feedback {feedbackId}", ex);
        }
    }

    public async Task AddFeedbackAsync(Feedback feedback)
    {
        try
        {
            await _feedbackRepository.InsertFeedbackAsync(feedback);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating feedback");
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
                throw new FeedbackNotFoundException($"Feedback {updateFeedback.Id} not found");
            }

            await _feedbackRepository.UpdateFeedbackAsync(updateFeedback);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating feedback {FeedbackId}", updateFeedback.Id);
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
                throw new FeedbackNotFoundException($"Feedback {feedbackId} not found");
            }

            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting feedback {FeedbackId}", feedbackId);
            throw new FeedbackDeleteException($"Failed to delete feedback {feedbackId}", ex);
        }
    }
}