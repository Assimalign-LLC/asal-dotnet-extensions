using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PatternContexts
{
    using Assimalign.Extensions.FileSystemGlobbing.Abstractions;

    public class PatternContextLinearInclude : PatternContextLinear
    {
        public PatternContextLinearInclude(IFileLinearPattern pattern)
            : base(pattern)
        {
        }

        public override void Declare(Action<IFilePathSegment, bool> onDeclare)
        {
            if (IsStackEmpty())
            {
                throw new InvalidOperationException();// SR.CannotDeclarePathSegment);
            }

            if (Frame.IsNotApplicable)
            {
                return;
            }

            if (Frame.SegmentIndex < Pattern.Segments.Count)
            {
                onDeclare(Pattern.Segments[Frame.SegmentIndex], IsLastSegment());
            }
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

            return !IsLastSegment() && TestMatchingSegment(directory.Name);
        }
    }
}
