using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal
{
    using Assimalign.Extensions.FileSystemGlobbing.Abstractions;



    /// <summary>
    /// This API supports infrastructure and is not intended to be used
    /// directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public interface IPatternContext
    {
        void Declare(Action<IPathSegment, bool> onDeclare);

        bool Test(DirectoryInfoBase directory);

        PatternTestResult Test(FileInfoBase file);

        void PushDirectory(DirectoryInfoBase directory);

        void PopDirectory();
    }
}
