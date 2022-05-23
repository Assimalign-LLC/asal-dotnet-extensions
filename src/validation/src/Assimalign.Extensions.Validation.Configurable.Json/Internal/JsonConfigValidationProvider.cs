using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Validation.Configurable;

internal sealed class JsonConfigValidationProvider<T> : IValidationConfigurableProvider
    where T : class
{
    private readonly JsonConfigValidationProfile<T> profile;

    private JsonConfigValidationProvider() { }

    internal JsonConfigValidationProvider(JsonConfigValidationProfile<T> profile)
    {
        this.profile = profile;
    }


    public IValidationProfile GetProfile() => this.profile;


    public bool TryGetProfile(Type type, out IValidationProfile profile)
    {
        if (type == typeof(T))
        {
            profile = this.profile;
            return true;
        }
        else
        {
            profile = null;
            return false;
        }
    }
}