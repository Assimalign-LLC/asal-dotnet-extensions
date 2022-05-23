using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.ObjectPool
{
    /// <summary>
    /// A base type for <see cref="IPooledObjectPolicy{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of object which is being pooled.</typeparam>
    public abstract class PooledObjectPolicy<T> : IPooledObjectPolicy<T> where T : notnull
    {
        /// <inheritdoc />
        public abstract T Create();

        /// <inheritdoc />
        public abstract bool Return(T obj);
    }
}
