using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing;

public interface IFileComponent
{
    /// <summary>
    /// A string containing the name of the file or directory
    /// </summary>
    string Name { get; }

    /// <summary>
    /// A string containing the full path of the file or directory
    /// </summary>
    string FullName { get; }

    /// <summary>
    /// Within a file system the parent component usually represents a 
    /// directory for the current file or directory.
    /// </summary>
    IFileComponentContainer ParentComponent { get; }
}
