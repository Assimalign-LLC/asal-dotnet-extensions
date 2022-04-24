using System;
using System.IO;

namespace Assimalign.Extensions.FileProviders.Physical;

using Assimalign.Extensions.FileSystemGlobbing;
using System.Collections.Generic;


/// <summary>
/// Represents a directory on a physical filesystem
/// </summary>
public class PhysicalDirectoryInfo : IFileSystemDirectoryInfo
{
    private readonly DirectoryInfo directoryInfo;

    /// <summary>
    /// Initializes an instance of <see cref="PhysicalDirectoryInfo"/> that wraps an instance of <see cref="System.IO.DirectoryInfo"/>
    /// </summary>
    /// <param name="info">The directory</param>
    public PhysicalDirectoryInfo(DirectoryInfo directoryInfo)
    {
        this.directoryInfo = directoryInfo;
    }

    /// <inheritdoc />
    public bool Exists => directoryInfo.Exists;

    /// <summary>
    /// Always equals -1.
    /// </summary>
    public long Length => -1;

    /// <inheritdoc />
    public string PhysicalPath => directoryInfo.FullName;

    /// <inheritdoc />
    public string Name => directoryInfo.Name;

    /// <summary>
    /// The time when the directory was last written to.
    /// </summary>
    public DateTimeOffset LastModified => directoryInfo.LastWriteTimeUtc;

    /// <summary>
    /// Always true.
    /// </summary>
    public bool IsDirectory => true;

    /// <inheritdoc />
    public string FullName => directoryInfo.FullName;

    public IFileSystemDirectoryInf?o ParentDirectory => directoryInfo.Parent is null ? null : new PhysicalDirectoryInfo(directoryInfo.Parent);

    /// <summary>
    /// Always throws an exception because read streams are not support on directories.
    /// </summary>
    /// <exception cref="InvalidOperationException">Always thrown</exception>
    /// <returns>Never returns</returns>
    public Stream CreateReadStream()
    {
        throw new InvalidOperationException();// SR.CannotCreateStream);
    }

    public IEnumerable<IFileSystemInfo> EnumerateFilesystemInfos()
    {
        throw new NotImplementedException();
    }

    public IFileSystemDirectoryInfo? GetDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public IFileSystemInfo? GetFile(string path)
    {
        throw new NotImplementedException();
    }
}
