using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.FileSystemGlobbing;

public interface IFileSystemDirectoryInfo : IFileSystemInfo
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<IFileSystemInfo> EnumerateFilesystemInfos();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IFileSystemDirectoryInfo? GetDirectory(string path);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IFileSystemInfo? GetFile(string path);
}
