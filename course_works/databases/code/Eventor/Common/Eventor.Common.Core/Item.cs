using Eventor.Common.Enums;
namespace Eventor.Common.Core;

/// <summary>
/// Предмет
/// </summary>
public class Item
{
    /// <summary>
    /// Идентификатор предмета
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Бутылка воды (3л.)</example>
    public string Name { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    /// <example>VIP</example>
    public ItemType Type { get; set; }

    /// <summary>
    /// Цена поездки
    /// </summary>
    /// <example>1000</example>
    public double Price { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Item)obj;
        return Id == other.Id
               && Name.Equals(other.Name)
               && Type == other.Type
               && Price == other.Price;
    }
}