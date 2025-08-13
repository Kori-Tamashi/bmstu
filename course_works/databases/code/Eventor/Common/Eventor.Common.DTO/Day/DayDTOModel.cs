using System.ComponentModel.DataAnnotations;

namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO конкретного дня мероприятия
/// </summary>
public class DayDTOModel
{
    /// <summary>
    /// Идентификатор дня мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор меню
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid MenuId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Заезд</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    /// <example>2</example>
    [Required]
    public int SequenceNumber { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Заезд на локацию и разгрузка</example>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Цена поездки
    /// </summary>
    /// <example>1000</example>
    [Required]
    public double Price { get; set; }

    public DayDTOModel(Guid id, Guid menuId, string name, int sequenceNumber, string description, double price)
    {
        Id = id;
        MenuId = menuId;
        Name = name;
        SequenceNumber = sequenceNumber;
        Description = description;
        Price = price;
    }
}

