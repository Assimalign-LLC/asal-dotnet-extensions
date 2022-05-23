using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Assimalign.Extensions.Validation;


/// <summary>
/// A fluent builder for creating a 
/// </summary>
public sealed class ValidatorFactoryBuilder
{
    internal readonly ConcurrentDictionary<string, IValidator> validators;

    internal ValidatorFactoryBuilder()
    {
        this.validators = new ConcurrentDictionary<string, IValidator>();
    }

    internal IDictionary<string, IValidator> Validators => this.validators;

    /// <summary>
    /// Builds a Validator scoped to the <paramref name="validatorName"/>.
    /// </summary>
    /// <param name="validatorName"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public ValidatorFactoryBuilder AddValidator(string validatorName, Action<ValidatorBuilder> configure)
    {
        if (string.IsNullOrEmpty(validatorName))
        {
            throw new ArgumentNullException(nameof(validatorName), $"The parameter 'validatorName' cannot be null or empty.");
        }

        this.validators.GetOrAdd(validatorName, validatorName => Validator.Create(configure));

        return this;
    }
}