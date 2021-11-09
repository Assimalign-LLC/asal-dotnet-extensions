using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    internal readonly struct CallSiteServiceCacheKey : IEquatable<CallSiteServiceCacheKey>
    {
        public static CallSiteServiceCacheKey Empty { get; } = new CallSiteServiceCacheKey(null, 0);

        /// <summary>
        /// Type of service being cached
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Reverse index of the service when resolved in <c>IEnumerable&lt;Type&gt;</c> where default instance gets slot 0.
        /// For example for service collection
        ///  IService Impl1
        ///  IService Impl2
        ///  IService Impl3
        /// We would get the following cache keys:
        ///  Impl1 2
        ///  Impl2 1
        ///  Impl3 0
        /// </summary>
        public int Slot { get; }

        public CallSiteServiceCacheKey(Type type, int slot)
        {
            Type = type;
            Slot = slot;
        }

        public bool Equals(CallSiteServiceCacheKey other)
        {
            return Type == other.Type && Slot == other.Slot;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type?.GetHashCode() ?? 23) * 397) ^ Slot;
            }
        }
    }
}
