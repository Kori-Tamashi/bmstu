using NpgsqlTypes;
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
    [PgName("Однодневный")]
    OneDay = 0,

    /// <summary>
    /// Многодневный
    /// </summary>
    [PgName("Многодневный")]
    MultiDay = 1,
}
