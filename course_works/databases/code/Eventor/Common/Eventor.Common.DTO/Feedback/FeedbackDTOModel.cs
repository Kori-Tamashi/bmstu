using System.ComponentModel.DataAnnotations;
namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO отзыва
/// </summary>
public class FeedbackDTOModel
{
    /// <summary>
    /// Идентификатор отзыва
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid PersonId { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    /// <example>Все прошло хорошо</example>
    [Required]
    public string Comment { get; set; }

    /// <summary>
    /// Рейтинг по 10-ти балльной шкале
    /// </summary>
    /// <example>7</example>
    [Required]
    public double Rating { get; set; }

    public FeedbackDTOModel(Guid id, Guid eventId, Guid personId, string comment, double rating)
    {
        Id = id;
        EventId = eventId;
        PersonId = personId;
        Comment = comment;
        Rating = rating;
    }
}
