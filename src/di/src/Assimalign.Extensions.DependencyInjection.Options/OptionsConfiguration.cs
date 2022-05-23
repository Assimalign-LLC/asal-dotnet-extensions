using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;


/// <summary>
/// Implementation of <see cref="IOptionsConfiguration{TOptions}"/>.
/// </summary>
/// <typeparam name="TOptions">Options type being configured.</typeparam>
public class OptionsConfiguration<TOptions> : IOptionsConfiguration<TOptions> 
    where TOptions : class
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="action">The action to register.</param>
    public OptionsConfiguration(Action<TOptions> action) 
        : this(typeof(TOptions).Name, action) { }

    public OptionsConfiguration(string name, Action<TOptions> action)
    {
        this.Name = name;
        this.Action = action;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The configuration action.
    /// </summary>
    public Action<TOptions> Action { get; }

    /// <summary>
    /// Invokes the registered configure <see cref="Action"/>.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    public virtual void Configure(TOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        Action?.Invoke(options);
    }
    /// <summary>
    /// Invoked to configure a <typeparamref name="TOptions"/> instance.
    /// </summary>
    /// <param name="name">The name of the options instance being configured.</param>
    /// <param name="options">The options instance to configure.</param>
    public virtual void Configure(string name, TOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        if (name == Name)
        {
            Action?.Invoke(options);
        }
    }
}
