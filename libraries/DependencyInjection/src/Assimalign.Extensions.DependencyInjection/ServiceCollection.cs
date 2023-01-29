using System;
using System.Collections;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Default implementation of <see cref="IServiceCollection"/>.
/// </summary>
public sealed class ServiceCollection : IServiceCollection
{
    private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();

    public ServiceCollection()
    {
            
    }
    public ServiceCollection(IEnumerable<ServiceDescriptor> descriptors)
    {
        this.descriptors.AddRange(descriptors);
    }


    /// <inheritdoc />
    public int Count => descriptors.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public ServiceDescriptor this[int index]
    {
        get
        {
            return descriptors[index];
        }
        set
        {
            descriptors[index] = value;
        }
    }

    /// <inheritdoc />
    public void Clear() => descriptors.Clear();

    /// <inheritdoc />
    public bool Contains(ServiceDescriptor item) => descriptors.Contains(item);

    /// <inheritdoc />
    public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => descriptors.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(ServiceDescriptor item) => descriptors.Remove(item);

    /// <inheritdoc />
    public IEnumerator<ServiceDescriptor> GetEnumerator() => descriptors.GetEnumerator();

    void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item) => descriptors.Add(item);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    

    /// <inheritdoc />
    public int IndexOf(ServiceDescriptor item) => descriptors.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, ServiceDescriptor item) => descriptors.Insert(index, item);

    /// <inheritdoc />
    public void RemoveAt(int index) => descriptors.RemoveAt(index);
}
