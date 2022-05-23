using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Default implementation of <see cref="IServiceCollection"/>.
/// </summary>
public class ServiceCollection : IServiceCollection
{
    private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();


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
    public void Clear()
    {
        descriptors.Clear();
    }

    /// <inheritdoc />
    public bool Contains(ServiceDescriptor item)
    {
        return descriptors.Contains(item);
    }

    /// <inheritdoc />
    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        descriptors.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public bool Remove(ServiceDescriptor item)
    {
        return descriptors.Remove(item);
    }

    /// <inheritdoc />
    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return descriptors.GetEnumerator();
    }

    void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
    {
        descriptors.Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public int IndexOf(ServiceDescriptor item)
    {
        return descriptors.IndexOf(item);
    }

    /// <inheritdoc />
    public void Insert(int index, ServiceDescriptor item)
    {
        descriptors.Insert(index, item);
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        descriptors.RemoveAt(index);
    }
}
