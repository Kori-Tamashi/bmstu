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
    [EnumMember(Value = "Мужчина")]
    Male = 0,

    /// <summary>
    /// Женщина
    /// </summary>
    [EnumMember(Value = "Женщина")]
    Female = 1
}
