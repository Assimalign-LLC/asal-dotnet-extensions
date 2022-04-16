using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace Assimalign.Extensions.Net.Http.Internal;

internal sealed class HttpMessageHandlerBuilderDefault : HttpMessageHandlerBuilder
{
    private string name;

    public HttpMessageHandlerBuilderDefault() { }


    public override string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            name = value;
        }
    }
    public override HttpMessageHandler PrimaryHandler { get; set; } = new HttpClientHandler();
    public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();
    public override HttpMessageHandler Build()
    {
        if (PrimaryHandler == null)
        {
            throw new InvalidOperationException();
        }
        return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
    }

    protected internal static HttpMessageHandler CreateHandlerPipeline(HttpMessageHandler primaryHandler, IEnumerable<DelegatingHandler> additionalHandlers)
    {
        if (primaryHandler == null)
        {
            throw new ArgumentNullException(nameof(primaryHandler));
        }

        if (additionalHandlers == null)
        {
            throw new ArgumentNullException(nameof(additionalHandlers));
        }

        IReadOnlyList<DelegatingHandler> additionalHandlersList = additionalHandlers as IReadOnlyList<DelegatingHandler> ?? additionalHandlers.ToArray();

        var next = primaryHandler;
        for (int i = additionalHandlersList.Count - 1; i >= 0; i--)
        {
            var handler = additionalHandlersList[i];
            if (handler == null)
            {
                throw new InvalidOperationException();
            }

            // Checking for this allows us to catch cases where someone has tried to re-use a handler. That really won't
            // work the way you want and it can be tricky for callers to figure out.
            if (handler.InnerHandler != null)
            {
                throw new InvalidOperationException();
            }

            handler.InnerHandler = next;
            next = handler;
        }

        return next;
    }
}

