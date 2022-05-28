using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    void Invoke(IJobContext context);
}
