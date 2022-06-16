using System;

namespace Assimalign.Extensions.Hosting.Internal
{
    using Assimalign.Extensions.Hosting;

    internal sealed class HostConfigureContainerAdapter<TContainerBuilder> : IHostConfigureContainerAdapter
    {
        private Action<HostBuilderContext, TContainerBuilder> action;

        public HostConfigureContainerAdapter(
            Action<HostBuilderContext, TContainerBuilder> action)
        {
            this.action = action ?? 
                throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(
            HostBuilderContext hostContext, 
            object containerBuilder)
        {
            this.action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }
}
