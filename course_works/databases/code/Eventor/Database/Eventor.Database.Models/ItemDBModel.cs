using Eventor.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Предмет
/// </summary>
[Table("items")]
public class ItemDBModel
{
    /// <summary>
    /// Идентификатор предмета
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("item_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Бутылка воды (3л.)</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    /// <example>VIP</example>
    [Column("type", TypeName = "item_type_enum")]
    public ItemType Type { get; set; }

    /// <summary>
    /// Цена поездки
    /// </summary>
    /// <example>1000</example>
    [Column("price", TypeName = "numeric")]
    public double Price { get; set; }

    public ItemDBModel(Guid id, string name, ItemType type, double price)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
    }
}
