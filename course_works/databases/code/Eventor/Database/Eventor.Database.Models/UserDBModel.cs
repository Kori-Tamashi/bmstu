﻿using Eventor.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eventor.Database.Models;

/// <summary>
/// Модель таблицы пользователей в базе данных
/// </summary>
[Table("users")]
public class UserDBModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Key]
    [Column("user_id", TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    /// <example>Иван Иванов</example>
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; }

    /// <summary>
    /// Телефон
    /// </summary>
    /// <example>+79198087676</example>
    [Column("phone", TypeName = "varchar(255)")]
    public string Phone { get; set; }

    /// <summary>
    /// Гендер
    /// </summary>
    /// <example>Мужчина</example>
    [Column("gender", TypeName = "gender")]
    public Gender Gender { get; set; }

    /// <summary>
    /// Зашифрованный пароль
    /// </summary>
    [Column("password", TypeName = "varchar(255)")]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Роль
    /// </summary>
    /// <example>Пользователь</example>
    [Column("role", TypeName = "role")]
    public UserRole Role { get; set; }

    public UserDBModel(Guid id, string name, string phone, Gender gender, string passwordHash,
        UserRole role)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Gender = gender;
        PasswordHash = passwordHash;
        Role = role;
    }
}

