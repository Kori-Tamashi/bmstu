using System.Runtime.Serialization;

namespace Eventor.Common.Enums;

/// <summary>
/// Права доступа
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Гость
    /// </summary>
    [EnumMember(Value = "Гость")]
    Guest = 0,

    /// <summary>
    /// Пользователь с простым правами доступа
    /// </summary>
    [EnumMember(Value = "Пользователь")]
    User = 1,

    /// <summary>
    /// Администратор
    /// </summary>
    [EnumMember(Value = "Администратор")]
    Administrator = 2,
}
