using Eventor.Common.Core;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xunit;
using Eventor.Common.Enums;

namespace Eventor.Tests.Database.Repositories;

public class FeedbackRepositoryTests
{
    private readonly DbContextOptions<EventorDBContext> _options;

    public FeedbackRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllFeedbackAsync_ReturnsAllFeedback()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Создаем связанные сущности
        var testEvent = new EventDBModel(
            eventId,
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 15, 4.5
        );

        var testPerson = new PersonDBModel(
            personId,
            "Test User",
            PersonType.Standart,
            true
        );

        var testFeedbacks = new List<FeedbackDBModel>
    {
        new(Guid.NewGuid(), eventId, personId, "Good", 9),
        new(Guid.NewGuid(), eventId, personId, "Bad", 4)
    };

        using (var context = new EventorDBContext(_options))
        {
            // Добавляем обязательные связанные сущности
            context.Events.Add(testEvent);
            context.Persons.Add(testPerson);

            // Затем добавляем отзывы
            context.Feedbacks.AddRange(testFeedbacks);
            await context.SaveChangesAsync();
        }

        // Act
        List<Feedback> result;
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            result = await repository.GetAllFeedbackAsync();
        }

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Rating == 9);
        Assert.Contains(result, f => f.Rating == 4);
    }

    [Fact]
    public async Task GetAllFeedbacksByEventAsync_ReturnsFilteredFeedback()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Создаем связанные сущности
        var testEvent = new EventDBModel(
            eventId,
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 15, 4.5
        );

        var testPerson = new PersonDBModel(
            personId,
            "Test User",
            PersonType.Standart,
            true
        );

        var testData = new List<FeedbackDBModel>
    {
        new(Guid.NewGuid(), eventId, personId, "Event feedback", 8),
        new(Guid.NewGuid(), Guid.NewGuid(), personId, "Other event", 7)
    };

        using (var context = new EventorDBContext(_options))
        {
            // Добавляем обязательные сущности
            context.Events.Add(testEvent);
            context.Persons.Add(testPerson);

            // Затем добавляем отзывы
            context.Feedbacks.AddRange(testData);
            await context.SaveChangesAsync();
        }

        // Act
        List<Feedback> result;
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            result = await repository.GetAllFeedbacksByEventAsync(eventId);
        }

        // Assert
        Assert.Single(result);
        Assert.Equal("Event feedback", result[0].Comment);
        Assert.Equal(8, result[0].Rating);
    }

    [Fact]
    public async Task GetAllFeedbacksByUserAsync_ReturnsUserFeedback()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Создаем обязательные связанные сущности
        var testEvent = new EventDBModel(
            eventId,
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 15, 4.5
        );

        var testUser = new PersonDBModel(
            userId,
            "Test User",
            PersonType.Standart,
            true
        );

        // Тестовые данные с корректными внешними ключами
        var testData = new List<FeedbackDBModel>
    {
        new(Guid.NewGuid(), eventId, userId, "User comment", 6),
        new(Guid.NewGuid(), eventId, Guid.NewGuid(), "Other user", 9)
    };

        using (var context = new EventorDBContext(_options))
        {
            // Сохраняем связанные сущности перед отзывами
            context.Events.Add(testEvent);
            context.Persons.Add(testUser);
            context.Feedbacks.AddRange(testData);
            await context.SaveChangesAsync();
        }

        // Act
        List<Feedback> result;
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            result = await repository.GetAllFeedbacksByUserAsync(userId);
        }

        // Assert
        Assert.Single(result);
        Assert.Equal(6, result[0].Rating);
        Assert.Equal("User comment", result[0].Comment);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_ReturnsCorrectFeedback()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        // Создаем связанные сущности
        var testEvent = new EventDBModel(
            eventId,
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 15, 4.5
        );

        var testPerson = new PersonDBModel(
            personId,
            "Test User",
            PersonType.Standart,
            true
        );

        var testFeedback = new FeedbackDBModel(feedbackId, eventId, personId, "Test Feedback", 10);

        using (var context = new EventorDBContext(_options))
        {
            // Добавляем обязательные сущности
            context.Events.Add(testEvent);
            context.Persons.Add(testPerson);
            context.Feedbacks.Add(testFeedback);
            await context.SaveChangesAsync();
        }

        // Act
        Feedback result;
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            result = await repository.GetFeedbackByIdAsync(feedbackId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Feedback", result.Comment);
        Assert.Equal(10, result.Rating);
        Assert.Equal(eventId, result.EventId);
        Assert.Equal(personId, result.PersonId);
    }

    [Fact]
    public async Task InsertFeedbackAsync_AddsNewFeedbackToDatabase()
    {
        // Arrange
        var newFeedback = new Feedback(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "New comment",
            7.5
        );

        // Act
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            await repository.InsertFeedbackAsync(newFeedback);
        }

        // Assert
        using (var context = new EventorDBContext(_options))
        {
            var feedbackInDb = await context.Feedbacks.FirstOrDefaultAsync();
            Assert.NotNull(feedbackInDb);
            Assert.Equal("New comment", feedbackInDb.Comment);
        }
    }

    [Fact]
    public async Task UpdateFeedbackAsync_UpdatesExistingFeedback()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var originalFeedback = new FeedbackDBModel(feedbackId, Guid.NewGuid(), Guid.NewGuid(), "Original", 5);

        using (var context = new EventorDBContext(_options))
        {
            context.Feedbacks.Add(originalFeedback);
            await context.SaveChangesAsync();
        }

        var updatedFeedback = new Feedback(
            feedbackId,
            originalFeedback.EventId,
            originalFeedback.PersonId,
            "Updated comment",
            9
        );

        // Act
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            await repository.UpdateFeedbackAsync(updatedFeedback);
        }

        // Assert
        using (var context = new EventorDBContext(_options))
        {
            var feedbackInDb = await context.Feedbacks.FindAsync(feedbackId);
            Assert.NotNull(feedbackInDb);
            Assert.Equal("Updated comment", feedbackInDb.Comment);
            Assert.Equal(9, feedbackInDb.Rating);
        }
    }

    [Fact]
    public async Task DeleteFeedbackAsync_RemovesFeedbackFromDatabase()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var testFeedback = new FeedbackDBModel(feedbackId, Guid.NewGuid(), Guid.NewGuid(), "To delete", 3);

        using (var context = new EventorDBContext(_options))
        {
            context.Feedbacks.Add(testFeedback);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            await repository.DeleteFeedbackAsync(feedbackId);
        }

        // Assert
        using (var context = new EventorDBContext(_options))
        {
            var feedbackInDb = await context.Feedbacks.FindAsync(feedbackId);
            Assert.Null(feedbackInDb);
        }
    }

    [Fact]
    public async Task DeleteFeedbackAsync_WhenNotFound_DoesNothing()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        using (var context = new EventorDBContext(_options))
        {
            var repository = new FeedbackRepository(context);
            await repository.DeleteFeedbackAsync(nonExistentId);

            Assert.Equal(0, await context.Feedbacks.CountAsync());
        }
    }
}