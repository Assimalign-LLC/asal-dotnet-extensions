using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging.EventLog
{
    public partial class EventInstance
    {
        public EventInstance(long instanceId, int categoryId) { }
        public EventInstance(long instanceId, int categoryId, EventLogEntryType entryType) { }
        public int CategoryId { get { throw null; } set { } }
        public EventLogEntryType EntryType { get { throw null; } set { } }
        public long InstanceId { get { throw null; } set { } }
    }
}
