using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы связи участников и дней в базе данных
/// </summary>
[Table("persons_days")]
public class PersonDayDBModel
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("person_id", TypeName = "uuid")]
    public Guid PersonId { get; set; }

    /// <summary>
    /// Участник
    /// </summary>
    [ForeignKey("PersonId")]
    public PersonDBModel? Person { get; set; }

    /// <summary>
    /// Идентификатор дня мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("day_id", TypeName = "uuid")]
    public Guid DayId { get; set; }

    /// <summary>
    /// День
    /// </summary>
    [ForeignKey("DayId")]
    public DayDBModel? Day { get; set; }

    public PersonDayDBModel(Guid personId, Guid dayId)
    {
        PersonId = personId;
        DayId = dayId;
    }
}
