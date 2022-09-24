using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.Validation;

using Assimalign.Extensions.Validation.Internal;

/// <summary>
/// 
/// </summary>
public abstract class ValidationProfileBuilder : IValidationProfileBuilder
{
    private bool isBuilt;
    private IList<IValidationProfile> profiles;

    public ValidationProfileBuilder()
    {
        this.profiles = new List<IValidationProfile>();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void OnBuild(IValidationProfileBuilder builder);

    
    /// <inheritdoc />
    IValidationProfileBuilder IValidationProfileBuilder.CreateProfile<T>(Action<IValidationRuleDescriptor<T>> configure)
    {
        var profile = new ValidationProfileDefault<T>(configure);
        var descriptor = new ValidationRuleDescriptor<T>()
        {
            ValidationItems = profile.ValidationItems,
        };

        profile.Configure(descriptor);

        profiles.Add(profile);

        return this;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Once built 
    /// </remarks>
    IEnumerable<IValidationProfile> IValidationProfileBuilder.Build()
    {
        if (!isBuilt)
        {
            OnBuild(this);
            isBuilt = true;
        }
        return profiles;
    }
}
