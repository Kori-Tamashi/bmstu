namespace Eventor.Services.Exceptions;

public class EventServiceException : Exception
{
    public EventServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class EventNotFoundException : EventServiceException
{
    public EventNotFoundException(string message)
        : base(message, null) { }

    public EventNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}

public class EventCreateException : EventServiceException
{
    public EventCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class EventUpdateException : EventServiceException
{
    public EventUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class EventDeleteException : EventServiceException
{
    public EventDeleteException(string message, Exception inner)
        : base(message, inner) { }
}