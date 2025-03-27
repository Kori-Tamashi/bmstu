using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Локация
/// </summary>
[Table("locations")]
public class LocationDBModel
{
    /// <summary>
    /// Идентификатор локации
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("location_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Коттедж</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Домик у озера</example>
    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    /// <summary>
    /// Цена аренды на 1 день
    /// </summary>
    /// <example>1000</example>
    [Column("price", TypeName = "numeric")]
    public double Price { get; set; }

    public LocationDBModel(Guid id, string name, string description, double price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }
}

