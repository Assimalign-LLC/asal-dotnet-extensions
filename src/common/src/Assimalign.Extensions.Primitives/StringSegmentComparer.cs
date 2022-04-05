using System;
using System.Collections.Generic;


namespace Assimalign.Extensions.Primitives
{
    public class StringSegmentComparer : IComparer<StringSegment>, IEqualityComparer<StringSegment>
    {
        public static StringSegmentComparer Ordinal { get; }
            = new StringSegmentComparer(StringComparison.Ordinal, StringComparer.Ordinal);

        public static StringSegmentComparer OrdinalIgnoreCase { get; }
            = new StringSegmentComparer(StringComparison.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);

        private StringSegmentComparer(StringComparison comparison, StringComparer comparer)
        {
            Comparison = comparison;
            Comparer = comparer;
        }

        private StringComparison Comparison { get; }
        private StringComparer Comparer { get; }

        public int Compare(StringSegment x, StringSegment y)
        {
            return StringSegment.Compare(x, y, Comparison);
        }

        public bool Equals(StringSegment x, StringSegment y)
        {
            return StringSegment.Equals(x, y, Comparison);
        }

        public int GetHashCode(StringSegment obj)
        {
            return string.GetHashCode(obj.AsSpan(), Comparison);
        }
    }
}
