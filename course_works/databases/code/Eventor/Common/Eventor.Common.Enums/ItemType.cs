using System.Runtime.Serialization;

namespace Eventor.Common.Enums;

/// <summary>
/// Тип предмета
/// </summary>
public enum ItemType
{
    /// <summary>
    /// Однодневный
    /// </summary>
    [EnumMember(Value = "Однодневный")]
    OneDay = 0,

    /// <summary>
    /// Многодневный
    /// </summary>
    [EnumMember(Value = "Многодневный")]
    MultiDay = 1,
}
