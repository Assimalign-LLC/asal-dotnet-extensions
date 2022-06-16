using System;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Hosting.Environments;
    using Assimalign.Extensions.Hosting;

    /// <summary>
    /// Extension methods for <see cref="IHostEnvironment"/>.
    /// </summary>
    public static class HostEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current host environment name is <see cref="HostEnvironments.Development"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="HostEnvironments.Development"/>, otherwise false.</returns>
        public static bool IsDevelopment(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment(HostEnvironments.Development);
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="HostEnvironments.QualityAssurance"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="HostEnvironments.QualityAssurance"/>, otherwise false.</returns>
        public static bool IsQualityAsurance(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment(HostEnvironments.QualityAssurance);
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="HostEnvironments.UserAcceptance"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="HostEnvironments.UserAcceptance"/>, otherwise false.</returns>
        public static bool IsUserAcceptanceTesting(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment(HostEnvironments.UserAcceptance);
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="HostEnvironments.Staging"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="HostEnvironments.Staging"/>, otherwise false.</returns>
        public static bool IsStaging(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment(HostEnvironments.Staging);
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="HostEnvironments.Production"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="HostEnvironments.Production"/>, otherwise false.</returns>
        public static bool IsProduction(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment(HostEnvironments.Production);
        }

        /// <summary>
        /// Compares the current host environment name against the specified value.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <param name="environmentName">Environment name to validate against.</param>
        /// <returns>True if the specified name is the same as the current environment, otherwise false.</returns>
        public static bool IsEnvironment(this IHostEnvironment hostEnvironment, string environmentName)
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
