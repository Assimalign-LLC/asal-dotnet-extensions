using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging.EventLog
{
    public class EntryWrittenEventArgs : EventArgs
    {
        public EntryWrittenEventArgs() { }
        public EntryWrittenEventArgs(EventLogEntry entry) { }
        public EventLogEntry Entry { get { throw null; } }
    }
}
