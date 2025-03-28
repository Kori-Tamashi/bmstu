using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO мероприятия
/// </summary>
public class EventDTOModel
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор локации мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid LocationId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Фестиваль</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Фестиваль урожая 2025 года</example>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Фестиваль урожая 2025 года</example>
    [Required]
    public DateOnly Date { get; set; }

    /// <summary>
    /// Количество человек
    /// </summary>
    /// <example>10</example>
    [Required]
    public int PersonCount { get; set; }

    /// <summary>
    /// Количество дней
    /// </summary>
    /// <example>3</example>
    [Required]
    public int DaysCount { get; set; }

    /// <summary>
    /// Наценка в процентах
    /// </summary>
    /// <example>10</example>
    [Required]
    public double Percent { get; set; }

    /// <summary>
    /// Рейтинг по 10-ти балльной шкале
    /// </summary>
    /// <example>7</example>
    [Required]
    public double Rating { get; set; }

    public EventDTOModel(Guid id, Guid locationId, string name, string description, DateOnly date,
        int personCount, int daysCount, double percent, double rating)
    {
        Id = id;
        LocationId = locationId;
        Name = name;
        Description = description;
        Date = date;
        PersonCount = personCount;
        DaysCount = daysCount;
        Percent = percent;
        Rating = rating;
    }
}
