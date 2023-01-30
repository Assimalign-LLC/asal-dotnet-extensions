


using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    //[ActiveIssue("https://github.com/dotnet/runtime/issues/33894", TestRuntimes.Mono)]
    public class ServiceProviderCompilationTest
    {
        [Theory]
        [InlineData(ServiceProviderEngine.Default, typeof(I999))]
        [InlineData(ServiceProviderEngine.Dynamic, typeof(I999))]
        [InlineData(ServiceProviderEngine.Runtime, typeof(I999))]
        [InlineData(ServiceProviderEngine.ILEmit, typeof(I999))]
        [InlineData(ServiceProviderEngine.Expressions, typeof(I999))]
        //[ActiveIssue("https://github.com/dotnet/runtime/issues/33894", TestRuntimes.Mono)]
        private async Task CompilesInLimitedStackSpace(ServiceProviderEngine mode, Type serviceType)
        {
            // Arrange
            var stackSize = 256 * 1024;
            var serviceCollection = new ServiceProviderBuilder();
            CompilationTestDataProvider.Register(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider(mode);

            // Act + Assert

            var tsc = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
                {
                    try
                    {
                        object service = null;
                        for (int i = 0; i < 10; i++)
                        {
                            service = serviceProvider.GetService(serviceType);
                        }
                        tsc.SetResult(service);
                    }
                    catch (Exception ex)
                    {
                        tsc.SetException(ex);
                    }
                }, stackSize);

            thread.Start();
            thread.Join();
            await tsc.Task;
        }
    }
}
