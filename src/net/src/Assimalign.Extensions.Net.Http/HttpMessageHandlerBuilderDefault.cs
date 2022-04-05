using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Assimalign.Extensions.Http
{
    internal sealed class HttpMessageHandlerBuilderDefault : HttpMessageHandlerBuilder
    {
        public HttpMessageHandlerBuilderDefault(IServiceProvider services)
        {
            Services = services;
        }

        private string _name;

        public override string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _name = value;
            }
        }

        public override HttpMessageHandler PrimaryHandler { get; set; } = new HttpClientHandler();

        public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();

        public override IServiceProvider Services { get; }

        public override HttpMessageHandler Build()
        {
            if (PrimaryHandler == null)
            {
                string message = "Something Happened";// SR.Format(SR.HttpMessageHandlerBuilder_PrimaryHandlerIsNull, nameof(PrimaryHandler));
                throw new InvalidOperationException(message);
            }

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}

