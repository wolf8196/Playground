using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using SignalRSample.Api;
using SignalRSample.Api.Client;

namespace SignalRSample.Client.HubClients
{
    internal sealed class MessageApiReceiver : MessageApiClient
    {
        private readonly ILogger<MessageApiReceiver> logger;

        public MessageApiReceiver(HubConnection connection, ILogger<MessageApiReceiver> logger)
            : base(connection)
        {
            this.logger = logger;
        }

        public override Task ReceiveMessageAsync(MessageDto message)
        {
            logger.Information("Received message from {User}: {Message}", message.UserName, message.Text);
            return Task.CompletedTask;
        }

        public override Task OnClosed(Exception? exception)
        {
            logger.Information(exception, "Connection closed.");
            return base.OnClosed(exception);
        }

        public override Task OnReconnecting(Exception? exception)
        {
            logger.Information(exception, "Reconnecting...");
            return base.OnReconnecting(exception);
        }

        public override Task OnReconnected(string? connectionId)
        {
            logger.Information("Reconnected...");
            return base.OnReconnected(connectionId);
        }
    }
}