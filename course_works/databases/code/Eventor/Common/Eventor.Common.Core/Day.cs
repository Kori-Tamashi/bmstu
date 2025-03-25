namespace Eventor.Common.Core;

/// <summary>
/// День мероприятия
/// </summary>
public class Day
{
    /// <summary>
    /// Идентификатор дня мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid EventId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    /// <example>Заезд</example>
    public string Name { get; set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    /// <example>2</example>
    public int SequenceNumber { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    /// <example>Заезд на локацию и разгрузка</example>
    public string Description { get; set; }

    /// <summary>
    /// Цена поездки
    /// </summary>
    /// <example>1000</example>
    public double Price { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Day)obj;
        return Id == other.Id
               && EventId == other.EventId
               && Name.Equals(other.Name)
               && SequenceNumber == other.SequenceNumber
               && Description.Equals(other.Description)
               && Price == other.Price;
    }
}