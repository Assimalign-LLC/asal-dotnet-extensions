using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.ObjectPool
{
    /// <summary>
    /// Default implementation for <see cref="PooledObjectPolicy{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of object which is being pooled.</typeparam>
    public class PooledObjectPolicyDefault<T> : PooledObjectPolicy<T> where T : class, new()
    {
        /// <inheritdoc />
        public override T Create()
        {
            return new T();
        }

        /// <inheritdoc />
        public override bool Return(T obj)
        {
            // DefaultObjectPool<T> doesn't call 'Return' for the default policy.
            // So take care adding any logic to this method, as it might require changes elsewhere.
            return true;
        }
    }
}
