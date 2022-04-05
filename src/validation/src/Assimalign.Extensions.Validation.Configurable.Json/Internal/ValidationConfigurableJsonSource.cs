using System;

namespace Assimalign.Extensions.Validation.Configurable;

internal sealed class ValidationConfigurableJsonSource<T> : IValidationConfigurableSource
    where T : class
{
    private readonly Func<ValidationConfigurableJsonProfile<T>> configure;

    public ValidationConfigurableJsonSource(Func<ValidationConfigurableJsonProfile<T>> configure)
    {
        this.configure = configure;
    }

    public IValidationConfigurableProvider Build() =>
        new ValidationConfigurableJsonProvider<T>(configure.Invoke());
}