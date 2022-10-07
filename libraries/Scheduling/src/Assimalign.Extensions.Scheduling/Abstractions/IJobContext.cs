using System;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// This interface represents data that is needed to be passed to the 
/// Job being executed
/// </summary>
public interface IJobContext
{
    /// <summary>
    /// The Job Execution TimeStamp
    /// </summary>
    DateTimeOffset TimeStamp { get; }
}
