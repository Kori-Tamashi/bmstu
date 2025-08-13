namespace Eventor.Services.Exceptions;

public class FeedbackServiceException : Exception
{
    public FeedbackServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class FeedbackNotFoundException : FeedbackServiceException
{
    public FeedbackNotFoundException(string message)
        : base(message, null) { }
}

public class FeedbackCreateException : FeedbackServiceException
{
    public FeedbackCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class FeedbackUpdateException : FeedbackServiceException
{
    public FeedbackUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class FeedbackDeleteException : FeedbackServiceException
{
    public FeedbackDeleteException(string message, Exception inner)
        : base(message, inner) { }
}