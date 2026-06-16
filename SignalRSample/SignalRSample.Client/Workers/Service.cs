using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SignalRSample.Api;

namespace SignalRSample.Client.Workers
{
    internal sealed class Service : BackgroundService
    {
        private readonly IMessageSender messageSender;
        private readonly ILogger<Service> logger;

        public Service(IMessageSender messageSender, ILogger<Service> logger)
        {
            this.logger = logger;
            this.messageSender = messageSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var user = $"User{new Random().Next(10)}";

            try
            {
                logger.Information("Starting client with User {User}", user);

                while (true)
                {
                    var message = Console.ReadLine() ?? string.Empty;
                    await messageSender.SendMessageAsync(new MessageDto { UserName = user, Text = message });
                    logger.Information("Sent message: {Message}", message);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred in client.");
            }
        }
    }
}