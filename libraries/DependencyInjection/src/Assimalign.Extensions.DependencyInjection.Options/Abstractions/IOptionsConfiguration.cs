namespace Assimalign.Extensions.DependencyInjection;

/// <summary>
/// Represents something that configures the <typeparamref name="TOptions"/> type.
/// Note: These are run before all <see cref="IOptionsPostConfiguration{TOptions}"/>.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public interface IOptionsConfiguration<in TOptions> where TOptions : class
{
    /// <summary>
    /// Invoked to configure a <typeparamref name="TOptions"/> instance.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    void Configure(TOptions options);

    /// <summary>
    /// Invoked to configure a <typeparamref name="TOptions"/> instance.
    /// </summary>
    /// <param name="name">The name of the options instance being configured.</param>
    /// <param name="options">The options instance to configure.</param>
    void Configure(string name, TOptions options);
}
