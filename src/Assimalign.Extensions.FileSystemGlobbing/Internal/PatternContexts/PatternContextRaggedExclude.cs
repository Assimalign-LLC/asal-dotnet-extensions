using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PatternContexts
{
    using Assimalign.Extensions.FileSystemGlobbing.Abstractions;

    public class PatternContextRaggedExclude : PatternContextRagged
    {
        public PatternContextRaggedExclude(IFileRaggedPattern pattern)
            : base(pattern)
        {
        }

        public override bool Test(DirectoryInfoBase directory)
        {
            if (IsStackEmpty())
            {
                throw new InvalidOperationException();// SR.CannotTestDirectory);
            }

            if (Frame.IsNotApplicable)
            {
                return false;
            }

            if (IsEndingGroup() && TestMatchingGroup(directory))
            {
                // directory excluded with file-like pattern
                return true;
            }

            if (Pattern.EndsWith.Count == 0 &&
                Frame.SegmentGroupIndex == Pattern.Contains.Count - 1 &&
                TestMatchingGroup(directory))
            {
                // directory excluded by matching up to final '/**'
                return true;
            }

            return false;
        }
    }
}
