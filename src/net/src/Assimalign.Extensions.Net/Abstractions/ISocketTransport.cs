using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Buffers;

namespace Assimalign.Extensions.Net;

using Assimalign.Extensions.Primitives;

public interface ISocketTransport :
    IThreadPoolWorkItem,
    IDisposable,
    IAsyncDisposable
{
    /// <summary>
    /// 
    /// </summary>
    CorrelationId ConnectionId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    EndPoint? RemoteEndpoint { get; }
    /// <summary>
    /// 
    /// </summary>
    EndPoint? LocalEndpoint { get; }
    /// <summary>
    /// Triggered when the client connection is closed.
    /// </summary>
    CancellationToken ConnectionClosed { get; }

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <remarks>
    /// For reference the Server Pipeline should contain the 'Client PipeReader' for processing
    /// while the Client Pipeline should contain the 'Server PipeReader' for sending
    ///
    ///   ----------------------------------------------
    ///                   Server Pipeline
    ///         Client PipeReader | Server PipeWriter      
    ///   ----------------------------------------------
    ///    ↑                                  ↓
    ///    ↑ Pipe Writer Copies               ↓ Pipe
    ///    ↑ Request                          ↓ Response
    ///   ----------------------------------------------
    ///                   Client Pipeline
    ///       Server PipeReader | Client PipeWriter
    ///   ----------------------------------------------
    /// </remarks>
    ISocketTransportPipeline Transport { get; }
    /// <summary>
    /// 
    /// </summary>
    ISocketTransportPipeline Application { get; }

    /// <summary>
    /// 
    /// </summary>
    AsyncCallback Sender { get; set; }
    /// <summary>
    /// 
    /// </summary>
    AsyncCallback Receiver { get; set; }
    /// <summary>
    /// 
    /// </summary>
    MemoryPool<byte> MemoryPool { get; }
    /// <summary>
    /// Should be responsible for initializing the 
    /// the sender and receiver.
    /// </summary>
    void Start();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task StartAsync();
    /// <summary>
    /// 
    /// </summary>
    void Abort();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task AbortAsync();
}
