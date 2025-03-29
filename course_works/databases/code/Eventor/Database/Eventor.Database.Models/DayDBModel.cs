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
    /// Идентификатор меню
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Menu")]
    [Column("menu_id", TypeName = "uuid")]
    public Guid MenuId { get; set; }

    /// <summary>
    /// Меню
    /// </summary>
    public MenuDBModel Menu { get; set; }

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

    /// <summary>
    /// Дни мероприятия
    /// </summary>
    public List<EventDayDBModel> EventDays { get; set; } = new();

    /// <summary>
    /// Участники дня
    /// </summary>
    public List<PersonDayDBModel> PersonDays { get; set; } = new();

    public DayDBModel(Guid id, Guid menuId, string name, int sequenceNumber, 
        string description, double price)
    {
        Id = id;
        MenuId = menuId;
        Name = name;
        SequenceNumber = sequenceNumber;
        Description = description;
        Price = price;
    }
}

