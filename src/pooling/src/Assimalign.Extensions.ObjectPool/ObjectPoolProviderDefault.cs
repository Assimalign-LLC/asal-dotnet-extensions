using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.ObjectPool
{
    /// <summary>
    /// The default <see cref="ObjectPoolProvider"/>.
    /// </summary>
    public class ObjectPoolProviderDefault : ObjectPoolProvider
    {
        /// <summary>
        /// The maximum number of objects to retain in the pool.
        /// </summary>
        public int MaximumRetained { get; set; } = Environment.ProcessorCount * 2;

        /// <inheritdoc/>
        public override ObjectPool<T> Create<T>(IPooledObjectPolicy<T> policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                return new ObjectPoolDisposable<T>(policy, MaximumRetained);
            }

            return new ObjectPoolDisposable<T>(policy, MaximumRetained);
        }
    }
}
