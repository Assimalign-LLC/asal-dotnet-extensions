namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Used to retrieve configured <typeparamref name="TOptions"/> instances.
/// </summary>
/// <typeparam name="TOptions">The type of options being requested.</typeparam>
public interface IOptions<out TOptions>
    where TOptions : class
{
    /// <summary>
    /// 
    /// </summary>
    string Name { get; }
    /// <summary>
    /// The default configured <typeparamref name="TOptions"/> instance
    /// </summary>
    TOptions Value { get; }
}