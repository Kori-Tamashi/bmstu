using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Common.Enums;

public static class EnumConverter
{
    public static Gender ToGender(this string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Строка не может быть пустой.");

        foreach (var gender in Enum.GetValues(typeof(Gender)).Cast<Gender>())
        {
            var memberInfo = typeof(Gender).GetMember(gender.ToString())[0];
            var pgNameAttribute = memberInfo.GetCustomAttribute<PgNameAttribute>();

            if (pgNameAttribute != null &&
                string.Equals(pgNameAttribute.PgName, value, StringComparison.OrdinalIgnoreCase))
            {
                return gender;
            }
        }

        throw new ArgumentException($"Недопустимое значение: {value}");
    }

    public static string ToString(this Gender gender)
    {
        var memberInfo = typeof(Gender).GetMember(gender.ToString()).FirstOrDefault();
        if (memberInfo == null)
            throw new ArgumentException($"Недопустимое значение перечисления: {gender}");

        var pgNameAttribute = memberInfo.GetCustomAttribute<PgNameAttribute>();
        return pgNameAttribute?.PgName ?? gender.ToString();
    }

    public static UserRole ToUserRole(this string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Строка не может быть пустой.");

        foreach (var role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>())
        {
            var memberInfo = typeof(UserRole).GetMember(role.ToString())[0];
            var pgNameAttribute = memberInfo.GetCustomAttribute<PgNameAttribute>();

            if (pgNameAttribute != null &&
                string.Equals(pgNameAttribute.PgName, value, StringComparison.OrdinalIgnoreCase))
            {
                return role;
            }
        }

        throw new ArgumentException($"Недопустимое значение: {value}");
    }

    public static string ToString(this UserRole role)
    {
        var memberInfo = typeof(UserRole).GetMember(role.ToString()).FirstOrDefault();
        if (memberInfo == null)
            throw new ArgumentException($"Недопустимое значение перечисления: {role}");

        var pgNameAttribute = memberInfo.GetCustomAttribute<PgNameAttribute>();
        return pgNameAttribute?.PgName ?? role.ToString();
    }
}
