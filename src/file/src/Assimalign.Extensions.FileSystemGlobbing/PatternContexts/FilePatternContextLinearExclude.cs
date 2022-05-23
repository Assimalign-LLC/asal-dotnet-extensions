using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.PatternContexts;

public class FilePatternContextLinearExclude : FilePatternContextLinear
{
    public FilePatternContextLinearExclude(IFileLinearPattern pattern)
        : base(pattern)
    {
    }

    public override bool Test(IFileSystemDirectoryInfo directory)
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
