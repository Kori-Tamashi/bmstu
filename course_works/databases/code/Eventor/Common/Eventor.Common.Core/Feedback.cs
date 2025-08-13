namespace Eventor.Common.Core;

/// <summary>
/// Отзыв
/// </summary>
public class Feedback
{
    /// <summary>
    /// Идентификатор отзыва
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid EventId { get; set; }

    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    /// <example>Все прошло хорошо</example>
    public string Comment { get; set; }

    /// <summary>
    /// Рейтинг по 10-ти балльной шкале
    /// </summary>
    /// <example>7</example>
    public double Rating { get; set; }

    public Feedback(Guid id, Guid eventId, Guid personId, string comment, double rating)
    {
        Id = id;
        EventId = eventId;
        PersonId = personId;
        Comment = comment;
        Rating = rating;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Feedback)obj;
        return Id == other.Id
               && EventId == other.EventId
               && PersonId == other.PersonId
               && Comment.Equals(other.Comment)
               && Rating == other.Rating;
    }
}