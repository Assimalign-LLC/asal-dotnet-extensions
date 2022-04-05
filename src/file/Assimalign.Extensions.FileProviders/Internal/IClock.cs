using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders.Internal
{
    internal interface IClock
    {
        DateTime UtcNow { get; }
    }
}
