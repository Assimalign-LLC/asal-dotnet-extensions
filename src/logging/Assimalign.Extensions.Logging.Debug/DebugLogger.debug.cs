#define DEBUG

namespace Assimalign.Extensions.Logging.Debug
{
    internal sealed partial class DebugLogger
    {
        private void DebugWriteLine(string message, string name)
        {
            System.Diagnostics.Debug.WriteLine(message, category: name);
        }
    }
}
