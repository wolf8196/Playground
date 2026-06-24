using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using SignalRSample.Api;
using SignalRSample.Server.Services;

namespace SignalRSample.Server.Hubs
{
    public class EventHub : Hub<IEventReceiver>, IEventSender
    {
        private readonly IEventService eventService;
        private readonly ILogger<EventHub> logger;

        public EventHub(IEventService eventService, ILogger<EventHub> logger)
        {
            this.eventService = eventService;
            this.logger = logger;
        }

        public async Task Subscribe(EventSubscriptionDto subscription)
        {
            foreach (var unitId in subscription.UnitIds)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, unitId.ToString());
                foreach (var evt in eventService.GetEvents(unitId))
                {
                    await Clients.Caller.EventAdded(evt);
                }

                logger.Debug("{ConnectionId} subscribed to group {UnitId}", Context.ConnectionId, unitId);
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