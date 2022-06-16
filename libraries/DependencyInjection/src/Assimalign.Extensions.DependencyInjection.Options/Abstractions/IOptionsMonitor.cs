using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Used for notifications when <typeparamref name="TOptions"/> instances change.
/// </summary>
/// <typeparam name="TOptions">The options type.</typeparam>
public interface IOptionsMonitor<out TOptions>
{
    /// <summary>
    /// Returns the current <typeparamref name="TOptions"/> instance with the <see cref="Options.Options.DefaultName"/>.
    /// </summary>
    TOptions CurrentValue { get; }

    /// <summary>
    /// Returns a configured <typeparamref name="TOptions"/> instance with the given name.
    /// </summary>
    TOptions Get(string name);

    /// <summary>
    /// Registers a listener to be called whenever a named <typeparamref name="TOptions"/> changes.
    /// </summary>
    /// <param name="listener">The action to be invoked when <typeparamref name="TOptions"/> has changed.</param>
    /// <returns>An <see cref="IDisposable"/> which should be disposed to stop listening for changes.</returns>
    IDisposable OnChange(Action<TOptions, string> listener);
}
