using NpgsqlTypes;
using System.Reflection;
using System.Runtime.Serialization;

namespace Eventor.Common.Enums;

/// <summary>
/// Пол пользователя
/// </summary>
public enum Gender
{
    /// <summary>
    /// Мужчина
    /// </summary>
    [PgName("Мужчина")]
    Male,

    /// <summary>
    /// Женщина
    /// </summary>
    [PgName("Женщина")]
    Female
}
