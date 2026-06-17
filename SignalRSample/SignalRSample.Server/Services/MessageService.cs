using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SignalRSample.Api;
using SignalRSample.Server.Hubs;

namespace SignalRSample.Server.Services
{
    public class MessageService : BackgroundService
    {
        private readonly IHubContext<MessageHub, IMessageReceiver> hubContext;

        public MessageService(IHubContext<MessageHub, IMessageReceiver> hubContext)
        {
            this.hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await hubContext.Clients.All.ReceiveMessageAsync(new MessageDto
                {
                    UserName = "Server",
                    Text = $"Ping. Date: {DateTime.Now}",
                });

                await Task.Delay(
                    TimeSpan.FromSeconds(10) + TimeSpan.FromSeconds(Random.Shared.NextInt64(5)),
                    stoppingToken);
            }
        }
    }
}