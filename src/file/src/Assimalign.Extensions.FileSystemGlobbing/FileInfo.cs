using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing;

public class FileInfo : IFileComponent
{
    private readonly System.IO.FileInfo _fileInfo;

    /// <summary>
    /// Initializes instance of <see cref="FileInfoWrapper" /> to wrap the specified object <see cref="System.IO.FileInfo" />.
    /// </summary>
    /// <param name="fileInfo">The <see cref="System.IO.FileInfo" /></param>
    public FileInfo(System.IO.FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    /// <summary>
    /// The file name. (Overrides <see cref="FileSystemInfo.Name" />).
    /// </summary>
    /// <remarks>
    /// Equals the value of <see cref="System.IO.FileInfo.Name" />.
    /// </remarks>
    public string Name => _fileInfo.Name;

    /// <summary>
    /// The full path of the file. (Overrides <see cref="FileSystemInfo.FullName" />).
    /// </summary>
    /// <remarks>
    /// Equals the value of <see cref="System.IO.FileSystemInfo.Name" />.
    /// </remarks>
    public string FullName => _fileInfo.FullName;

    /// <summary>
    /// The directory containing the file. (Overrides <see cref="FileSystemInfo.ParentDirectory" />).
    /// </summary>
    /// <remarks>
    /// Equals the value of <see cref="System.IO.FileInfo.Directory" />.
    /// </remarks>
    public FileDirectoryInfo ParentDirectory => new FileDirectoryInfo(_fileInfo.Directory);
    IFileComponentContainer IFileComponent.ParentComponent => this.ParentDirectory;
}
