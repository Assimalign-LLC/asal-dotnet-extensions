﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Options;

    /// <summary>
    /// Implementation of <see cref="IConfigureNamedOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    /// <typeparam name="TDep">Dependency type.</typeparam>
    public class ConfigureNamedOptions<TOptions, TDep> : IConfigureNamedOptions<TOptions>
        where TOptions : class
        where TDep : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep dependency, Action<TOptions, TDep> action)
        {
            Name = name;
            Action = action;
            Dependency = dependency;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep> Action { get; }

        /// <summary>
        /// The dependency.
        /// </summary>
        public TDep Dependency { get; }

        /// <summary>
        /// Invokes the registered configure <see cref="Action"/> if the <paramref name="name"/> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions"/> instance with the <see cref="Options.DefaultName"/>.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options) => Configure(Options<TOptions>.DefaultName, options);
    }

}
