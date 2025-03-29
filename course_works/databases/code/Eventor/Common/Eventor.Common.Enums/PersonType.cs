using System.Runtime.Serialization;

namespace Eventor.Common.Enums;

/// <summary>
/// Тип участника
/// </summary>
public enum PersonType
{
    /// <summary>
    /// Участник без привилегий
    /// </summary>
    [EnumMember(Value = "Обычный участник")]
    Standart = 0,

    /// <summary>
    /// Участник с привилегиями
    /// </summary>
    [EnumMember(Value = "VIP")]
    VIP = 1,

    /// <summary>
    /// Организатор
    /// </summary>
    [EnumMember(Value = "Организатор")]
    Organizer = 2,
}
