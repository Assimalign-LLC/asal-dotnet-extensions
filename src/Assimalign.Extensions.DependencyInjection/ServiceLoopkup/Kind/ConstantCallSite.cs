using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup.Kind
{
    internal sealed class ConstantCallSite : CallSiteService
    {
        private readonly Type _serviceType;
        internal object DefaultValue => Value;

        public ConstantCallSite(Type serviceType, object defaultValue) : base(CallSiteResultCache.None)
        {
            _serviceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            if (defaultValue != null && !serviceType.IsInstanceOfType(defaultValue))
            {
                throw new ArgumentException(SR.Format(SR.ConstantCantBeConvertedToServiceType, defaultValue.GetType(), serviceType));
            }

            Value = defaultValue;
        }

        public override Type ServiceType => _serviceType;
        public override Type ImplementationType => DefaultValue?.GetType() ?? _serviceType;
        public override CallSiteKind Kind { get; } = CallSiteKind.Constant;
    }
}
