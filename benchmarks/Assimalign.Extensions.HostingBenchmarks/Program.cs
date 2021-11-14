using System;

namespace Assimalign.Extensions.HostingBenchmarks
{
    using Assimalign.Extensions.Hosting;
    using Assimalign.Extensions.DependencyInjection;
    using System.Threading;
    using System.Threading.Tasks;

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
        private readonly BenchmarkBackgroundConsoleService service;
        public BenchmakrBackgroundService(
            BenchmarkBackgroundConsoleService service)
        {
            this.service = service;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Thread.Sleep(2000);
                service.Write();
            }
        }
    }
}
