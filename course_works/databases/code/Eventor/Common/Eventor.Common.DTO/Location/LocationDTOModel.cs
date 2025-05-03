using System.ComponentModel.DataAnnotations;
namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO локации
/// </summary>
public class LocationDTOModel
{
    /// <summary>
    /// Идентификатор локации
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Коттедж</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Домик у озера</example>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Цена аренды на 1 день
    /// </summary>
    /// <example>1000</example>
    [Required]
    public double Price { get; set; }

    /// <summary>
    /// Вместимость
    /// </summary>
    /// <example>1000</example>
    [Required]
    public int Capacity { get; set; }

    public LocationDTOModel(Guid id, string name, string description, double price, int capacity)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Capacity = capacity;
    }
}

