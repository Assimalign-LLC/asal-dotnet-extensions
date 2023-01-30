


using System;
using System.Collections;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassImplementingIEnumerable : IEnumerable
    {
        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }
}
