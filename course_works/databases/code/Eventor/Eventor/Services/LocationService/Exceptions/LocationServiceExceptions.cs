namespace Eventor.Services.Exceptions;

public class LocationServiceException : Exception
{
    public LocationServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class LocationNotFoundException : LocationServiceException
{
    public LocationNotFoundException(string message)
        : base(message, null) { }
}

public class LocationCreateException : LocationServiceException
{
    public LocationCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class LocationUpdateException : LocationServiceException
{
    public LocationUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class LocationDeleteException : LocationServiceException
{
    public LocationDeleteException(string message, Exception inner)
        : base(message, inner) { }
}