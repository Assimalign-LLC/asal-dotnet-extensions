using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Net;

public interface ISocketTransportReceiver : IAsyncResult
{
    /// <summary>
    /// 
    /// </summary>
    void Initialize();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();
}
