using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PatternContexts
{
    using Assimalign.Extensions.FileSystemGlobbing;

    public class PatternContextLinearExclude : PatternContextLinear
    {
        public PatternContextLinearExclude(IFileLinearPattern pattern)
            : base(pattern)
        {
        }

        public override bool Test(IFileComponentContainer directory)
        {
            if (IsStackEmpty())
            {
                throw new InvalidOperationException();// SR.CannotTestDirectory);
            }

            if (Frame.IsNotApplicable)
            {
                return false;
            }

            return IsLastSegment() && TestMatchingSegment(directory.Name);
        }
    }
}
