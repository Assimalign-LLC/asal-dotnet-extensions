


using System;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithThrowingCtor
    {
        public ClassWithThrowingCtor(IFakeService service)
        {
            throw new Exception(nameof(ClassWithThrowingCtor));
        }
    }
}
