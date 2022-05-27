using System;
using System.Net.Http;
using System.Collections.Concurrent;

namespace Assimalign.Extensions.Net.Http.Internal;

internal static class InvocationCache
{
    public static ConcurrentDictionary<string, HttpClient> clientCache = new();
    public static ConcurrentDictionary<string, HttpMessageHandler> handlerCache = new();

    public static Func<string, HttpClient> MemoiseClient(Func<string, HttpClient> method)
    {
        return input => clientCache.TryGetValue(input, out var client) ?
            client :
            clientCache[input] = method.Invoke(input);
    }

    public static Func<string, HttpMessageHandler> MemoiseHandler(Func<string, HttpMessageHandler> method)
    {
        return input => handlerCache.TryGetValue(input, out var handler) ?
           handler :
           handlerCache[input] = method.Invoke(input);
    }
}
