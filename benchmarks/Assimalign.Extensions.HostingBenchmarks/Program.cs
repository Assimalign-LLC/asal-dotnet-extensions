using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.HostingBenchmarks
{
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Hosting;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.Logging.Abstractions;

    internal class Program
    {
        static void Main(string[] args)
        {
            HostBuilder.Create()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(typeof(BenchmarkBackgroundConsoleService));
                    services.AddHostedService<BenchmakrBackgroundService>();
                })
                .Build()
                .Start();
        }
    }

    public class BenchmarkBackgroundConsoleService
    {
        public void Write()
        {
            Console.WriteLine("Background Service is Running");
        }
    }


    public class BenchmakrBackgroundService : HostBackgroundService
    {
        private readonly ILogger<BenchmakrBackgroundService> logger;
        private readonly BenchmarkBackgroundConsoleService service;
        public BenchmakrBackgroundService(
            ILogger<BenchmakrBackgroundService> logger,
            BenchmarkBackgroundConsoleService service)
        {
            this.logger = logger;
            this.service = service;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Thread.Sleep(2000);
                service.Write();
                logger.LogError("Some Information");
            }
        }
    }
}
