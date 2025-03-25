namespace Eventor.Common.Core;

/// <summary>
/// Мероприятие
/// </summary>
public class Event
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор локации мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid LocationId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Фестиваль</example>
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Фестиваль урожая 2025 года</example>
    public string Description { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Фестиваль урожая 2025 года</example>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Количество человек
    /// </summary>
    /// <example>10</example>
    public int PersonCount { get; set; }

    /// <summary>
    /// Количество дней
    /// </summary>
    /// <example>3</example>
    public int DaysCount { get; set; }

    /// <summary>
    /// Наценка в процентах
    /// </summary>
    /// <example>10</example>
    public double Percent { get; set; }

    /// <summary>
    /// Рейтинг по 10-ти балльной шкале
    /// </summary>
    /// <example>7</example>
    public double Rating { get; set; }

    public Event(Guid id, Guid locationId, string name, string description, DateOnly date,
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

    /// <summary>
    /// Проверяет, истекло ли мероприятие (текущая дата позже даты окончания)
    /// </summary>
    public bool IsEventExpired()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly endDate = Date.AddDays(DaysCount);
        return today > endDate;
    }

    /// <summary>
    /// Проверяет, активно ли мероприятие (текущая дата в промежутке [Date; Date + DaysCount])
    /// </summary>
    public bool IsEventActive()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly endDate = Date.AddDays(DaysCount);
        return today >= Date && today <= endDate;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Event)obj;
        return Id == other.Id
               && LocationId == other.LocationId
               && Name.Equals(other.Name)
               && Description.Equals(other.Description)
               && Date.Equals(other.Date)
               && PersonCount == other.PersonCount
               && DaysCount == other.DaysCount
               && Percent == other.Percent
               && Rating == other.Rating;
    }
}
