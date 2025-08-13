using NpgsqlTypes;
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
    [PgName("Простой участник")]
    Standart = 0,

    /// <summary>
    /// Участник с привилегиями
    /// </summary>
    [PgName("VIP-персона")]
    VIP = 1,

    /// <summary>
    /// Организатор
    /// </summary>
    [PgName("Организатор")]
    Organizer = 2,
}
