namespace Assimalign.Extensions.Logging.Console
{
    internal readonly struct LogMessageEntry
    {
        public LogMessageEntry(string message, bool logAsError = false)
        {
            Message = message;
            LogAsError = logAsError;
        }

        public readonly string Message;
        public readonly bool LogAsError;
    }
}
