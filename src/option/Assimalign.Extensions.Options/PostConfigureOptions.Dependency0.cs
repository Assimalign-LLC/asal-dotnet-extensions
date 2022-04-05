using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Options.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IPostConfigureOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    public class PostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// Creates a new instance of <see cref="PostConfigureOptions{TOptions}"/>.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="action">The action to register.</param>
        public PostConfigureOptions(string name, Action<TOptions> action)
        {
            Name = name;
            Action = action;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The initialization action.
        /// </summary>
        public Action<TOptions> Action { get; }

        /// <summary>
        /// Invokes the registered initialization <see cref="Action"/> if the <paramref name="name"/> matches.
        /// </summary>
        /// <param name="name">The name of the action to invoke.</param>
        /// <param name="options">The options to use in initialization.</param>
        public virtual void PostConfigure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to initialize all named options.
            if (Name == null || name == Name)
            {
                Action?.Invoke(options);
            }
        }
    }
}
