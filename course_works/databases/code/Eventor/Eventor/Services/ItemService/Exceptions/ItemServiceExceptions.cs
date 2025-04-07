namespace Eventor.Services.Exceptions;

public class ItemServiceException : Exception
{
    public ItemServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class ItemNotFoundException : ItemServiceException
{
    public ItemNotFoundException(string message)
        : base(message, null) { }
}

public class ItemCreateException : ItemServiceException
{
    public ItemCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class ItemUpdateException : ItemServiceException
{
    public ItemUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class ItemDeleteException : ItemServiceException
{
    public ItemDeleteException(string message, Exception inner)
        : base(message, inner) { }
}
