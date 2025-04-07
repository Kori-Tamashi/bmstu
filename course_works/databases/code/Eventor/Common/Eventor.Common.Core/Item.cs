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
    /// Стоимость
    /// </summary>
    /// <example>1000</example>
    public double Price { get; set; }

    public Item(Guid id, string name, ItemType type, double price)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
    }

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