using Eventor.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Services;

/// <summary>
/// Интерфейс сервис авторизации 
/// </summary>
public interface IOauthService
{
    /// <summary>
    /// Зарегистрировать пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <param name="password">Пароль в явном виде</param>
    /// <exception cref="UserCreateException">При добавлении пользователя</exception>
    Task Registrate(User user, string password);

    /// <summary>
    /// Авторизовать пользователя
    /// </summary>
    /// <param name="login">логин - связан с email-ом</param>
    /// <param name="password">пароль</param>
    /// <returns>Данные пользователя</returns>
    /// <exception cref="IncorrectPasswordException">Пароль неверный</exception>
    /// <exception cref="UserLoginNotFoundException">Логин не найден</exception>
    Task<User> Login(string phone, string password);
}