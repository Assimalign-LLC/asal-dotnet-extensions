using System;
using System.Collections;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Specifies the contract for a collection of service descriptors.
/// </summary>
public interface IServiceCollection :
    ICollection<ServiceDescriptor>,
    IEnumerable<ServiceDescriptor>
{
    ServiceDescriptor this[int index] { get; set; }
    int IndexOf(ServiceDescriptor item);
    void Insert(int index, ServiceDescriptor item);
    void RemoveAt(int index);
}
