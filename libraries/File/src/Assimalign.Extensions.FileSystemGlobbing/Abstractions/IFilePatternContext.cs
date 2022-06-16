using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing;


/// <summary>
/// This API supports infrastructure and is not intended to be used
/// directly from your code. This API may change or be removed in future releases.
/// </summary>
public interface IFilePatternContext
{
    void Declare(Action<IFilePathSegment, bool> onDeclare);

    bool Test(IFileSystemDirectoryInfo directory);

    FilePatternTestResult Test(IFileSystemInfo file);

    void PushDirectory(IFileSystemDirectoryInfo directory);

    void PopDirectory();
}
