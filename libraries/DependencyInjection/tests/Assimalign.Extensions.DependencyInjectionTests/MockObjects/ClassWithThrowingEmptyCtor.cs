


using System;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithThrowingEmptyCtor
    {
        public ClassWithThrowingEmptyCtor()
        {
            throw new Exception(nameof(ClassWithThrowingEmptyCtor));
        }
    }
}
