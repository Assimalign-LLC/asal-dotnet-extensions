using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal
{
    using Assimalign.Extensions.FileSystemGlobbing.Abstractions;

    internal sealed class InMemoryFileInfo : FileInfoBase
    {
        private InMemoryDirectoryInfo _parent;

        public InMemoryFileInfo(string file, InMemoryDirectoryInfo parent)
        {
            FullName = file;
            Name = Path.GetFileName(file);
            _parent = parent;
        }

        public override string FullName { get; }

        public override string Name { get; }

        public override DirectoryInfoBase ParentDirectory
        {
            get
            {
                return _parent;
            }
        }
    }
}
