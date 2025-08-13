using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Common.Enums;

public enum UserRole
{
    [PgName("Администратор")]
    Admin,

    [PgName("Зарегистрированный пользователь")]
    User,

    [PgName("Гость")]
    Guest
}
