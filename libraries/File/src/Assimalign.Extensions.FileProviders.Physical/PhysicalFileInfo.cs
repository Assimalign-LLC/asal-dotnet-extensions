using System;
using System.IO;


namespace Assimalign.Extensions.FileProviders.Physical;

using Assimalign.Extensions.FileSystemGlobbing;


/// <summary>
/// Represents a file on a physical filesystem
/// </summary>
public class PhysicalFileInfo : IFileSystemInfo
{
    private readonly FileInfo fileInfo;

    /// <summary>
    /// Initializes an instance of <see cref="PhysicalFileInfo"/> that wraps an instance of <see cref="System.IO.FileInfo"/>
    /// </summary>
    /// <param name="fileInfo">The <see cref="System.IO.FileInfo"/></param>
    public PhysicalFileInfo(FileInfo fileInfo)
    {
        this.fileInfo = fileInfo;
    }

    /// <inheritdoc />
    public bool Exists => fileInfo.Exists;

    /// <inheritdoc />
    public long Length => fileInfo.Length;


    /// <inheritdoc />
    public string Name => fileInfo.Name;

    /// <inheritdoc />
    public DateTimeOffset LastModified => fileInfo.LastWriteTimeUtc;

    /// <summary>
    /// Always false.
    /// </summary>
    public bool IsDirectory => false;

    /// <inheritdoc />
    public string FullName => fileInfo.FullName;

    public IFileSystemDirectoryInfo? ParentDirectory => fileInfo.Directory is null ? null : new PhysicalDirectoryInfo(fileInfo.Directory);

    /// <inheritdoc />
    public Stream CreateReadStream()
    {
        // We are setting buffer size to 1 to prevent FileStream from allocating it's internal buffer
        // 0 causes constructor to throw
        int bufferSize = 1;
        return new FileStream(
            FullName,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite,
            bufferSize,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
    }
}
