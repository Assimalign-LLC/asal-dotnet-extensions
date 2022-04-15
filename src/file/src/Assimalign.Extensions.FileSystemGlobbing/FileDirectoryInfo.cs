using System;
using System.IO;
using System.Collections.Generic;

namespace Assimalign.Extensions.FileSystemGlobbing;

public class FileDirectoryInfo : IFileComponentContainer
{
    private readonly bool isParentPath;
    private readonly DirectoryInfo directoryInfo;

    /// <summary>
    /// Initializes an instance of <see cref="FileDirectoryInfoWrapper" />.
    /// </summary>
    /// <param name="directoryInfo">The <see cref="DirectoryInfo" />.</param>
    public FileDirectoryInfo(DirectoryInfo directoryInfo) : this(directoryInfo, isParentPath: false) { }

    private FileDirectoryInfo(DirectoryInfo directoryInfo, bool isParentPath)
    {
        this.directoryInfo = directoryInfo;
        this.isParentPath = isParentPath;
    }

    /// <inheritdoc />
    public IEnumerable<IFileComponent> EnumerateFileComponents()
    {
        if (directoryInfo.Exists)
        {
            IEnumerable<FileSystemInfo> fileSystemInfos;
            
            try
            {
                fileSystemInfos = directoryInfo.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is DirectoryInfo directoryInfo)
                {
                    yield return new FileDirectoryInfo(directoryInfo);
                }
                else if (fileSystemInfo is System.IO.FileInfo fileInfo)
                {
                    yield return new FileInfo(fileInfo);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<FileDirectoryInfo> EnumerateDirectories()
    {
        if (directoryInfo.Exists)
        {
            IEnumerable<FileSystemInfo> fileSystemInfos;

            try
            {
                fileSystemInfos = directoryInfo.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is DirectoryInfo directoryInfo)
                {
                    yield return new FileDirectoryInfo(directoryInfo);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<FileInfo> EnumerateFiles()
    {
        if (directoryInfo.Exists)
        {
            IEnumerable<FileSystemInfo> fileSystemInfos;

            try
            {
                fileSystemInfos = directoryInfo.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is System.IO.FileInfo fileInfo)
                {
                    yield return new FileInfo(fileInfo);
                }
            }
        }
    }

    /// <summary>
    /// Returns an instance of <see cref="FileDirectoryInfo" /> that represents a subdirectory.
    /// </summary>
    /// <remarks>
    /// If <paramref name="name" /> equals '..', this returns the parent directory.
    /// </remarks>
    /// <param name="name">The directory name</param>
    /// <returns>The directory</returns>
    public FileDirectoryInfo GetDirectory(string name)
    {
        bool isParentPath = string.Equals(name, "..", StringComparison.Ordinal);

        if (isParentPath)
        {
            return new FileDirectoryInfo(
                new DirectoryInfo(Path.Combine(directoryInfo.FullName, name)),
                isParentPath);
        }
        else
        {
            DirectoryInfo[] dirs = directoryInfo.GetDirectories(name);

            if (dirs.Length == 1)
            {
                return new FileDirectoryInfo(dirs[0], isParentPath);
            }
            else if (dirs.Length == 0)
            {
                return null;
            }
            else
            {
                // This shouldn't happen. The parameter name isn't supposed to contain wild card.
                throw new InvalidOperationException(
                    $"More than one sub directories are found under {directoryInfo.FullName} with name {name}.");
            }
        }
    }
    IFileComponentContainer IFileComponentContainer.GetContainer(string name) => this.GetDirectory(name);

    /// <inheritdoc />
    public FileInfo GetFile(string name) => new FileInfo(new System.IO.FileInfo(Path.Combine(directoryInfo.FullName, name)));
    IFileComponent IFileComponentContainer.GetComponent(string path) => this.GetFile(path);

    /// <inheritdoc />
    public string Name => isParentPath ? ".." : directoryInfo.Name;

    /// <summary>
    /// Returns the full path to the directory.
    /// </summary>
    /// <remarks>
    /// Equals the value of <seealso cref="System.IO.FileSystemInfo.FullName" />.
    /// </remarks>
    public string FullName => directoryInfo.FullName;

    /// <summary>
    /// Returns the parent directory.
    /// </summary>
    /// <remarks>
    /// Equals the value of <seealso cref="System.IO.DirectoryInfo.Parent" />.
    /// </remarks>
    public FileDirectoryInfo ParentDirectory => new FileDirectoryInfo(directoryInfo.Parent);
    IFileComponentContainer IFileComponent.ParentComponent => ParentDirectory;
}
