using Eventor.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO предмета меню 
/// </summary>
public class ItemDTOModel
{
    /// <summary>
    /// Идентификатор предмета
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Бутылка воды (3л.)</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    /// <example>VIP</example>
    [Required]
    public ItemType Type { get; set; }

    /// <summary>
    /// Цена поездки
    /// </summary>
    /// <example>1000</example>
    [Required]
    public double Price { get; set; }

    public ItemDTOModel(Guid id, string name, ItemType type, double price)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
    }
}
