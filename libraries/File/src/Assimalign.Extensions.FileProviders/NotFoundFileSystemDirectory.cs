using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders;

using Assimalign.Extensions.FileSystemGlobbing;
using System.IO;

public class NotFoundFileSystemDirectory : IFileSystemDirectoryInfo
{

    public static NotFoundFileSystemDirectory Singleton => new NotFoundFileSystemDirectory();

    public string Name => throw new NotImplementedException();

    public string FullName => throw new NotImplementedException();

    public bool Exists => throw new NotImplementedException();

    public long Length => throw new NotImplementedException();

    public DateTimeOffset LastModified => throw new NotImplementedException();

    public bool IsDirectory => throw new NotImplementedException();

    public IFileSystemDirectoryInfo ParentDirectory => throw new NotImplementedException();

    public Stream CreateReadStream()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IFileSystemInfo> EnumerateFileSystem()
    {
        throw new NotImplementedException();
    }

    public IFileSystemDirectoryInfo GetDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public IFileSystemInfo GetFile(string path)
    {
        throw new NotImplementedException();
    }
}
