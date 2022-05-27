using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Abstractions
{
    /// <summary>
    /// Defines methods for objects that are managed by the host.
    /// </summary>
    public interface IHostBackgroundService : IHostService, IDisposable
    {
        /// <summary>
        /// This method is called when the <see cref="IHostService"/> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostService.StopAsync(CancellationToken)"/> is called.</param>
        /// <returns>A <see cref="Task"/> that represents the long running operations.</returns>
        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
