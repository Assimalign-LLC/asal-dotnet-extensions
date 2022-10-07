
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders;

using Assimalign.Extensions.Primitives;
using Assimalign.Extensions.FileSystemGlobbing;

public abstract class FileProvider : IFileProvider
{
    public abstract IFileSystemDirectoryInfo GetDirectory(string subpath);
    public abstract IFileSystemInfo GetFile(string subpath);
    public abstract IChangeToken Watch(string filter);


    /// <summary>
    /// 
    /// </summary>
    public static IFileProvider Null => new NullFileProvider();

    private class NullFilePovider : IFileProvider
    {
        public IFileSystemDirectoryInfo GetDirectory(string subpath)
        {
            
        }

        public IFileSystemInfo GetFile(string subpath)
        {
            
        }

        public IChangeToken Watch(string filter)
        {
           
        }
    }
}
