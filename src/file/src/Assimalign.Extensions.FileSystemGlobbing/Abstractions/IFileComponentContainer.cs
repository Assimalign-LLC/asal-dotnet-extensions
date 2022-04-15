using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing;

public interface IFileComponentContainer : IFileComponent
{
    /// <summary>
    /// Enumerates all files and directories in the directory.
    /// </summary>
    /// <returns>Collection of files and directories</returns>
    IEnumerable<IFileComponent> EnumerateFileComponents();

    /// <summary>
    /// Returns an instance of <see cref="FileDirectoryInfo" /> that represents a subdirectory
    /// </summary>
    /// <param name="path">The directory name</param>
    /// <returns>Instance of <see cref="FileDirectoryInfo" /> even if directory does not exist</returns>
    IFileComponentContainer GetContainer(string path);

    /// <summary>
    /// Returns an instance of <see cref="FileInfo" /> that represents a file in the directory
    /// </summary>
    /// <param name="path">The file name</param>
    /// <returns>Instance of <see cref="FileInfo" /> even if file does not exist</returns>
    IFileComponent GetComponent(string path);
}
