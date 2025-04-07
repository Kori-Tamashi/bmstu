using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Services.Exceptions;

// UserLoginAlreadyExistsException.cs
public class UserLoginAlreadyExistsException : Exception
{
    public UserLoginAlreadyExistsException(string message) : base(message) { }
}

// UserLoginNotFoundException.cs
public class UserLoginNotFoundException : Exception
{
    public UserLoginNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}

// IncorrectPasswordException.cs
public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string message) : base(message) { }
}