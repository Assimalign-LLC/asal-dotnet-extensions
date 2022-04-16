using System;

namespace Assimalign.Extensions.Net.Http;

/// <summary>
/// Used by the <see cref="HttpClientFactory"/> to apply additional initialization to the configure the
/// <see cref="HttpMessageHandlerBuilder"/> immediately before <see cref="HttpMessageHandlerBuilder.Build()"/>
/// is called.
/// </summary>
public interface IHttpMessageHandlerBuilderFilter
{
    /// <summary>
    /// Applies additional initialization to the <see cref="HttpMessageHandlerBuilder"/>
    /// </summary>
    /// <param name="next">A delegate which will run the next <see cref="IHttpMessageHandlerBuilderFilter"/>.</param>
    Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next);
}
