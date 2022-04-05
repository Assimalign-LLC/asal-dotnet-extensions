

namespace Assimalign.Extensions.Options.Abstractions
{
    /// <summary>
    /// Used to create <typeparamref name="TOptions"/> instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptionsFactory<TOptions>
        where TOptions : class
    {
        /// <summary>
        /// Returns a configured <typeparamref name="TOptions"/> instance with the given name.
        /// </summary>
        TOptions Create(string name);
    }
}
