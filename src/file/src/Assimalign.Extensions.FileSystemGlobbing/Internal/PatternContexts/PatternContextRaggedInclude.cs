using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PatternContexts
{
    using Assimalign.Extensions.FileSystemGlobbing;
    using Assimalign.Extensions.FileSystemGlobbing.Internal.PathSegments;

    public class PatternContextRaggedInclude : PatternContextRagged
    {
        public PatternContextRaggedInclude(IFileRaggedPattern pattern)
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

            if (IsStartingGroup() && Frame.SegmentIndex < Frame.SegmentGroup.Count)
            {
                onDeclare(Frame.SegmentGroup[Frame.SegmentIndex], false);
            }
            else
            {
                onDeclare(WildcardPathSegment.MatchAll, false);
            }
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

            if (IsStartingGroup() && !TestMatchingSegment(directory.Name))
            {
                // deterministic not-included
                return false;
            }

            return true;
        }
    }
}
