using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using SignalRSample.Api;
using SignalRSample.Api.Client.Events;
using SignalRSample.Client.Services;

namespace SignalRSample.Client.HubListeners
{
    internal sealed class EventReceiver : EventListener
    {
        private readonly IEventService eventService;
        private readonly ILogger<EventReceiver> logger;

        public EventReceiver(
            HubConnection connection,
            IEventService eventService,
            ILogger<EventReceiver> logger)
            : base(connection)
        {
            this.eventService = eventService;
            this.logger = logger;
        }

        public override Task EventAdded(EventDto evt)
        {
            eventService.AddEvent(evt);
            return Task.CompletedTask;
        }

        public override Task EventRemoved(EventDto evt)
        {
            eventService.RemoveEvent(evt);
            return Task.CompletedTask;
        }

        public override Task EventUpdated(EventDto evt)
        {
            eventService.UpdateEvent(evt);
            return Task.CompletedTask;
        }

        public override Task OnClosed(Exception? exception)
        {
            logger.Information("Connection closed. Error: {Error}", exception?.Message);
            return base.OnClosed(exception);
        }

        public override Task OnReconnecting(Exception? exception)
        {
            logger.Information("Reconnecting... Error: {Error}", exception?.Message);
            return base.OnReconnecting(exception);
        }

        public override Task OnReconnected(string? connectionId)
        {
            logger.Information("Reconnected. ConnectionId: {ConnectionId}", connectionId);
            return base.OnReconnected(connectionId);
        }
    }
}