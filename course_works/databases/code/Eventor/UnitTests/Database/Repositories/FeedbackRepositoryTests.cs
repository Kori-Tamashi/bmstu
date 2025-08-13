using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Context;
using Eventor.Database.Models;
using Eventor.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Eventor.Tests.Database.Repositories;

public class FeedbackRepositoryTests : IDisposable
{
    private readonly EventorDBContext _context;
    private readonly ILogger<FeedbackRepository> _logger;
    private readonly FeedbackRepository _repository;
    private readonly DbContextOptions<EventorDBContext> _options;

    public FeedbackRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<EventorDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventorDBContext(_options);
        _logger = Mock.Of<ILogger<FeedbackRepository>>();
        _repository = new FeedbackRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private FeedbackDBModel CreateTestFeedback(
        Guid? eventId = null,
        Guid? personId = null,
        string comment = "Test Comment",
        double rating = 5.0)
    {
        var feedbackId = Guid.NewGuid();
        var testEvent = new EventDBModel(
            eventId ?? Guid.NewGuid(),
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 15, 4.5);

        var testPerson = new PersonDBModel(
            personId ?? Guid.NewGuid(),
            "Test User",
            PersonType.Standart,
            true);

        _context.Events.Add(testEvent);
        _context.Persons.Add(testPerson);
        _context.SaveChanges();

        return new FeedbackDBModel(
            feedbackId,
            testEvent.Id,
            testPerson.Id,
            comment,
            rating);
    }

    [Fact]
    public async Task GetAllFeedbackAsync_ShouldReturnAllFeedback()
    {
        // Arrange
        var feedbacks = new List<FeedbackDBModel>
        {
            CreateTestFeedback(comment: "Feedback 1"),
            CreateTestFeedback(comment: "Feedback 2")
        };

        await _context.Feedbacks.AddRangeAsync(feedbacks);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllFeedbackAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Comment == "Feedback 1");
        Assert.Contains(result, f => f.Comment == "Feedback 2");
    }

    [Fact]
    public async Task GetAllFeedbackAsync_ShouldLogErrorOnFailure()
    {
        // Arrange
        var corruptedContext = new Mock<EventorDBContext>(_options);
        corruptedContext.Setup(c => c.Feedbacks).Throws<Exception>();
        var repository = new FeedbackRepository(corruptedContext.Object, _logger);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetAllFeedbackAsync());
        Mock.Get(_logger).VerifyLog(LogLevel.Error, "Ошибка получения списка отзывов");
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_ShouldReturnCorrectFeedback()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        await _context.Feedbacks.AddAsync(testFeedback);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetFeedbackByIdAsync(testFeedback.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testFeedback.Comment, result.Comment);
        Assert.Equal(testFeedback.Rating, result.Rating);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_ShouldReturnNullForNonExistingId()
    {
        // Act
        var result = await _repository.GetFeedbackByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertFeedbackAsync_ShouldAddNewFeedback()
    {
        // Arrange
        // Создаем и сохраняем тестовые Event и Person
        var testEvent = new EventDBModel(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            10, 2, 15, 4.5);

        var testPerson = new PersonDBModel(
            Guid.NewGuid(),
            "Test User",
            PersonType.Standart,
            true);

        await _context.Events.AddAsync(testEvent);
        await _context.Persons.AddAsync(testPerson);
        await _context.SaveChangesAsync();

        // Создаем отзыв с валидными внешними ключами
        var newFeedback = new Feedback(
            Guid.NewGuid(),
            testEvent.Id,
            testPerson.Id,
            "New Feedback",
            8.5);

        // Act
        await _repository.InsertFeedbackAsync(newFeedback);
        var result = await _context.Feedbacks.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newFeedback.Comment, result.Comment);
        Assert.Equal(testEvent.Id, result.EventId);
        Assert.Equal(testPerson.Id, result.PersonId);
    }

    [Fact]
    public async Task InsertFeedbackAsync_ShouldThrowOnInvalidData()
    {
        // Arrange
        var invalidFeedback = new Feedback(
            Guid.NewGuid(),
            Guid.NewGuid(), // Несуществующий Event
            Guid.NewGuid(), // Несуществующий Person
            "Invalid",
            10);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _repository.InsertFeedbackAsync(invalidFeedback)
        );

        Assert.Contains("не найден", exception.Message);
    }

    [Fact]
    public async Task UpdateFeedbackAsync_ShouldUpdateExistingFeedback()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        await _context.Feedbacks.AddAsync(testFeedback);
        await _context.SaveChangesAsync();

        var updatedFeedback = new Feedback(
            testFeedback.Id,
            testFeedback.EventId,
            testFeedback.PersonId,
            "Updated Comment",
            9.5);

        // Act
        await _repository.UpdateFeedbackAsync(updatedFeedback);
        var result = await _context.Feedbacks.FindAsync(testFeedback.Id);

        // Assert
        Assert.Equal("Updated Comment", result.Comment);
        Assert.Equal(9.5, result.Rating);
    }

    [Fact]
    public async Task UpdateFeedbackAsync_ShouldNotThrowForNonExistingFeedback()
    {
        // Arrange
        var nonExistingFeedback = new Feedback(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Non-existing",
            1);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            _repository.UpdateFeedbackAsync(nonExistingFeedback));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task DeleteFeedbackAsync_ShouldRemoveFeedback()
    {
        // Arrange
        var testFeedback = CreateTestFeedback();
        await _context.Feedbacks.AddAsync(testFeedback);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteFeedbackAsync(testFeedback.Id);
        var result = await _context.Feedbacks.FindAsync(testFeedback.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteFeedbackAsync_ShouldLogWarningForNonExistingId()
    {
        // Act
        await _repository.DeleteFeedbackAsync(Guid.NewGuid());

        // Assert
        Mock.Get(_logger).VerifyLog(LogLevel.Warning, "не найден");
    }
}