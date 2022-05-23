using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Validation.Internal;

internal class ValidationProfileDefault<TValue> : ValidationProfile<TValue>
{
    private readonly Action<IValidationRuleDescriptor<TValue>> configure;

    public ValidationProfileDefault(Action<IValidationRuleDescriptor<TValue>> configure)
    {
        this.configure = configure;
    }
    public override void Configure(IValidationRuleDescriptor<TValue> descriptor)
    {
        configure.Invoke(descriptor);
    }
}
