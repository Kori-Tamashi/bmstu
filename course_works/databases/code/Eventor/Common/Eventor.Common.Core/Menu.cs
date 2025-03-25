namespace Eventor.Common.Core;

/// <summary>
/// День мероприятия
/// </summary>
public class Menu
{
    /// <summary>
    /// Идентификатор меню
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Основное меню</example>
    public string Name { get; set; }

    /// <summary>
    /// Стоимость
    /// </summary>
    /// <example>1000</example>
    public double Cost { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Menu)obj;
        return Id == other.Id
               && Name.Equals(other.Name)
               && Cost == other.Cost;
    }
}