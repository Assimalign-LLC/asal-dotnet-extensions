using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging.EventLog
{
    public enum OverflowAction
    {
        DoNotOverwrite = -1,
        OverwriteAsNeeded = 0,
        OverwriteOlder = 1,
    }
}
