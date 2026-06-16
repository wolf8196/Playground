using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SignalRSample.Server.Services
{
    public class ControlService : BackgroundService
    {
        private readonly IHostApplicationLifetime lifetime;
        private readonly ILogger<ControlService> logger;

        public ControlService(IHostApplicationLifetime lifetime, ILogger<ControlService> logger)
        {
            this.lifetime = lifetime;
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var command = (Console.ReadLine() ?? string.Empty).Split();

                if (command.Length == 2 && command[0] == "stop")
                {
                    Program.Delay = TimeSpan.FromSeconds(int.Parse(command[1]));
                    lifetime.StopApplication();
                }
            }

            return Task.CompletedTask;
        }
    }
}