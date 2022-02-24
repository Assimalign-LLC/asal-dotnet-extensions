using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PathSegments
{
    public class CurrentPathSegment : IFilePathSegment
    {
        public bool CanProduceStem { get { return false; } }

        public bool Match(string value)
        {
            return false;
        }
    }
}
