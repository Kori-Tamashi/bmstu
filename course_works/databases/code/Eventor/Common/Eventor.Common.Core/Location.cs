namespace Eventor.Common.Core;

/// <summary>
/// Локация
/// </summary>
public class Location
{
    /// <summary>
    /// Идентификатор локации
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Коттедж</example>
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Домик у озера</example>
    public string Description { get; set; }

    /// <summary>
    /// Цена аренды на 1 день
    /// </summary>
    /// <example>1000</example>
    public double Price { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Location)obj;
        return Id == other.Id
               && Name.Equals(other.Name)
               && Description.Equals(other.Description)
               && Price == other.Price;
    }
}