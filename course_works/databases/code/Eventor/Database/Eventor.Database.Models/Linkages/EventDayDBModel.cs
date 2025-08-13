using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы связи мероприятий и дней в базе данных
/// </summary>
[Table("events_days")]
public class EventDayDBModel
{
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

    /// <summary>
    /// Идентификатор дня мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Day")]
    [Column("day_id", TypeName = "uuid")]
    public Guid DayId { get; set; }

    // <summary>
    /// Конкретный день
    /// </summary>
    public DayDBModel? Day { get; set; }

    public EventDayDBModel(Guid eventId, Guid dayId)
    {
        EventId = eventId;
        DayId = dayId;
    }
}
