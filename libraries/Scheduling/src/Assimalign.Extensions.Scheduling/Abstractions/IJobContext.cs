using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// 
/// </summary>
public interface IJobContext
{
    /// <summary>
    /// 
    /// </summary>
    DateTime SignalTime { get; }
}
