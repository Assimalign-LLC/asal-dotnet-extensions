using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Interface used to validate options.
/// </summary>
/// <typeparam name="TOptions">The options type to validate.</typeparam>
public interface IOptionsValidator<TOptions> where TOptions : class
{
    /// <summary>
    /// Validates a specific named options instance (or all when name is null).
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options instance.</param>
    /// <returns>The <see cref="OptionsValidationResult"/> result.</returns>
    OptionsValidationResult Validate(string name, TOptions options);
}
