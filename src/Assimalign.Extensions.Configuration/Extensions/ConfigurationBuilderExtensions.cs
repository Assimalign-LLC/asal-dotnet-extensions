using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Configuration
{
    using Assimalign.Extensions.FileProviders.Physical;
    using Assimalign.Extensions.FileProviders.Abstractions;
    using Assimalign.Extensions.Configuration.Sources;
    using Assimalign.Extensions.Configuration.Providers;
    using Assimalign.Extensions.Configuration.Abstractions;

    public static class ConfigurationBuilderExtensions
    {
        #region Chaining Provider
        /// <summary>
        /// Adds an existing configuration to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="config">The <see cref="IConfiguration"/> to add.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config)
            => AddConfiguration(configurationBuilder, config, shouldDisposeConfiguration: false);

        /// <summary>
        /// Adds an existing configuration to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="config">The <see cref="IConfiguration"/> to add.</param>
        /// <param name="shouldDisposeConfiguration">Whether the configuration should get disposed when the configuration provider is disposed.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config, bool shouldDisposeConfiguration)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            configurationBuilder.Add(new ChainedConfigurationSource()
            {
                Configuration = config,
                ShouldDisposeConfiguration = shouldDisposeConfiguration,
            });
            return configurationBuilder;
        }
        #endregion

        #region Memory Provider
        /// <summary>
        /// Adds the memory configuration provider to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            configurationBuilder.Add(new MemoryConfigurationSource());
            return configurationBuilder;
        }

        /// <summary>
        /// Adds the memory configuration provider to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="initialData">The data to add to memory configuration provider.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryCollection(
            this IConfigurationBuilder configurationBuilder,
            IEnumerable<KeyValuePair<string, string>> initialData)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            configurationBuilder.Add(new MemoryConfigurationSource { InitialData = initialData });
            return configurationBuilder;
        }
        #endregion

        #region Command Line Provider

        /// <summary>
        ///   Adds a <see cref="CommandLineConfigurationProvider"/> <see cref="IConfigurationProvider"/>
        ///   that reads configuration values from the command line.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="args">The command line args.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        /// <remarks>
        ///   <para>
        ///     The values passed on the command line, in the <c>args</c> string array, should be a set
        ///     of keys prefixed with two dashes ("--") and then values, separate by either the
        ///     equals sign ("=") or a space (" ").
        ///   </para>
        ///   <para>
        ///     A forward slash ("/") can be used as an alternative prefix, with either equals or space, and when using
        ///     an equals sign the prefix can be left out altogether.
        ///   </para>
        ///   <para>
        ///     There are five basic alternative formats for arguments:
        ///     <c>key1=value1 --key2=value2 /key3=value3 --key4 value4 /key5 value5</c>.
        ///   </para>
        /// </remarks>
        /// <example>
        ///   A simple console application that has five values.
        ///   <code>
        ///     // dotnet run key1=value1 --key2=value2 /key3=value3 --key4 value4 /key5 value5
        ///
        ///     using Assimalign.Extensions.Configuration;
        ///     using System;
        ///
        ///     namespace CommandLineSample
        ///     {
        ///        public class Program
        ///        {
        ///            public static void Main(string[] args)
        ///            {
        ///                var builder = new ConfigurationBuilder();
        ///                builder.AddCommandLine(args);
        ///
        ///                var config = builder.Build();
        ///
        ///                Console.WriteLine($"Key1: '{config["Key1"]}'");
        ///                Console.WriteLine($"Key2: '{config["Key2"]}'");
        ///                Console.WriteLine($"Key3: '{config["Key3"]}'");
        ///                Console.WriteLine($"Key4: '{config["Key4"]}'");
        ///                Console.WriteLine($"Key5: '{config["Key5"]}'");
        ///            }
        ///        }
        ///     }
        ///   </code>
        /// </example>
        public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder configurationBuilder, string[] args)
        {
            return configurationBuilder.AddCommandLine(args, switchMappings: null);
        }

        /// <summary>
        ///   Adds a <see cref="CommandLineConfigurationProvider"/> <see cref="IConfigurationProvider"/> that reads
        ///   configuration values from the command line using the specified switch mappings.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="args">The command line args.</param>
        /// <param name="switchMappings">
        ///   The switch mappings. A dictionary of short (with prefix "-") and
        ///   alias keys (with prefix "--"), mapped to the configuration key (no prefix).
        /// </param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        /// <remarks>
        ///   <para>
        ///     The <c>switchMappings</c> allows additional formats for alternative short and alias keys
        ///     to be used from the command line. Also see the basic version of <c>AddCommandLine</c> for
        ///     the standard formats supported.
        ///   </para>
        ///   <para>
        ///     Short keys start with a single dash ("-") and are mapped to the main key name (without
        ///     prefix), and can be used with either equals or space. The single dash mappings are intended
        ///     to be used for shorter alternative switches.
        ///   </para>
        ///   <para>
        ///     Note that a single dash switch cannot be accessed directly, but must have a switch mapping
        ///     defined and accessed using the full key. Passing an undefined single dash argument will
        ///     cause as <c>FormatException</c>.
        ///   </para>
        ///   <para>
        ///     There are two formats for short arguments:
        ///     <c>-k1=value1 -k2 value2</c>.
        ///   </para>
        ///   <para>
        ///     Alias key definitions start with two dashes ("--") and are mapped to the main key name (without
        ///     prefix), and can be used in place of the normal key. They also work when a forward slash prefix
        ///     is used in the command line (but not with the no prefix equals format).
        ///   </para>
        ///   <para>
        ///     There are only four formats for aliased arguments:
        ///     <c>--alt3=value3 /alt4=value4 --alt5 value5 /alt6 value6</c>.
        ///   </para>
        /// </remarks>
        /// <example>
        ///   A simple console application that has two short and four alias switch mappings defined.
        ///   <code>
        ///     // dotnet run -k1=value1 -k2 value2 --alt3=value2 /alt4=value3 --alt5 value5 /alt6 value6
        ///
        ///     using Assimalign.Extensions.Configuration;
        ///     using System;
        ///     using System.Collections.Generic;
        ///
        ///     namespace CommandLineSample
        ///     {
        ///        public class Program
        ///        {
        ///            public static void Main(string[] args)
        ///            {
        ///                var switchMappings = new Dictionary&lt;string, string&gt;()
        ///                {
        ///                    { "-k1", "key1" },
        ///                    { "-k2", "key2" },
        ///                    { "--alt3", "key3" },
        ///                    { "--alt4", "key4" },
        ///                    { "--alt5", "key5" },
        ///                    { "--alt6", "key6" },
        ///                };
        ///                var builder = new ConfigurationBuilder();
        ///                builder.AddCommandLine(args, switchMappings);
        ///
        ///                var config = builder.Build();
        ///
        ///                Console.WriteLine($"Key1: '{config["Key1"]}'");
        ///                Console.WriteLine($"Key2: '{config["Key2"]}'");
        ///                Console.WriteLine($"Key3: '{config["Key3"]}'");
        ///                Console.WriteLine($"Key4: '{config["Key4"]}'");
        ///                Console.WriteLine($"Key5: '{config["Key5"]}'");
        ///                Console.WriteLine($"Key6: '{config["Key6"]}'");
        ///            }
        ///        }
        ///     }
        ///   </code>
        /// </example>
        public static IConfigurationBuilder AddCommandLine(
            this IConfigurationBuilder configurationBuilder,
            string[] args,
            IDictionary<string, string> switchMappings)
        {
            configurationBuilder.Add(new CommandLineConfigurationSource { Args = args, SwitchMappings = switchMappings });
            return configurationBuilder;
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the command line.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder builder, Action<CommandLineConfigurationSource> configureSource)
            => builder.Add(configureSource);
        #endregion

        #region Environment Variable Provider
        // <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new EnvironmentVariablesConfigurationSource());
            return configurationBuilder;
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables
        /// with a specified prefix.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="prefix">The prefix that environment variable names must start with. The prefix will be removed from the environment variable names.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvironmentVariables(
            this IConfigurationBuilder configurationBuilder,
            string prefix)
        {
            configurationBuilder.Add(new EnvironmentVariablesConfigurationSource { Prefix = prefix });
            return configurationBuilder;
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder, Action<EnvironmentVariablesConfigurationSource> configureSource)
            => builder.Add(configureSource);
        #endregion

        #region File Provider
        private static string FileProviderKey = "FileProvider";
        private static string FileLoadExceptionHandlerKey = "FileLoadExceptionHandler";

        /// <summary>
        /// Sets the default <see cref="IFileProvider"/> to be used for file-based providers.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="fileProvider">The default file provider instance.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder SetFileProvider(this IConfigurationBuilder builder, IFileProvider fileProvider)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Properties[FileProviderKey] = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            return builder;
        }

        /// <summary>
        /// Gets the default <see cref="IFileProvider"/> to be used for file-based providers.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>The default <see cref="IFileProvider"/>.</returns>
        public static IFileProvider GetFileProvider(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(FileProviderKey, out object provider))
            {
                return provider as IFileProvider;
            }

            return new PhysicalFileProvider(AppContext.BaseDirectory ?? string.Empty);
        }

        /// <summary>
        /// Sets the FileProvider for file-based providers to a PhysicalFileProvider with the base path.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="basePath">The absolute path of file-based providers.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder SetBasePath(this IConfigurationBuilder builder, string basePath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            return builder.SetFileProvider(new PhysicalFileProvider(basePath));
        }

        /// <summary>
        /// Sets a default action to be invoked for file-based providers when an error occurs.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="handler">The Action to be invoked on a file load exception.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder SetFileLoadExceptionHandler(this IConfigurationBuilder builder, Action<FileLoadExceptionContext> handler)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Properties[FileLoadExceptionHandlerKey] = handler;
            return builder;
        }

        /// <summary>
        /// Gets the default <see cref="IFileProvider"/> to be used for file-based providers.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static Action<FileLoadExceptionContext> GetFileLoadExceptionHandler(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(FileLoadExceptionHandlerKey, out object handler))
            {
                return handler as Action<FileLoadExceptionContext>;
            }
            return null;
        }
        #endregion

        #region Json Provider 
        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string path)
        {
            return AddJsonFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddJsonFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddJsonFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException();// SR.Error_InvalidFilePath, nameof(path));
            }

            return builder.AddJsonFile(s =>
            {
                s.FileProvider = provider;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, Action<JsonConfigurationSource> configureSource)
            => builder.Add(configureSource);

        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="stream">The <see cref="Stream"/> to read the json configuration data from.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonStream(this IConfigurationBuilder builder, Stream stream)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Add<JsonStreamConfigurationSource>(s => s.Stream = stream);
        }
        #endregion
    }
}
