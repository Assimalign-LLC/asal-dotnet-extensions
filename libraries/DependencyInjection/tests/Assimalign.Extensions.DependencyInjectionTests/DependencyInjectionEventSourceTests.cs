


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using Assimalign.Extensions.DependencyInjection.Specification.Fakes;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    [CollectionDefinition(nameof(EventSourceTests), DisableParallelization = true)]
    public class EventSourceTests : ICollectionFixture<EventSourceTests>
    {
    }

    [Collection(nameof(EventSourceTests))]
    public class DependencyInjectionEventSourceTests : IDisposable
    {
        private readonly TestEventListener _listener = new TestEventListener();

        public DependencyInjectionEventSourceTests()
        {
            // clear the provider list in between tests
            typeof(ServiceEventSource).GetField("_providers", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(ServiceEventSource.Log, new List<WeakReference<ServiceProvider>>());

            _listener.EnableEvents(DependencyInjectionEventSource.Log, EventLevel.Verbose);
        }

        [Fact]
        public void ExistsWithCorrectId()
        {
            var esType = typeof(DependencyInjectionEventSource);

            Assert.NotNull(esType);

            Assert.Equal("Microsoft-Extensions-DependencyInjection", EventSource.GetName(esType));
            Assert.Equal(Guid.Parse("27837f46-1a43-573d-d30c-276de7d02192"), EventSource.GetGuid(esType));
            Assert.NotEmpty(EventSource.GenerateManifest(esType, "assemblyPathToIncludeInManifest"));
        }

        [Fact]
        public void EmitsCallSiteBuiltEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var fakeDisposeCallback = new FakeDisposeCallback();
            serviceCollection.AddSingleton(fakeDisposeCallback);
            serviceCollection.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
            serviceCollection.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeMultipleService>(provider => new FakeDisposableCallbackInnerService(fakeDisposeCallback));
            serviceCollection.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();

            serviceCollection.BuildServiceProvider().GetService<IEnumerable<IFakeOuterService>>();

            var callsiteBuiltEvent = _listener.EventData.Single(e => e.EventName == "CallSiteBuilt");


            Assert.Equal(
                string.Join(Environment.NewLine,
                "{",
                "  \"serviceType\": \"System.Collections.Generic.IEnumerable`1[Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService]\",",
                "  \"kind\": \"IEnumerable\",",
                "  \"cache\": \"None\",",
                "  \"itemType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService\",",
                "  \"size\": \"1\",",
                "  \"items\": [",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService\",",
                "      \"kind\": \"Constructor\",",
                "      \"cache\": \"Dispose\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackOuterService\",",
                "      \"arguments\": [",
                "        {",
                "          \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeService\",",
                "          \"kind\": \"Constructor\",",
                "          \"cache\": \"Root\",",
                "          \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "          \"arguments\": [",
                "            {",
                "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\",",
                "              \"kind\": \"Constant\",",
                "              \"cache\": \"None\",",
                "              \"value\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "            }",
                "          ]",
                "        },",
                "        {",
                "          \"serviceType\": \"System.Collections.Generic.IEnumerable`1[Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService]\",",
                "          \"kind\": \"IEnumerable\",",
                "          \"cache\": \"None\",",
                "          \"itemType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "          \"size\": \"4\",",
                "          \"items\": [",
                "            {",
                "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Constructor\",",
                "              \"cache\": \"Root\",",
                "              \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "              \"arguments\": [",
                "                {",
                "                  \"ref\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "                }",
                "              ]",
                "            },",
                "            {",
                "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Factory\",",
                "              \"cache\": \"Root\",",
                "              \"method\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService <EmitsCallSiteBuiltEvent>b__0(System.IServiceProvider)\"",
                "            },",
                "            {",
                "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Constructor\",",
                "              \"cache\": \"Scope\",",
                "              \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "              \"arguments\": [",
                "                {",
                "                  \"ref\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "                }",
                "              ]",
                "            },",
                "            {",
                "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Constructor\",",
                "              \"cache\": \"Dispose\",",
                "              \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "              \"arguments\": [",
                "                {",
                "                  \"ref\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "                }",
                "              ]",
                "            }",
                "          ]",
                "        },",
                "        {",
                "          \"ref\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "        }",
                "      ]",
                "    }",
                "  ]",
                "}"),
                JObject.Parse(GetProperty<string>(callsiteBuiltEvent, "callSite")).ToString());

            Assert.Equal("System.Collections.Generic.IEnumerable`1[Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService]", GetProperty<string>(callsiteBuiltEvent, "serviceType"));
            Assert.Equal(0, GetProperty<int>(callsiteBuiltEvent, "chunkIndex"));
            Assert.Equal(1, GetProperty<int>(callsiteBuiltEvent, "chunkCount"));
            Assert.Equal(1, callsiteBuiltEvent.EventId);
        }

        [Fact]
        public void EmitsServiceResolvedEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<IFakeService>();
            serviceProvider.GetService<IFakeService>();
            serviceProvider.GetService<IFakeService>();

            var serviceResolvedEvents = _listener.EventData.Where(e => e.EventName == "ServiceResolved").ToArray();

            Assert.Equal(3, serviceResolvedEvents.Length);
            foreach (var serviceResolvedEvent in serviceResolvedEvents)
            {
                Assert.Equal("Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeService", GetProperty<string>(serviceResolvedEvent, "serviceType"));
                Assert.Equal(2, serviceResolvedEvent.EventId);
            }
        }

        [Fact]
        public void EmitsExpressionTreeBuiltEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider(ServiceProviderMode.Expressions);

            serviceProvider.GetService<IFakeService>();

            var expressionTreeGeneratedEvent = _listener.EventData.Single(e => e.EventName == "ExpressionTreeGenerated");

            Assert.Equal("Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeService", GetProperty<string>(expressionTreeGeneratedEvent, "serviceType"));
            Assert.Equal(9, GetProperty<int>(expressionTreeGeneratedEvent, "nodeCount"));
            Assert.Equal(3, expressionTreeGeneratedEvent.EventId);
        }

        [Fact]
        [ActiveIssue("https://github.com/dotnet/runtime/issues/35753", TestPlatforms.Windows)]
        public void EmitsDynamicMethodBuiltEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider(ServiceProviderMode.ILEmit);

            serviceProvider.GetService<IFakeService>();

            var expressionTreeGeneratedEvent = _listener.EventData.Single(e => e.EventName == "DynamicMethodBuilt");

            Assert.Equal("Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeService", GetProperty<string>(expressionTreeGeneratedEvent, "serviceType"));
            Assert.Equal(12, GetProperty<int>(expressionTreeGeneratedEvent, "methodSize"));
            Assert.Equal(4, expressionTreeGeneratedEvent.EventId);
        }

        [Fact]
        public void EmitsScopeDisposedEvent()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetService<IFakeService>();
            }

            var scopeDisposedEvent = _listener.EventData.Single(e => e.EventName == "ScopeDisposed");

            Assert.Equal(1, GetProperty<int>(scopeDisposedEvent, "scopedServicesResolved"));
            Assert.Equal(1, GetProperty<int>(scopeDisposedEvent, "disposableServices"));
            Assert.Equal(5, scopeDisposedEvent.EventId);
        }

        [Fact]
        public void EmitsServiceRealizationFailedEvent()
        {
            var exception = new Exception("Test error.");
            DependencyInjectionEventSource.Log.ServiceRealizationFailed(exception, 1234);

            var eventName = nameof(DependencyInjectionEventSource.Log.ServiceRealizationFailed);
            var serviceRealizationFailedEvent = _listener.EventData.Single(e => e.EventName == eventName);

            Assert.Equal("System.Exception: Test error.", GetProperty<string>(serviceRealizationFailedEvent, "exceptionMessage"));
            Assert.Equal(1234, GetProperty<int>(serviceRealizationFailedEvent, "serviceProviderHashCode"));
            Assert.Equal(6, serviceRealizationFailedEvent.EventId);
        }

        [Fact]
        public void EmitsServiceProviderBuilt()
        {
            ServiceCollection serviceCollection = new();
            FakeDisposeCallback fakeDisposeCallback = new();
            serviceCollection.AddSingleton(fakeDisposeCallback);
            serviceCollection.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
            serviceCollection.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeMultipleService>(provider => new FakeDisposableCallbackInnerService(fakeDisposeCallback));
            serviceCollection.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddScoped(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            serviceCollection.AddTransient<IFakeOpenGenericService<PocoClass>, FakeOpenGenericService<PocoClass>>();

            using ServiceProvider provider = serviceCollection.BuildServiceProvider();

            EventWrittenEventArgs serviceProviderBuiltEvent = _listener.EventData.Single(e => e.EventName == "ServiceProviderBuilt");
            GetProperty<int>(serviceProviderBuiltEvent, "serviceProviderHashCode"); // assert hashcode exists as an int
            Assert.Equal(4, GetProperty<int>(serviceProviderBuiltEvent, "singletonServices"));
            Assert.Equal(2, GetProperty<int>(serviceProviderBuiltEvent, "scopedServices"));
            Assert.Equal(3, GetProperty<int>(serviceProviderBuiltEvent, "transientServices"));
            Assert.Equal(1, GetProperty<int>(serviceProviderBuiltEvent, "closedGenericsServices"));
            Assert.Equal(1, GetProperty<int>(serviceProviderBuiltEvent, "openGenericsServices"));
            Assert.Equal(7, serviceProviderBuiltEvent.EventId);

            EventWrittenEventArgs serviceProviderDescriptorsEvent = _listener.EventData.Single(e => e.EventName == "ServiceProviderDescriptors");
            Assert.Equal(
                string.Join(Environment.NewLine,
                "{",
                "  \"descriptors\": [",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationInstance\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback (instance)\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService\",",
                "      \"lifetime\": \"Transient\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackOuterService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationFactory\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService <EmitsServiceProviderBuilt>b__0(System.IServiceProvider)\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Scoped\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Transient\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeService\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOpenGenericService`1[TValue]\",",
                "      \"lifetime\": \"Scoped\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeOpenGenericService`1[TVal]\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.IFakeOpenGenericService`1[Assimalign.Extensions.DependencyInjection.Specification.Fakes.PocoClass]\",",
                "      \"lifetime\": \"Transient\",",
                "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.Specification.Fakes.FakeOpenGenericService`1[Assimalign.Extensions.DependencyInjection.Specification.Fakes.PocoClass]\"",
                "    }",
                "  ]",
                "}"),
                JObject.Parse(GetProperty<string>(serviceProviderDescriptorsEvent, "descriptors")).ToString());

            GetProperty<int>(serviceProviderDescriptorsEvent, "serviceProviderHashCode"); // assert hashcode exists as an int
            Assert.Equal(0, GetProperty<int>(serviceProviderDescriptorsEvent, "chunkIndex"));
            Assert.Equal(1, GetProperty<int>(serviceProviderDescriptorsEvent, "chunkCount"));
            Assert.Equal(8, serviceProviderDescriptorsEvent.EventId);
        }

        /// <summary>
        /// Verifies that when an EventListener is enabled after the ServiceProvider has been built,
        /// the ServiceProviderBuilt events fire. This way users can get ServiceProvider info when
        /// attaching while the app is running.
        /// </summary>
        [Fact]
        public void EmitsServiceProviderBuiltOnAttach()
        {
            _listener.DisableEvents(DependencyInjectionEventSource.Log);

            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(new FakeDisposeCallback());

            using ServiceProvider provider = serviceCollection.BuildServiceProvider();

            Assert.Empty(_listener.EventData);

            _listener.EnableEvents(DependencyInjectionEventSource.Log, EventLevel.Verbose);

            EventWrittenEventArgs serviceProviderBuiltEvent = _listener.EventData.Single(e => e.EventName == "ServiceProviderBuilt");
            Assert.Equal(1, GetProperty<int>(serviceProviderBuiltEvent, "singletonServices"));

            EventWrittenEventArgs serviceProviderDescriptorsEvent = _listener.EventData.Single(e => e.EventName == "ServiceProviderDescriptors");
            Assert.NotNull(JObject.Parse(GetProperty<string>(serviceProviderDescriptorsEvent, "descriptors")));
        }

        private T GetProperty<T>(EventWrittenEventArgs data, string propName)
            => (T)data.Payload[data.PayloadNames.IndexOf(propName)];

        private class TestEventListener : EventListener
        {
            private volatile bool _disposed;
            private ConcurrentQueue<EventWrittenEventArgs> _events = new ConcurrentQueue<EventWrittenEventArgs>();

            public IEnumerable<EventWrittenEventArgs> EventData => _events;

            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                if (!_disposed)
                {
                    _events.Enqueue(eventData);
                }
            }

            public override void Dispose()
            {
                _disposed = true;
                base.Dispose();
            }
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}
