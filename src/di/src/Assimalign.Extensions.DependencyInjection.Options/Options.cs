namespace Assimalign.Extensions.DependencyInjection;


public class Options<TOptions> : IOptions<TOptions>
    where TOptions : class
{ 
    /// <summary>
    /// Initializes the wrapper with the options instance to return.
    /// </summary>
    /// <param name="options">The options instance to return.</param>
    public Options(TOptions value) 
        : this(typeof(TOptions).Name, value) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public Options(string name, TOptions value)
    {
        this.Name = name;
        this.Value = value;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// The options instance.
    /// </summary>
    public TOptions Value { get; }

    public static readonly string DefaultName = typeof(TOptions).Name;

    /// <summary>
    /// Creates a wrapper around an instance of <typeparamref name="TOptions"/> to return itself as an <see cref="IOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type.</typeparam>
    /// <param name="options">Options object.</param>
    /// <returns>Wrapped options object.</returns>
    public static IOptions<TOptions> Create(TOptions options) => 
        new Options<TOptions>(options);
}
