using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SignalRSample.Api;
using Spectre.Console;

namespace SignalRSample.Client.Services
{
    internal sealed class EventService : BackgroundService, IEventService
    {
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, EventDto>> units;

        private readonly IEventSender eventSender;
        private readonly ILogger<EventService> logger;

        private List<int>? subscribtion;

        public EventService(IEventSender eventSender, ILogger<EventService> logger)
        {
            this.eventSender = eventSender;
            this.logger = logger;

            units = new ConcurrentDictionary<int, ConcurrentDictionary<string, EventDto>>();
        }

        public void AddEvent(EventDto evt)
        {
            units.AddOrUpdate(
                evt.UnitId,
                key => new ConcurrentDictionary<string, EventDto>(),
                (key, existing) =>
                {
                    existing.TryAdd(evt.Identifier, evt);
                    return existing;
                });
        }

        public void RemoveEvent(EventDto evt)
        {
            units.AddOrUpdate(
                evt.UnitId,
                key => new ConcurrentDictionary<string, EventDto>(),
                (key, existing) =>
                {
                    existing.TryRemove(evt.Identifier, out _);
                    return existing;
                });
        }

        public void UpdateEvent(EventDto evt)
        {
            units.AddOrUpdate(
                evt.UnitId,
                key => new ConcurrentDictionary<string, EventDto>(),
                (key, existing) =>
                {
                    existing.AddOrUpdate(evt.Identifier,
                        key => evt,
                        (key, existing) => evt);
                    return existing;
                });
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
            await Task.Delay(1000, stoppingToken);

            logger.Information("Enter unit ids to subscribe to:");

            subscribtion = Enumerable.Range(0, 2).Select(x => (int)Random.Shared.NextInt64(0, 5)).Distinct().ToList();
            // var subscribtion = Console.ReadLine()?.Split(',').Select(int.Parse).ToArray() ?? [];

            await Subscribe();

            await Monitor(stoppingToken);
        }

        private async Task Monitor(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(4000);

                var table = new Table()
                   .AddColumn("Unit")
                   .AddColumn("EventId")
                   .AddColumn("Number");

                AnsiConsole.Live(table)
                    .Start(ctx =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            table.Rows.Clear();

                            foreach (var unit in units)
                            {
                                foreach (var evt in unit.Value.Values)
                                {
                                    table.AddRow(
                                        evt.UnitId.ToString(),
                                        evt.Identifier,
                                        evt.Number.ToString());
                                }
                            }

                            ctx.Refresh();
                            Thread.Sleep(1000);
                        }
                    });
            },
            token);
        }
    }
}