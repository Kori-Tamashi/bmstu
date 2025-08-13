using Eventor.Common.Enums;
namespace Eventor.Common.Core;

/// <summary>
/// Участник мероприятия
/// </summary>
public class Person
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    /// <example>Иван Иванов</example>
    public string Name { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    /// <example>VIP</example>
    public PersonType Type { get; set; }

    /// <summary>
    /// Факт оплаты
    /// </summary>
    /// <example>Заезд</example>
    public bool Paid { get; set; }

    public Person(Guid id, string name, PersonType type, bool paid)
    {
        Id = id;
        Name = name;
        Type = type;
        Paid = paid;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Person)obj;
        return Id == other.Id
               && Name.Equals(other.Name)
               && Type == other.Type
               && Paid == other.Paid;
    }
}