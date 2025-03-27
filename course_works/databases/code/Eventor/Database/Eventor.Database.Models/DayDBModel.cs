using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы дней мероприятия в базе данных
/// </summary>
[Table("days")]
public class DayDBModel
{
    /// <summary>
    /// Идентификатор дня мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("day_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Event")]
    [Column("event_id", TypeName = "uuid")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Заезд</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    /// <example>2</example>
    [Column("sequence_number", TypeName = "integer")]
    public int SequenceNumber { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Заезд на локацию и разгрузка</example>
    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    /// <summary>
    /// Цена поездки
    /// </summary>
    /// <example>1000</example>
    [Column("price", TypeName = "numeric")]
    public double Price { get; set; }

    public DayDBModel(Guid id, Guid eventId, string name, int sequenceNumber, 
        string description, double price)
    {
        Id = id;
        EventId = eventId;
        Name = name;
        SequenceNumber = sequenceNumber;
        Description = description;
        Price = price;
    }
}

