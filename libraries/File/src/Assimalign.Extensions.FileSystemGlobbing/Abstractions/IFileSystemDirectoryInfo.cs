using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.FileSystemGlobbing;

public interface IFileSystemDirectoryInfo : IFileSystemInfo
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<IFileSystemInfo> EnumerateFileSystem();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IFileSystemDirectoryInfo? GetDirectory(string path);
    /// <summary>
    ///  Returns the <see cref="IFileSystemInfo"/> for a given file 
    ///  from the current location of a given directory.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IFileSystemInfo? GetFile(string path);
}
