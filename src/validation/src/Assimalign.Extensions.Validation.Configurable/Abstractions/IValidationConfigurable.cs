using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.Validation.Configurable;

/// <summary>
/// Represents a collection of configurable providers which build <see cref="IValidationProfile"/>.
/// </summary>
public interface IValidationConfigurable
{
    /// <summary>
    /// A collection of configurable provider which are responsible for 
    /// </summary>
    IEnumerable<IValidationConfigurableProvider> Providers { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<IValidationProfile> GetProfiles();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">The type the validation profile is registered to.</param>
    /// <returns></returns>
    IValidationProfile GetProfile(Type type);
}
