using System;

namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// The <see cref="IDisposable.Dispose"/> method ends the scope lifetime. Once Dispose
/// is called, any scoped services that have been resolved from
/// <see cref="ServiceProvider"/> will be disposed.
/// </summary>
public interface IServiceScope : IDisposable
{
    /// <summary>
    /// The <see cref="IServiceProvider"/> used to resolve dependencies from the scope.
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}
