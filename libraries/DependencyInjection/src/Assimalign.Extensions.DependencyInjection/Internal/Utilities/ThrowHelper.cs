using System;
using System.Runtime.CompilerServices;

namespace Assimalign.Extensions.DependencyInjection.Internal;

internal static class ThrowHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowObjectDisposedException()
    {
        throw new ObjectDisposedException(nameof(IServiceProvider));
    }
}
