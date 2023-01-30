using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using Assimalign.Extensions.DependencyInjection.MockObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection;

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
        typeof(ServiceEventSource)
            .GetField("providers", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(ServiceEventSource.Log, new List<WeakReference<ServiceProvider>>());

        _listener.EnableEvents(ServiceEventSource.Log, EventLevel.Verbose);
    }

    [Fact]
    public void ExistsWithCorrectId()
    {
        var esType = typeof(ServiceEventSource);

        Assert.NotNull(esType);

        Assert.Equal("Assimalign-Extensions-DependencyInjection", EventSource.GetName(esType));
        Assert.Equal(Guid.Parse("0822a912-6c71-5e61-e8da-26b4732bc141"), EventSource.GetGuid(esType));
        Assert.NotEmpty(EventSource.GenerateManifest(esType, "assemblyPathToIncludeInManifest"));
    }

    [Fact]
    public void EmitsCallSiteBuiltEvent()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        var fakeDisposeCallback = new FakeDisposeCallback();
        builder.AddSingleton(fakeDisposeCallback);
        builder.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
        builder.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
        builder.AddSingleton<IFakeMultipleService>(provider => new FakeDisposableCallbackInnerService(fakeDisposeCallback));
        builder.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
        builder.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
        builder.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();

        builder.Build().GetService<IEnumerable<IFakeOuterService>>();

        var callsiteBuiltEvent = _listener.EventData.Single(e => e.EventName == "CallSiteBuilt");

        var content = JObject.Parse(GetProperty<string>(callsiteBuiltEvent, "callSite")).ToString();
        
        Assert.StartsWith(
            string.Join(Environment.NewLine,
            "{",
            "  \"serviceType\": \"System.Collections.Generic.IEnumerable`1[Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOuterService]\",",
            "  \"kind\": \"Enumerable\",",
            "  \"cache\": \"None\",",
            "  \"itemType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOuterService\",",
            "  \"size\": \"1\",",
            "  \"items\": [",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOuterService\",",
            "      \"kind\": \"Constructor\",",
            "      \"cache\": \"Dispose\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackOuterService\",",
            "      \"arguments\": [",
            "        {",
            "          \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeService\",",
            "          \"kind\": \"Constructor\",",
            "          \"cache\": \"Root\",",
            "          \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\",",
            "          \"arguments\": [",
            "            {",
            "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\",",
            "              \"kind\": \"Constant\",",
            "              \"cache\": \"None\",",
            "              \"value\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\"",
            "            }",
            "          ]",
            "        },",
            "        {",
            "          \"serviceType\": \"System.Collections.Generic.IEnumerable`1[Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService]\",",
            "          \"kind\": \"Enumerable\",",
            "          \"cache\": \"None\",",
            "          \"itemType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "          \"size\": \"4\",",
            "          \"items\": [",
            "            {",
            "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "              \"kind\": \"Constructor\",",
            "              \"cache\": \"Root\",",
            "              \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\",",
            "              \"arguments\": [",
            "                {",
            "                  \"ref\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\"",
            "                }",
            "              ]",
            "            },",
            "            {",
            "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "              \"kind\": \"Factory\",",
            "              \"cache\": \"Root\",",
            "              \"method\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService <EmitsCallSiteBuiltEvent>b__0(System.IServiceProvider)\"",
            "            },",
            "            {",
            "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "              \"kind\": \"Constructor\",",
            "              \"cache\": \"Scope\",",
            "              \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\",",
            "              \"arguments\": [",
            "                {",
            "                  \"ref\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\"",
            "                }",
            "              ]",
            "            },",
            "            {",
            "              \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "              \"kind\": \"Constructor\",",
            "              \"cache\": \"Dispose\",",
            "              \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\",",
            "              \"arguments\": [",
            "                {",
            "                  \"ref\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\"",
            "                }",
            "              ]",
            "            }",
            "          ]",
            "        },",
            "        {",
            "          \"ref\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\"",
            "        }",
            "      ]",
            "    }",
            "  ]",
            "}"),content);

        Assert.Equal("System.Collections.Generic.IEnumerable`1[Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOuterService]", GetProperty<string>(callsiteBuiltEvent, "serviceType"));
        Assert.Equal(0, GetProperty<int>(callsiteBuiltEvent, "chunkIndex"));
        Assert.Equal(1, GetProperty<int>(callsiteBuiltEvent, "chunkCount"));
        Assert.Equal(1, callsiteBuiltEvent.EventId);
    }

    [Fact]
    public void EmitsServiceResolvedEvent()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        builder.AddSingleton<IFakeService, FakeService>();

        var serviceProvider = builder.Build();

        serviceProvider.GetService<IFakeService>();
        serviceProvider.GetService<IFakeService>();
        serviceProvider.GetService<IFakeService>();

        var serviceResolvedEvents = _listener.EventData.Where(e => e.EventName == "ServiceResolved").ToArray();

        Assert.Equal(3, serviceResolvedEvents.Length);
        foreach (var serviceResolvedEvent in serviceResolvedEvents)
        {
            Assert.Equal("Assimalign.Extensions.DependencyInjection.MockObjects.IFakeService", GetProperty<string>(serviceResolvedEvent, "serviceType"));
            Assert.Equal(2, serviceResolvedEvent.EventId);
        }
    }

    [Fact]
    public void EmitsExpressionTreeBuiltEvent()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        builder.AddTransient<IFakeService, FakeService>();

        var serviceProvider = builder.BuildServiceProvider(ServiceProviderEngine.Expressions);

        serviceProvider.GetService<IFakeService>();

        var expressionTreeGeneratedEvent = _listener.EventData.Single(e => e.EventName == "ExpressionTreeGenerated");

        Assert.Equal("Assimalign.Extensions.DependencyInjection.MockObjects.IFakeService", GetProperty<string>(expressionTreeGeneratedEvent, "serviceType"));
        Assert.Equal(9, GetProperty<int>(expressionTreeGeneratedEvent, "nodeCount"));
        Assert.Equal(3, expressionTreeGeneratedEvent.EventId);
    }

    //[Fact]
    //[ActiveIssue("https://github.com/dotnet/runtime/issues/35753", TestPlatforms.Windows)]
    //public void EmitsDynamicMethodBuiltEvent()
    //{
    //    // Arrange
    //    var builder = new ServiceCollection();
    //    builder.AddTransient<IFakeService, FakeService>();

    //    var serviceProvider = builder.BuildServiceProvider(ServiceProviderMode.ILEmit);

    //    serviceProvider.GetService<IFakeService>();

    //    var expressionTreeGeneratedEvent = _listener.EventData.Single(e => e.EventName == "DynamicMethodBuilt");

    //    Assert.Equal("Assimalign.Extensions.DependencyInjection.MockObjects.IFakeService", GetProperty<string>(expressionTreeGeneratedEvent, "serviceType"));
    //    Assert.Equal(12, GetProperty<int>(expressionTreeGeneratedEvent, "methodSize"));
    //    Assert.Equal(4, expressionTreeGeneratedEvent.EventId);
    //}

    [Fact]
    public void EmitsScopeDisposedEvent()
    {
        var builder = new ServiceProviderBuilder();
        builder.AddScoped<IFakeService, FakeService>();

        var serviceProvider = builder.Build();

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
        ServiceEventSource.Log.ServiceRealizationFailed(exception, 1234);

        var eventName = nameof(ServiceEventSource.Log.ServiceRealizationFailed);
        var serviceRealizationFailedEvent = _listener.EventData.Single(e => e.EventName == eventName);

        Assert.Equal("System.Exception: Test error.", GetProperty<string>(serviceRealizationFailedEvent, "exceptionMessage"));
        Assert.Equal(1234, GetProperty<int>(serviceRealizationFailedEvent, "serviceProviderHashCode"));
        Assert.Equal(6, serviceRealizationFailedEvent.EventId);
    }

    [Fact]
    public void EmitsServiceProviderBuilt()
    {
        ServiceProviderBuilder builder = new();
        FakeDisposeCallback fakeDisposeCallback = new();
        builder.AddSingleton(fakeDisposeCallback);
        builder.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
        builder.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
        builder.AddSingleton<IFakeMultipleService>(provider => new FakeDisposableCallbackInnerService(fakeDisposeCallback));
        builder.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
        builder.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
        builder.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();
        builder.AddScoped(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
        builder.AddTransient<IFakeOpenGenericService<PocoClass>, FakeOpenGenericService<PocoClass>>();
        
        using ServiceProvider provider = (ServiceProvider)builder.Build();

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
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback\",",
            "      \"lifetime\": \"Singleton\",",
            "      \"implementationInstance\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposeCallback (instance)\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOuterService\",",
            "      \"lifetime\": \"Transient\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackOuterService\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "      \"lifetime\": \"Singleton\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "      \"lifetime\": \"Singleton\",",
            "      \"implementationFactory\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService <EmitsServiceProviderBuilt>b__0(System.IServiceProvider)\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "      \"lifetime\": \"Scoped\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeMultipleService\",",
            "      \"lifetime\": \"Transient\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeService\",",
            "      \"lifetime\": \"Singleton\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeDisposableCallbackInnerService\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOpenGenericService`1[TValue]\",",
            "      \"lifetime\": \"Scoped\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeOpenGenericService`1[TVal]\"",
            "    },",
            "    {",
            "      \"serviceType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.IFakeOpenGenericService`1[Assimalign.Extensions.DependencyInjection.MockObjects.PocoClass]\",",
            "      \"lifetime\": \"Transient\",",
            "      \"implementationType\": \"Assimalign.Extensions.DependencyInjection.MockObjects.FakeOpenGenericService`1[Assimalign.Extensions.DependencyInjection.MockObjects.PocoClass]\"",
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
        _listener.DisableEvents(ServiceEventSource.Log);

        ServiceProviderBuilder builder = new();
        builder.AddSingleton(new FakeDisposeCallback());

        using ServiceProvider provider = (ServiceProvider)builder.Build();

        Assert.Empty(_listener.EventData);

        _listener.EnableEvents(ServiceEventSource.Log, EventLevel.Verbose);

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
