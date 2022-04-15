using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders
{
    using Assimalign.Extensions.Primitives;
    using Assimalign.Extensions.FileProviders;

    /// <summary>
    /// An empty file provider with no contents.
    /// </summary>
    public class NullFileProvider : IFileProvider
    {
        /// <summary>
        /// Enumerate a non-existent directory.
        /// </summary>
        /// <param name="subpath">A path under the root directory. This parameter is ignored.</param>
        /// <returns>A <see cref="IFileDirectoryContent"/> that does not exist and does not contain any contents.</returns>
        public IFileDirectoryContent GetDirectoryContents(string subpath) => NotFoundDirectoryContents.Singleton;

        /// <summary>
        /// Locate a non-existent file.
        /// </summary>
        /// <param name="subpath">A path under the root directory.</param>
        /// <returns>A <see cref="IFileInfo"/> representing a non-existent file at the given path.</returns>
        public IFileInfo GetFileInfo(string subpath) => new NotFoundFileInfo(subpath);

        /// <summary>
        /// Returns a <see cref="IStateToken"/> that monitors nothing.
        /// </summary>
        /// <param name="filter">Filter string used to determine what files or folders to monitor. This parameter is ignored.</param>
        /// <returns>A <see cref="IStateToken"/> that does not register callbacks.</returns>
        public IStateToken Watch(string filter) => NullChangeToken.Singleton;
    }
}
