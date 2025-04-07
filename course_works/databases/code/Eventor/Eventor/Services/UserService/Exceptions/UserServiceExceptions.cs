namespace Eventor.Services.Exceptions;

public class UserServiceException : Exception
{
    public UserServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class UserNotFoundException : UserServiceException
{
    public UserNotFoundException(string message)
        : base(message, null) { }
}

public class UserCreateException : UserServiceException
{
    public UserCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class UserUpdateException : UserServiceException
{
    public UserUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class UserDeleteException : UserServiceException
{
    public UserDeleteException(string message, Exception inner)
        : base(message, inner) { }
}