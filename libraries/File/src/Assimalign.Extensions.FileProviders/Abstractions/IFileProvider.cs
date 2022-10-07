using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders;

using Assimalign.Extensions.Primitives;
using Assimalign.Extensions.FileSystemGlobbing;


/// <summary>
/// A read-only file provider abstraction.
/// </summary>
public interface IFileProvider
{
    /// <summary>
    /// Locate a file at the given path.
    /// </summary>
    /// <param name="subpath">Relative path that identifies the file.</param>
    /// <returns>The file information. Caller must check Exists property.</returns>
    IFileSystemInfo GetFile(string subpath);

    /// <summary>
    /// Enumerate a directory at the given path, if any.
    /// </summary>
    /// <param name="subpath">Relative path that identifies the directory.</param>
    /// <returns>Returns the contents of the directory.</returns>
    IFileSystemDirectoryInfo GetDirectory(string subpath);

    /// <summary>
    /// Creates a <see cref="IChangeToken"/> for the specified <paramref name="filter"/>.
    /// </summary>
    /// <param name="filter">Filter string used to determine what files or folders to monitor. Example: **/*.cs, *.*, subFolder/**/*.cshtml.</param>
    /// <returns>An <see cref="IChangeToken"/> that is notified when a file matching <paramref name="filter"/> is added, modified or deleted.</returns>
    IChangeToken Watch(string filter);
}
