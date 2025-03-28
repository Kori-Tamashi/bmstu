using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы меню и предметов в базе данных
/// </summary>
[Table("menu_items")]
public class MenuItemsDBModel
{
    /// <summary>
    /// Идентификатор меню
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("menu_id", TypeName = "uuid")]
    public Guid MenuId { get; set; }

    /// <summary>
    /// Меню
    /// </summary>
    public MenuDBModel? Menu { get; set; }

    /// <summary>
    /// Идентификатор предмета
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("item_id", TypeName = "uuid")]
    public Guid ItemId { get; set; }

    /// <summary>
    /// Предмет
    /// </summary>
    public ItemDBModel? Item { get; set; }

    /// <summary>
    /// Количество предметов
    /// </summary>
    /// <example>10</example>
    [Column("amount", TypeName = "integer")]
    public int Amount { get; set; }

    public MenuItemsDBModel(Guid menuId, Guid itemId, int amount)
    {
        MenuId = menuId;
        ItemId = itemId;
        Amount = amount;
    }
}

