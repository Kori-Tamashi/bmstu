using System.ComponentModel.DataAnnotations.Schema;

namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы пользователя и участника базе данных
/// </summary>
[Table("users_persons")]
public class UserPersonDBModel
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
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [ForeignKey("Person")]
    [Column("person_id", TypeName = "uuid")]
    public Guid PersonId { get; set; }

    /// <summary>
    /// Мероприятие
    /// </summary>
    public PersonDBModel? Person { get; set; }

    public UserPersonDBModel(Guid userId, Guid personId)
    {
        UserId = userId;
        PersonId = personId;
    }
}
