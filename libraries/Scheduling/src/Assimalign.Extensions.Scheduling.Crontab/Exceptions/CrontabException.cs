using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public class CrontabException : Exception
{
    public CrontabException() :
        base("Crontab error.")
    { } // TODO: Fix message and add it to resource.

    public CrontabException(string? message) :
        base(message)
    { }

    public CrontabException(string? message, Exception? innerException) :
        base(message, innerException)
    { }
}
