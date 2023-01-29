using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// A factory for creating named <see cref="IServiceProvider"/> instances.
/// </summary>
public interface IServiceProviderFactory
{
    /// <summary>
    /// Creates a default <see cref="IServiceProvider"/> instance.
    /// </summary>
    /// <returns></returns>
    IServiceProvider Create();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProviderName"></param>
    /// <returns></returns>
    IServiceProvider Create(string serviceProviderName);
}