using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Services.Exceptions;

public class DayServiceException : Exception
{
    public DayServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class DayNotFoundException : DayServiceException
{
    public DayNotFoundException(string message)
        : base(message, null) { }

    public DayNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}

public class DayCreateException : DayServiceException
{
    public DayCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class DayUpdateException : DayServiceException
{
    public DayUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class DayDeleteException : DayServiceException
{
    public DayDeleteException(string message, Exception inner)
        : base(message, inner) { }
}
