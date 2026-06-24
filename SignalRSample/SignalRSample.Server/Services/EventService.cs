using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Serilog;
using SignalRSample.Api;
using SignalRSample.Server.Hubs;
using SignalRSample.Shared;
using Spectre.Console;

namespace SignalRSample.Server.Services
{
    public class EventService : BackgroundService, IEventService
    {
        private readonly EventCollection events;
        private readonly IHubContext<EventHub, IEventReceiver> hubContext;
        private readonly ILogger<EventService> logger;

        public EventService(
            IHubContext<EventHub, IEventReceiver> hubContext,
            ILogger<EventService> logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            events = new EventCollection();
        }

        public IReadOnlyCollection<EventDto> GetEvents(int unitId)
        {
            return events.GetEvents(unitId);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);
            var simuTasks = Enumerable.Range(1, 5).Select(i => SimulateUnitEvents(i, stoppingToken))
                .Append(EventMonitor.SpectreConsole(events, stoppingToken))
                .ToArray();

            await Task.WhenAll(simuTasks);
        }

        private async Task SimulateUnitEvents(int unitId, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var ops = new[]
                    {
                        (Operation.Update, 30),
                        (Operation.Add, 10),
                        (Operation.Remove, 10)
                    }
                    .SelectMany(x => Enumerable.Repeat(x.Item1, x.Item2))
                    .Shuffle()
                    .ToList();

                    var operation = ops[(int)Random.Shared.NextInt64(0, ops.Count - 1)];
                    var evts = events.GetEvents(unitId);
                    var evtId = $"Event-{Random.Shared.NextInt64(1, 4)}";
                    var evt = evts.FirstOrDefault(x => x.Identifier == evtId);

                    switch (operation)
                    {
                        case Operation.Add:
                            {
                                if (evt != null)
                                {
                                    continue;
                                }

                                evt = new EventDto
                                {
                                    UnitId = unitId,
                                    Identifier = evtId,
                                    Number = 1,
                                };
                                events.AddEvent(evt);
                                await hubContext.Clients.Group(unitId.ToString()).EventAdded(evt);
                                break;
                            }

                        case Operation.Remove:
                            {
                                if (evt != null)
                                {
                                    events.RemoveEvent(evt);
                                    await hubContext.Clients.Group(unitId.ToString()).EventRemoved(evt);
                                }

                                break;
                            }

                        case Operation.Update:
                            {
                                if (evt != null)
                                {
                                    evt.Number++;
                                    events.UpdateEvent(evt);
                                    await hubContext.Clients.Group(unitId.ToString()).EventUpdated(evt);
                                }
                                break;
                            }

                        default:
                            break;
                    }

                    await Task.Delay(1000 + (int)Random.Shared.NextInt64(0, 1000), token);
                }
            },
            token);
        }

        private enum Operation
        {
            Add,
            Remove,
            Update
        }
    }
}