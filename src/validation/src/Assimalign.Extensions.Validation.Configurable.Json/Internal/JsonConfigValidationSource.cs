using System;

namespace Assimalign.Extensions.Validation.Configurable;

internal sealed class JsonConfigValidationSource<T> : IValidationConfigurableSource
    where T : class
{
    private readonly Func<JsonConfigValidationProfile<T>> configure;

    public JsonConfigValidationSource(Func<JsonConfigValidationProfile<T>> configure)
    {
        this.configure = configure;
    }

    public IValidationConfigurableProvider Build() =>
        new JsonConfigValidationProvider<T>(configure.Invoke());
}