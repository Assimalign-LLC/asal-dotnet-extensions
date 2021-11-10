using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging.EventLog
{
    public class EventLogEntryCollection : ICollection, IEnumerable
    {
        internal EventLogEntryCollection() { }
        public int Count { get { throw null; } }
        public virtual EventLogEntry this[int index] { get { throw null; } }
        bool ICollection.IsSynchronized { get { throw null; } }
        object ICollection.SyncRoot { get { throw null; } }
        public void CopyTo(EventLogEntry[] entries, int index) { }
        public IEnumerator GetEnumerator() { throw null; }
        void ICollection.CopyTo(Array array, int index) { }
    }
}
