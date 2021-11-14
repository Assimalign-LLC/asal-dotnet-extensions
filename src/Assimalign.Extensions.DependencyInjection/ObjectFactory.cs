using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection
{
    using Assimalign.Extensions.DependencyInjection.Utilities;

    /// <summary>
    /// The result of <see cref="ActivatorUtilities.CreateFactory(Type, Type[])"/>.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to get service arguments from.</param>
    /// <param name="arguments">Additional constructor arguments.</param>
    /// <returns>The instantiated type.</returns>
    public delegate object ObjectFactory(IServiceProvider serviceProvider, object?[]? arguments);
}
