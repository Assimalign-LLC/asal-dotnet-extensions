using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// 
/// </summary>
public interface IJob
{
    /// <summary>
    /// 
    /// </summary>
    string JobId { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InvokeAsync(IJobContext context, CancellationToken cancellationToken);
}
