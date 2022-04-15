using System;
using System.Threading;

namespace Assimalign.Extensions.FileProviders;

using Assimalign.Extensions.Primitives;

internal interface IPollingChangeToken : IStateToken
{
    CancellationTokenSource CancellationTokenSource { get; }
}
