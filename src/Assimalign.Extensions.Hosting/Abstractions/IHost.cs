using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Abstractions
{
    /// <summary>
    /// A program abstraction.
    /// </summary>
    public interface IHost : IDisposable
    {
        /// <summary>
        /// The programs configured services.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Starts the program.
        /// </summary>
        /// <param name="cancellationToken">Used to abort program start.</param>
        /// <returns>A <see cref="Task"/> that will be completed when the <see cref="IHost"/> starts.</returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempts to gracefully stop the program.
        /// </summary>
        /// <param name="cancellationToken">Used to indicate when stop should no longer be graceful.</param>
        /// <returns>A <see cref="Task"/> that will be completed when the <see cref="IHost"/> stops.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
