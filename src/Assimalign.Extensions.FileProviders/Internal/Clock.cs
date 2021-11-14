using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders.Internal
{
    internal sealed class Clock : IClock
    {
        public static readonly Clock Instance = new Clock();

        private Clock()
        {
        }

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
