using System;
using System.Threading;

namespace Assimalign.Extensions.FileProviders.Abstractions
{
    using Assimalign.Extensions.Primitives.Abstractions;

    internal interface IPollingChangeToken : IChangeToken
    {
        CancellationTokenSource CancellationTokenSource { get; }
    }
}
