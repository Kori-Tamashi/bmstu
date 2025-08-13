using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Services.Exceptions;

public class PersonServiceException : Exception
{
    public PersonServiceException(string message, Exception inner)
        : base(message, inner) { }
}

public class PersonNotFoundException : PersonServiceException
{
    public PersonNotFoundException(string message)
        : base(message, null) { }

    public PersonNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}

public class PersonCreateException : PersonServiceException
{
    public PersonCreateException(string message, Exception inner)
        : base(message, inner) { }
}

public class PersonUpdateException : PersonServiceException
{
    public PersonUpdateException(string message, Exception inner)
        : base(message, inner) { }
}

public class PersonDeleteException : PersonServiceException
{
    public PersonDeleteException(string message, Exception inner)
        : base(message, inner) { }
}
