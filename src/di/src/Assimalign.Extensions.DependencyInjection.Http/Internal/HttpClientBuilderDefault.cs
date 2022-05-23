namespace Assimalign.Extensions.DependencyInjection.Internal;


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
