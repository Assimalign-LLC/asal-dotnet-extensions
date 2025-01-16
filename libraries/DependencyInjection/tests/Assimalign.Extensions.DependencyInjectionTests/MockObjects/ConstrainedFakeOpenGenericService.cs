﻿


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ConstrainedFakeOpenGenericService<TVal> : IFakeOpenGenericService<TVal>
        where TVal : PocoClass
    {
        public ConstrainedFakeOpenGenericService(TVal value)
        {
            Value = value;
        }
        public TVal Value { get; }
    }
}