


using System;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassImplementingIComparable : IComparable<ClassImplementingIComparable>
    {
        public int CompareTo(ClassImplementingIComparable other) => 0;
    }
}
