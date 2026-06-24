using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SignalRSample.Api;
using SignalRSample.Shared;

namespace SignalRSample.Client.Services
{
    internal sealed class EventService : BackgroundService, IEventService
    {
        private readonly EventCollection events;

        private readonly IEventSender eventSender;
        private readonly ILogger<EventService> logger;

        private List<int>? subscribtion;

        public EventService(IEventSender eventSender, ILogger<EventService> logger)
        {
            this.eventSender = eventSender;
            this.logger = logger;

            events = new EventCollection();
        }

        public void AddEvent(EventDto evt)
        {
            events.AddEvent(evt);
        }

        public void RemoveEvent(EventDto evt)
        {
            events.RemoveEvent(evt);
        }

        public void UpdateEvent(EventDto evt)
        {
            events.UpdateEvent(evt);
        }

        public async Task Subscribe()
        {
            if (subscribtion != null)
            {
                logger.Information("Subscribing to {UnitIds}", subscribtion);

                await eventSender.Subscribe(new EventSubscriptionDto
                {
                    UnitIds = subscribtion,
                });

                logger.Information("Subscribed to {UnitIds}", subscribtion);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken);

            logger.Information("Enter unit ids to subscribe to:");

            // subscribtion = Enumerable.Range(0, 2).Select(x => (int)Random.Shared.NextInt64(0, 5)).Distinct().ToList();
            // subscribtion = Console.ReadLine()?.Split(',').Select(int.Parse).ToArray() ?? [];
            subscribtion = Enumerable.Range(1, 5).ToList();

            await Subscribe();

            await Task.Delay(4000, stoppingToken);
            await EventMonitor.SpectreConsole(events, stoppingToken);
        }
    }
}