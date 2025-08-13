using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Меню конкретного дня мероприятия
/// </summary>
[Table("menu")]
public class MenuDBModel
{
    /// <summary>
    /// Идентификатор меню
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("menu_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Основное меню</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Стоимость
    /// </summary>
    /// <example>1000</example>
    [Column("cost", TypeName = "numeric")]
    public double Cost { get; set; }

    /// <summary>
    /// Предметы меню
    /// </summary>
    public List<MenuItemsDBModel> MenuItems { get; set; } = new();

    public MenuDBModel(Guid id, string name, double cost)
    {
        Id = id;
        Name = name;
        Cost = cost;
    }
}

