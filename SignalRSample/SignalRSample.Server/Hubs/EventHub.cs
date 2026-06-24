using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using SignalRSample.Api;

namespace SignalRSample.Server.Hubs
{
    public class EventHub : Hub<IEventReceiver>, IEventSender
    {
        private readonly ILogger<EventHub> logger;

        public EventHub(ILogger<EventHub> logger)
        {
            this.logger = logger;
        }

        public async Task Subscribe(EventSubscriptionDto subscription)
        {
            foreach (var unitId in subscription.UnitIds)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, unitId.ToString());
                logger.Information("{ConnectionId} subscribed to group {UnitId}", Context.ConnectionId, unitId);
            }
        }

        public async Task Unsubscribe(EventSubscriptionDto subscription)
        {
            foreach (var unitId in subscription.UnitIds)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, unitId.ToString());
                logger.Information("{ConnectionId} unsubscribed from group {UnitId}", Context.ConnectionId, unitId);
            }
        }

        public override Task OnConnectedAsync()
        {
            logger.Information("{ConectionId} connected.", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            logger.Information(exception, "{ConectionId} disconnected.", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}