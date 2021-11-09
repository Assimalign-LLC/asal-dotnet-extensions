using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PatternContexts
{
    using Assimalign.Extensions.FileSystemGlobbing.Abstractions;

    public abstract class PatternContext<TFrame> : IPatternContext
    {
        private Stack<TFrame> _stack = new Stack<TFrame>();
        protected TFrame Frame;

        public virtual void Declare(Action<IPathSegment, bool> declare) { }

        public abstract PatternTestResult Test(FileInfoBase file);

        public abstract bool Test(DirectoryInfoBase directory);

        public abstract void PushDirectory(DirectoryInfoBase directory);

        public virtual void PopDirectory()
        {
            Frame = _stack.Pop();
        }

        protected void PushDataFrame(TFrame frame)
        {
            _stack.Push(Frame);
            Frame = frame;
        }

        protected bool IsStackEmpty()
        {
            return _stack.Count == 0;
        }
    }
}
