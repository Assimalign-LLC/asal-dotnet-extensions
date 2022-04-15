using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Internal.PatternContexts;

using Assimalign.Extensions.FileSystemGlobbing;

public abstract class PatternContext<TFrame> : IFilePatternContext
{
    private Stack<TFrame> _stack = new Stack<TFrame>();
    protected TFrame Frame;

    public virtual void Declare(Action<IFilePathSegment, bool> declare) { }

    public abstract FilePatternTestResult Test(IFileComponent file);

    public abstract bool Test(IFileComponentContainer directory);

    public abstract void PushDirectory(IFileComponentContainer directory);

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
