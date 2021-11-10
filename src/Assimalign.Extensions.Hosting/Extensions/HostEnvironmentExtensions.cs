using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Hosting.Environments;
    using Assimalign.Extensions.Hosting.Abstractions;

    /// <summary>
    /// Extension methods for <see cref="IHostingEnvironment"/>.
    /// </summary>
    public static class HostEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current host environment name is <see cref="EnvironmentName.Development"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentName.Development"/>, otherwise false.</returns>
        public static bool IsDevelopment(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            return hostEnvironment.IsEnvironment(HostEnvironments.Development);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostEnvironment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsQualityAsurance(this IHostEnvironment hostEnvironment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostEnvironment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsUserAcceptanceTesting(this IHostEnvironment hostEnvironment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="EnvironmentName.Staging"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentName.Staging"/>, otherwise false.</returns>
        public static bool IsStaging(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            return hostEnvironment.IsEnvironment(HostEnvironments.Staging);
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="EnvironmentName.Production"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentName.Production"/>, otherwise false.</returns>
        public static bool IsProduction(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            return hostEnvironment.IsEnvironment(HostEnvironments.Production);
        }

        /// <summary>
        /// Compares the current host environment name against the specified value.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <param name="environmentName">Environment name to validate against.</param>
        /// <returns>True if the specified name is the same as the current environment, otherwise false.</returns>
        public static bool IsEnvironment(
            this IHostEnvironment hostEnvironment,
            string environmentName)
        {
            if (hostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            return string.Equals(
                hostEnvironment.EnvironmentName,
                environmentName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
