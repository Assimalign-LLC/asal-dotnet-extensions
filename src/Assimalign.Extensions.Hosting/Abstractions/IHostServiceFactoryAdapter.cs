using System;

namespace Assimalign.Extensions.Hosting.Abstractions
{
    using Assimalign.Extensions.DependencyInjection.Abstractions;

    internal interface IHostServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);

        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}

