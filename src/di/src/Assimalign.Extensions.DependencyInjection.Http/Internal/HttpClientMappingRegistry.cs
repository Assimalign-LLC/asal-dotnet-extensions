using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection.Internal;

// Internal tracking for HTTP Client configuration. This is used to prevent some common mistakes
// that are easy to make with HTTP Client registration.
internal sealed class HttpClientMappingRegistry
{
    public Dictionary<string, Type> NamedClientRegistrations { get; } = new Dictionary<string, Type>();
}
