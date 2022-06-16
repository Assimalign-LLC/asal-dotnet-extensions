namespace Assimalign.Extensions.Hosting.Abstractions
{
    internal interface IHostConfigureContainerAdapter
    {
        void ConfigureContainer(HostBuilderContext hostContext, object containerBuilder);
    }
}
