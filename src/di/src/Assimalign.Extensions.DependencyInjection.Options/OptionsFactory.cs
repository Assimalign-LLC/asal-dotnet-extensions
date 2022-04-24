using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

using Assimalign.Extensions.DependencyInjection.Exceptions;

/// <summary>
/// Implementation of <see cref="IOptionsFactory{TOptions}"/>.
/// </summary>
/// <typeparam name="TOptions">The type of options being requested.</typeparam>
public class OptionsFactory<TOptions> :
    IOptionsFactory<TOptions>
    where TOptions : class
{
    private readonly IOptionsValidator<TOptions>[] optionsValidators;
    private readonly IOptionsConfiguration<TOptions>[] optionsSetupConfigurations;
    private readonly IOptionsPostConfiguration<TOptions>[] optionsPostConfigurations;

    /// <summary>
    /// Initializes a new instance with the specified options configurations.
    /// </summary>
    /// <param name="setups">The configuration actions to run.</param>
    /// <param name="postConfigures">The initialization actions to run.</param>
    public OptionsFactory(
        IEnumerable<IOptionsConfiguration<TOptions>> setups,
        IEnumerable<IOptionsPostConfiguration<TOptions>> postConfigures)
        : this(setups, postConfigures, validations: Array.Empty<IOptionsValidator<TOptions>>())
    { }

    /// <summary>
    /// Initializes a new instance with the specified options configurations.
    /// </summary>
    /// <param name="setups">The configuration actions to run.</param>
    /// <param name="postConfigures">The initialization actions to run.</param>
    /// <param name="validations">The validations to run.</param>
    public OptionsFactory(
        IEnumerable<IOptionsConfiguration<TOptions>> setups,
        IEnumerable<IOptionsPostConfiguration<TOptions>> postConfigures,
        IEnumerable<IOptionsValidator<TOptions>> validations)
    {
        // The default DI container uses arrays under the covers. Take advantage of this knowledge
        // by checking for an array and enumerate over that, so we don't need to allocate an enumerator.
        // When it isn't already an array, convert it to one, but don't use System.Linq to avoid pulling Linq in to
        // small trimmed applications.

        optionsSetupConfigurations = setups as IOptionsConfiguration<TOptions>[] ?? new List<IOptionsConfiguration<TOptions>>(setups).ToArray();
        optionsPostConfigurations = postConfigures as IOptionsPostConfiguration<TOptions>[] ?? new List<IOptionsPostConfiguration<TOptions>>(postConfigures).ToArray();
        optionsValidators = validations as IOptionsValidator<TOptions>[] ?? new List<IOptionsValidator<TOptions>>(validations).ToArray();
    }

    /// <summary>
    /// Returns a configured <typeparamref name="TOptions"/> instance with the given <paramref name="name"/>.
    /// </summary>
    public TOptions Create(string name)
    {
        var options = CreateInstance(name);

        foreach (var initialSetup in optionsSetupConfigurations)
        {
            initialSetup.Configure(name, options);
        }
        foreach (var postSetup in optionsPostConfigurations)
        {
            postSetup.PostConfigure(name, options);
        }

        if (optionsValidators != null)
        {
            var failures = new List<string>();
            foreach (IOptionsValidator<TOptions> validate in optionsValidators)
            {
                OptionsValidationResult result = validate.Validate(name, options);
                if (result is not null && result.Failed)
                {
                    failures.AddRange(result.Failures);
                }
            }
            if (failures.Count > 0)
            {
                throw new OptionsValidationException(name, typeof(TOptions), failures);
            }
        }

        return options;
    }

    /// <summary>
    /// Creates a new instance of options type
    /// </summary>
    protected virtual TOptions CreateInstance(string name) => Activator.CreateInstance<TOptions>();
}
