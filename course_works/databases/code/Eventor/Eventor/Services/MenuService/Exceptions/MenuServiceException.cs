namespace Eventor.Services.Exceptions;

public class MenuServiceException : Exception
{
    public MenuServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class MenuNotFoundException : MenuServiceException
{
    public MenuNotFoundException(string message)
        : base(message, null) { }

    public MenuNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}

public class MenuCreateException : MenuServiceException
{
    public MenuCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class MenuUpdateException : MenuServiceException
{
    public MenuUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class MenuDeleteException : MenuServiceException
{
    public MenuDeleteException(string message, Exception inner)
        : base(message, inner) { }
}
