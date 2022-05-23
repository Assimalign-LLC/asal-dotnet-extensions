using System.Linq;

namespace Assimalign.Extensions.Linq.Serialization.Extensions
{
    internal static class ObjectExtensions
    {
        public static bool IsEqualToAny<T>(this T item, params T[] items)
        {
            return items.Contains(item);
        }
    }
}
