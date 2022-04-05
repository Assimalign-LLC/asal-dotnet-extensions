using System;
using System.Net.Http;

namespace Assimalign.Extensions.Http
{
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Http.Abstractions;

    /// <summary>
    /// Extensions methods for <see cref="IHttpMessageHandlerFactory"/>.
    /// </summary>
    public static class HttpMessageHandlerFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="HttpMessageHandler"/> using the default configuration.
        /// </summary>
        /// <param name="factory">The <see cref="IHttpMessageHandlerFactory"/>.</param>
        /// <returns>An <see cref="HttpMessageHandler"/> configured using the default configuration.</returns>
        public static HttpMessageHandler CreateHandler(this IHttpMessageHandlerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return factory.CreateHandler(Options<object>.DefaultName);
        }
    }
}
