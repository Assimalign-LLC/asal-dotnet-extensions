using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Options.Abstractions;

    internal sealed class UnnamedOptionsManager<TOptions> :
       IOptions<TOptions>
       where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private volatile object _syncObj;
        private volatile TOptions _value;

        public UnnamedOptionsManager(IOptionsFactory<TOptions> factory) => _factory = factory;

        public TOptions Value
        {
            get
            {
                if (_value is TOptions value)
                {
                    return value;
                }

                lock (_syncObj ?? Interlocked.CompareExchange(ref _syncObj, new object(), null) ?? _syncObj)
                {
                    return _value ??= _factory.Create(Options<TOptions>.DefaultName);
                }
            }
        }
    }
}
