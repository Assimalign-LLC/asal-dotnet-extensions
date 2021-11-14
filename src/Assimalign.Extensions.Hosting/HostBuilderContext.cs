using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Hosting.Abstractions;
    using Assimalign.Extensions.Configuration.Abstractions;

    public class HostBuilderContext
    {
        public HostBuilderContext(IDictionary<object, object> properties)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// The <see cref="IHostEnvironment" /> initialized by the <see cref="IHost" />.
        /// </summary>
        public IHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="IHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }
}
