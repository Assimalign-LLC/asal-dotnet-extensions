using System;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace Assimalign.Extensions.Validation;


/// <summary>
/// 
/// </summary>
public sealed class ValidatorFactoryBuilder
{
    internal readonly ConcurrentDictionary<string, IValidator> validators;

    internal ValidatorFactoryBuilder()
    {
        this.validators = new ConcurrentDictionary<string, IValidator>();
    }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<IValidator> Validators => this.validators.Values;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validatorName"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public ValidatorFactoryBuilder AddValidator(string validatorName, Action<ValidationOptions> configure)
    {
        if (string.IsNullOrEmpty(validatorName))
        {
            throw new ArgumentNullException(nameof(validatorName));
        }

        var validator = Validator.Create(configure);

        this.validators.GetOrAdd(validatorName, validator);

        return this;
    }
}