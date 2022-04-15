

namespace Assimalign.Extensions.Http
{
    using Assimalign.Extensions.Http;
    using Assimalign.Extensions.DependencyInjection;

    internal sealed class HttpClientBuilderDefault : IHttpClientBuilder
    {
        public HttpClientBuilderDefault(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}
