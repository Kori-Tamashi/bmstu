using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Services.Exceptions;

public class EconomyServiceException : Exception
{
    public EconomyServiceException(string message) : base(message) { }

    public EconomyServiceException(string message, Exception ex) : base(message, ex) { }
}
