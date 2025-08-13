using Eventor.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO участника мероприятия
/// </summary>
public class PersonDTOModel
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    /// <example>Иван Иванов</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    /// <example>VIP</example>
    [Required]
    public PersonType Type { get; set; }

    /// <summary>
    /// Факт оплаты
    /// </summary>
    /// <example>Заезд</example>
    [Required]
    public bool Paid { get; set; }

    public PersonDTOModel(Guid id, string name, PersonType type, bool paid)
    {
        Id = id;
        Name = name;
        Type = type;
        Paid = paid;
    }
}

