using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders.Abstractions
{
    /// <summary>
    /// Represents a directory's content in the file provider.
    /// </summary>
    public interface IDirectoryContents : IEnumerable<IFileInfo>
    {
        /// <summary>
        /// True if a directory was located at the given path.
        /// </summary>
        bool Exists { get; }
    }
}
