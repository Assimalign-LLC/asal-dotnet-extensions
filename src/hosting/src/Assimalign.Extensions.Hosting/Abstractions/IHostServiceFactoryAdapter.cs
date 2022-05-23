using System;

namespace Assimalign.Extensions.Hosting.Abstractions
{
    using Assimalign.Extensions.DependencyInjection;

    internal interface IHostServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);

        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}

