using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы участников в базе данных
/// </summary>
[Table("events")]
public class EventDBModel
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("event_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор локации мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Location")]
    [Column("location_id", TypeName = "uuid")]
    public Guid LocationId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Фестиваль</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Фестиваль урожая 2025 года</example>
    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Фестиваль урожая 2025 года</example>
    [Column("date", TypeName = "date")]
    public DateOnly Date { get; set; }

    /// <summary>
    /// Количество человек
    /// </summary>
    /// <example>10</example>
    [Column("person_count", TypeName = "integer")]
    public int PersonCount { get; set; }

    /// <summary>
    /// Количество дней
    /// </summary>
    /// <example>3</example>
    [Column("days_count", TypeName = "integer")]
    public int DaysCount { get; set; }

    /// <summary>
    /// Наценка в процентах
    /// </summary>
    /// <example>10</example>
    [Column("percent", TypeName = "numeric")]
    public double Percent { get; set; }

    /// <summary>
    /// Рейтинг по 10-ти балльной шкале
    /// </summary>
    /// <example>7</example>
    [Column("rating", TypeName = "numeric")]
    public double Rating { get; set; }

    public EventDBModel(Guid id, Guid locationId, string name, string description, DateOnly date,
        int personCount, int daysCount, double percent, double rating)
    {
        Id = id;
        LocationId = locationId;
        Name = name;
        Description = description;
        Date = date;
        PersonCount = personCount;
        DaysCount = daysCount;
        Percent = percent;
        Rating = rating;
    }
}

