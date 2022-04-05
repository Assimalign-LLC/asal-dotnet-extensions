using System;


namespace Assimalign.Extensions.Options
{

    using Assimalign.Extensions.Configuration;
    using Assimalign.Extensions.Configuration.Abstractions;


    /// <summary>
    /// Configures an option instance by using <see cref="ConfigurationBinder.Bind(IConfiguration, object)"/> against an <see cref="IConfiguration"/>.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to bind.</typeparam>
    public class NamedConfigureFromConfigurationOptions<TOptions> : ConfigureNamedOptions<TOptions>
        where TOptions : class
    {
        /// <summary>
        /// Constructor that takes the <see cref="IConfiguration"/> instance to bind against.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
        public NamedConfigureFromConfigurationOptions(string name, IConfiguration config)
            : this(name, config, _ => { })
        { }

        /// <summary>
        /// Constructor that takes the <see cref="IConfiguration"/> instance to bind against.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
        /// <param name="configureBinder">Used to configure the <see cref="ConfigurationBinderOptions"/>.</param>
        public NamedConfigureFromConfigurationOptions(string name, IConfiguration config, Action<ConfigurationBinderOptions> configureBinder)
            : base(name, options => BindFromOptions(options, config, configureBinder))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
        }

        //[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode",
        //    Justification = "The only call to this method is the constructor which is already annotated as RequiresUnreferencedCode.")]
        private static void BindFromOptions(TOptions options, IConfiguration config, Action<ConfigurationBinderOptions> configureBinder) => config.Bind(options, configureBinder);
    }
}
