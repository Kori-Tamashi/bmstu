using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventor.Database.Models;

/// <summary>
/// Отзыв
/// </summary>
[Table("feedbacks")]
public class FeedbackDBModel
{
    /// <summary>
    /// Идентификатор отзыва
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("feedback_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Event")]
    [Column("event_id", TypeName = "uuid")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Person")]
    [Column("person_id", TypeName = "uuid")]
    public Guid PersonId { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    /// <example>Все прошло хорошо</example>
    [Column("comment", TypeName = "varchar(255)")]
    public string Comment { get; set; }

    /// <summary>
    /// Рейтинг по 10-ти балльной шкале
    /// </summary>
    /// <example>7</example>
    [Column("rating", TypeName = "numeric")]
    public double Rating { get; set; }

    public FeedbackDBModel(Guid id, Guid eventId, Guid personId, string comment, double rating)
    {
        Id = id;
        EventId = eventId;
        PersonId = personId;
        Comment = comment;
        Rating = rating;
    }
}

