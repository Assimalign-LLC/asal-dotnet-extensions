

namespace Assimalign.Extensions.DependencyInjection;

using Assimalign.Extensions.Primitives;

/// <summary>
/// Used to fetch <see cref="IStateToken"/> used for tracking options changes.
/// </summary>
/// <typeparam name="TOptions">Options type.</typeparam>
public interface IOptionsChangeTokenSource<out TOptions>
{
    /// <summary>
    /// Returns a <see cref="IStateToken"/> which can be used to register a change notification callback.
    /// </summary>
    /// <returns>Change token.</returns>
    IStateToken GetChangeToken();

    /// <summary>
    /// The name of the option instance being changed.
    /// </summary>
    string Name { get; }
}
