using System;
using System.Net.Http;


namespace Assimalign.Extensions.Http
{
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Http.Abstractions;

    /// <summary>
    /// Extensions methods for <see cref="IHttpClientFactory"/>.
    /// </summary>
    public static class HttpClientFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="HttpClient"/> using the default configuration.
        /// </summary>
        /// <param name="factory">The <see cref="IHttpClientFactory"/>.</param>
        /// <returns>An <see cref="HttpClient"/> configured using the default configuration.</returns>
        public static HttpClient CreateClient(this IHttpClientFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return factory.CreateClient(Options<object>.DefaultName);
        }
    }
}
