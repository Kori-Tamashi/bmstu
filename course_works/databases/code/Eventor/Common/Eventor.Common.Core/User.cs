using Eventor.Common.Enums;
namespace Eventor.Common.Core;

/// <summary>
/// Пользователь
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    /// <example>Иван Иванов</example>
    public string Name { get; set; }

    /// <summary>
    /// Телефон
    /// </summary>
    /// <example>+79198087676</example>
    public string Phone { get; set; }

    /// <summary>
    /// Гендер
    /// </summary>
    /// <example>Мужчина</example>
    public Gender Gender { get; set; }

    /// <summary>
    /// Зашифрованный пароль
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Роль
    /// </summary>
    /// <example>Пользователь</example>
    public UserRole Role { get; set; }

    public User(Guid id, string name, string phone, Gender gender, string passwordHash, UserRole role)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Gender = gender;
        PasswordHash = passwordHash;
        Role = role;
    }


    /// <summary>
    /// Изменяет роль пользователя
    /// </summary>
    public void ChangePermission(UserRole role)
    {
        Role = role;
    }

    /// <summary>
    /// Генерирует хеш пароля и сохраняет его в свойство PasswordHash
    /// </summary>
    public void CreateHash(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Проверяет соответствие пароля сохраненному хешу
    /// </summary>
    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (User)obj;
        return Id == other.Id
               && Name.Equals(other.Name)
               && Phone.Equals(other.Phone)
               && Gender == other.Gender
               && PasswordHash.Equals(other.PasswordHash)
               && Role == other.Role;
    }
}
