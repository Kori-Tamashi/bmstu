using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы связи пользователей и мероприятий в базе данных
/// </summary>
[Table("users_events")]
public class UserEventDBModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("User")]
    [Column("user_id", TypeName = "uuid")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public UserDBModel? User { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Event")]
    [Column("event_id", TypeName = "uuid")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Мероприятие
    /// </summary>
    public EventDBModel? Event { get; set; }

    public UserEventDBModel(Guid userId, Guid eventId)
    {
        UserId = userId;
        EventId = eventId;
    }
}
