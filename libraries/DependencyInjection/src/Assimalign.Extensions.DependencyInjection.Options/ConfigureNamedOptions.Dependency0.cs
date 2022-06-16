using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Implementation of <see cref="IConfigureNamedOptions{TOptions}"/>.
/// </summary>
/// <typeparam name="TOptions">Options type being configured.</typeparam>
public class ConfigureNamedOptions<TOptions> : IOptionsConfiguration<TOptions> 
    where TOptions : class
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">The name of the options.</param>
    /// <param name="action">The action to register.</param>
    public ConfigureNamedOptions(string name, Action<TOptions> action)
    {
        Name = name;
        Action = action;
    }

    /// <summary>
    /// The options name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The configuration action.
    /// </summary>
    public Action<TOptions> Action { get; }

    /// <summary>
    /// Invokes the registered configure <see cref="Action"/> if the <paramref name="name"/> matches.
    /// </summary>
    /// <param name="name">The name of the options instance being configured.</param>
    /// <param name="options">The options instance to configure.</param>
    public virtual void Configure(string name, TOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        // Null name is used to configure all named options.
        if (Name == null || name == Name)
        {
            Action?.Invoke(options);
        }
    }

    /// <summary>
    /// Invoked to configure a <typeparamref name="TOptions"/> instance with the <see cref="Options.DefaultName"/>.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    public void Configure(TOptions options) => Configure(Options<TOptions>.DefaultName, options);
}
