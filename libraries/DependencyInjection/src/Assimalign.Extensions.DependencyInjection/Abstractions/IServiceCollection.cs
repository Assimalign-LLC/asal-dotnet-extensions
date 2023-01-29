using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Specifies the contract for a collection of service descriptors.
/// </summary>
public interface IServiceCollection : IList<ServiceDescriptor>
{
    /// <summary>
    /// Determines if the specified service type is available from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceType">An object that specifies the type of service object to test.</param>
    /// <returns>true if the specified service is a available, false if it is not.</returns>
    //bool IsService(Type serviceType);
}
