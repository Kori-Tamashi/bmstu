using System.ComponentModel.DataAnnotations;

namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO меню
/// </summary>
public class MenuDTOModel
{
    /// <summary>
    /// Идентификатор меню
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Основное меню</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Стоимость
    /// </summary>
    /// <example>1000</example>
    [Required]
    public double Cost { get; set; }

    public MenuDTOModel(Guid id, string name, double cost)
    {
        Id = id;
        Name = name;
        Cost = cost;
    }
}

