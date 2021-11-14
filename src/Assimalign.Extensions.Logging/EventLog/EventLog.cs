using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Assimalign.Extensions.Logging.EventLog
{
    [DefaultEventAttribute("EntryWritten")]
    public class EventLog : Component, ISupportInitialize
    {
        public EventLog() { }
        public EventLog(string logName) { }
        public EventLog(string logName, string machineName) { }
        public EventLog(string logName, string machineName, string source) { }
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool EnableRaisingEvents { get { throw null; } set { } }
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public EventLogEntryCollection Entries { get { throw null; } }
        [DefaultValueAttribute("")]
        [ReadOnlyAttribute(true)]
        [SettingsBindableAttribute(true)]
        public string Log { get { throw null; } set { } }
        [System.ComponentModel.BrowsableAttribute(false)]
        public string LogDisplayName { get { throw null; } }
        [System.ComponentModel.DefaultValueAttribute(".")]
        [System.ComponentModel.ReadOnlyAttribute(true)]
        [System.ComponentModel.SettingsBindableAttribute(true)]
        public string MachineName { get { throw null; } set { } }
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public long MaximumKilobytes { get { throw null; } set { } }
        [System.ComponentModel.BrowsableAttribute(false)]
        public int MinimumRetentionDays { get { throw null; } }
        [BrowsableAttribute(false)]
        public OverflowAction OverflowAction { get { throw null; } }
        [DefaultValueAttribute("")]
        [ReadOnlyAttribute(true)]
        [SettingsBindableAttribute(true)]
        public string Source { get { throw null; } set { } }
        [BrowsableAttribute(false)]
        [DefaultValueAttribute(null)]
        public ISynchronizeInvoke SynchronizingObject { get { throw null; } set { } }
        public event EntryWrittenEventHandler EntryWritten { add { } remove { } }
        public void BeginInit() { }
        public void Clear() { }
        public void Close() { }
        public static void CreateEventSource(EventSourceCreationData sourceData) { }
        public static void CreateEventSource(string source, string logName) { }
        [System.ObsoleteAttribute("EventLog.CreateEventSource has been deprecated. Use System.Diagnostics.EventLog.CreateEventSource(EventSourceCreationData sourceData) instead.")]
        public static void CreateEventSource(string source, string logName, string machineName) { }
        public static void Delete(string logName) { }
        public static void Delete(string logName, string machineName) { }
        public static void DeleteEventSource(string source) { }
        public static void DeleteEventSource(string source, string machineName) { }
        protected override void Dispose(bool disposing) { }
        public void EndInit() { }
        public static bool Exists(string logName) { throw null; }
        public static bool Exists(string logName, string machineName) { throw null; }
        public static EventLog[] GetEventLogs() { throw null; }
        public static EventLog[] GetEventLogs(string machineName) { throw null; }
        public static string LogNameFromSourceName(string source, string machineName) { throw null; }
        public void ModifyOverflowPolicy(OverflowAction action, int retentionDays) { }
        public void RegisterDisplayName(string resourceFile, long resourceId) { }
        public static bool SourceExists(string source) { throw null; }
        public static bool SourceExists(string source, string machineName) { throw null; }
        public void WriteEntry(string message) { }
        public void WriteEntry(string message, EventLogEntryType type) { }
        public void WriteEntry(string message, EventLogEntryType type, int eventID) { }
        public void WriteEntry(string message, EventLogEntryType type, int eventID, short category) { }
        public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData) { }
        public static void WriteEntry(string source, string message) { }
        public static void WriteEntry(string source, string message, EventLogEntryType type) { }
        public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID) { }
        public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category) { }
        public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData) { }
        public void WriteEvent(EventInstance instance, byte[] data, params object[] values) { }
        public void WriteEvent(EventInstance instance, params object[] values) { }
        public static void WriteEvent(string source, EventInstance instance, byte[] data, params object[] values) { }
        public static void WriteEvent(string source, EventInstance instance, params object[] values) { }
    }
}
