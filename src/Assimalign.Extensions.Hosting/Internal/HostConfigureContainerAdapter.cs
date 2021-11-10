using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Internal
{
    internal sealed class HostConfigureContainerAdapter<TContainerBuilder> : IHostConfigureContainerAdapter
    {
        private Action<HostBuilderContext, TContainerBuilder> action;

        public HostConfigureContainerAdapter(Action<HostBuilderContext, TContainerBuilder> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(HostBuilderContext hostContext, object containerBuilder)
        {
            this.action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }
}
