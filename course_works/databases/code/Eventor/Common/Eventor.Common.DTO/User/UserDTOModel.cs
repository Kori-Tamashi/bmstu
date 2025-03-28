using Eventor.Common.Enums;
using System.ComponentModel.DataAnnotations;
namespace Eventor.Common.DTO;

/// <summary>
/// Модель DTO пользователя
/// </summary>
public class UserDTOModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    /// <example>Иван Иванов</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Телефон
    /// </summary>
    /// <example>+79198087676</example>
    [Required]
    public string Phone { get; set; }

    /// <summary>
    /// Гендер
    /// </summary>
    /// <example>Мужчина</example>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Зашифрованный пароль
    /// </summary>
    [Required]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Роль
    /// </summary>
    /// <example>Пользователь</example>
    [Required]
    public UserRole Role { get; set; }

    public UserDTOModel(Guid id, string name, string phone, Gender gender, string passwordHash, UserRole role)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Gender = gender;
        PasswordHash = passwordHash;
        Role = role;
    }
}

