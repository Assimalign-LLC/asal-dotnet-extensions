using System;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TService"></typeparam>
public interface IServiceFactory<out TService>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    TService Create(object serviceKey)
}