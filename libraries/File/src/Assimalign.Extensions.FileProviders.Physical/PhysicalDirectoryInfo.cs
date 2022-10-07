using System;
using System.IO;
using System.Collections.Generic;

namespace Assimalign.Extensions.FileProviders;

using Assimalign.Extensions.FileSystemGlobbing;
using Assimalign.Extensions.FileProviders.Internal;



/// <summary>
/// Represents a directory on a physical filesystem
/// </summary>
public class PhysicalDirectoryInfo : IFileSystemDirectoryInfo
{
    private readonly string directory;
    private readonly DirectoryInfo directoryInfo;
    private readonly ExclusionFilterType directoryInfoFilters;

    private IEnumerable<IFileSystemInfo> files; 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    public PhysicalDirectoryInfo(string directory)  
        : this(directory, ExclusionFilterType.Sensitive) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="filters"></param>
    public PhysicalDirectoryInfo(string directory, ExclusionFilterType filters) 
        : this(new DirectoryInfo(directory), filters)
    {
        this.directory = directory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryInfo"></param>
    public PhysicalDirectoryInfo(DirectoryInfo directoryInfo) 
        : this(directoryInfo, ExclusionFilterType.Sensitive) { }

    /// <summary>
    /// Initializes an instance of <see cref="PhysicalDirectoryInfo"/> that wraps an instance of <see cref="System.IO.DirectoryInfo"/>
    /// </summary>
    /// <param name="info">The directory</param>
    internal PhysicalDirectoryInfo(DirectoryInfo directoryInfo, ExclusionFilterType filters)
    {
        this.directoryInfo = directoryInfo;
    }


    private void EnsureInitialized()
    {
        try
        {
            files = directoryInfo
                .EnumerateFileSystemInfos()
                .Where(info => !FileSystemInfoHelper.IsExcluded(info, directoryInfoFilters))
                .Select<FileSystemInfo, IFileSystemInfo>(info =>
                {
                    if (info is FileInfo file)
                    {
                        return new PhysicalFileInfo(file);
                    }
                    else if (info is DirectoryInfo dir)
                    {
                        return new PhysicalDirectoryInfo(dir);
                    }
                        // shouldn't happen unless BCL introduces new implementation of base type
                        throw new InvalidOperationException();// SR.UnexpectedFileSystemInfo);
                    });
        }
        catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
        {
            files = Enumerable.Empty<IFileSystemInfo>();
        }
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


    public IFileSystemDirectoryInfo? ParentDirectory => directoryInfo.Parent is null ? null : new PhysicalDirectoryInfo(directoryInfo.Parent);

    /// <summary>
    /// Always throws an exception because read streams are not support on directories.
    /// </summary>
    /// <exception cref="InvalidOperationException">Always thrown</exception>
    /// <returns>Never returns</returns>
    public Stream CreateReadStream()
    {
        throw new InvalidOperationException();// SR.CannotCreateStream);
    }

    public IEnumerable<IFileSystemInfo> EnumerateFileSystem()
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
