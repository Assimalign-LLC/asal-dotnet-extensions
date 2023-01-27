using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Specifies the contract for a collection of service descriptors.
/// </summary>
public interface IServiceCollection : ICollection<ServiceDescriptor>, IEnumerable<ServiceDescriptor>
{
    /// <inheritdoc />
    ServiceDescriptor this[int index] { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    int IndexOf(ServiceDescriptor item);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, ServiceDescriptor item);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    void RemoveAt(int index);
}
