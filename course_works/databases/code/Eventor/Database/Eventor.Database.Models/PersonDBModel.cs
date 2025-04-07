using Eventor.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель базы данных участников
/// </summary>
[Table("persons")]
public class PersonDBModel
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("person_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    /// <example>Иван Иванов</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    /// <example>VIP</example>
    [Column("type", TypeName = "person_type")]
    public PersonType Type { get; set; }

    /// <summary>
    /// Факт оплаты
    /// </summary>
    /// <example>Заезд</example>
    [Column("paid", TypeName = "bool")]
    public bool Paid { get; set; }

    /// <summary>
    /// Выбранные дни
    /// </summary>
    public List<PersonDayDBModel> SelectedDays { get; set; }

    public PersonDBModel(Guid id, string name, PersonType type, bool paid)
    {
        Id = id;
        Name = name;
        Type = type;
        Paid = paid;
    }
}

