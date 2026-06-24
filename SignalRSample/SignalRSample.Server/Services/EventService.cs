using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Serilog;
using SignalRSample.Api;
using SignalRSample.Server.Hubs;
using Spectre.Console;

namespace SignalRSample.Server.Services
{
    public class EventService : BackgroundService
    {
        private readonly ConcurrentDictionary<int, List<EventDto>> units;
        private readonly IHubContext<EventHub, IEventReceiver> hubContext;
        private readonly ILogger<EventService> logger;

        public EventService(
            IHubContext<EventHub, IEventReceiver> hubContext,
            ILogger<EventService> logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            units = new ConcurrentDictionary<int, List<EventDto>>(
                Enumerable.Range(1, 5)
                .ToDictionary(x => x, x => new List<EventDto>()));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var simuTasks = units.Keys.Select(i => SimulateUnitEvents(i, stoppingToken))
                .Append(Monitor(stoppingToken))
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
                    var evts = units[unitId];
                    var evtId = $"Event-{Random.Shared.NextInt64(1, 4)}";

                    switch (operation)
                    {
                        case Operation.Add:
                            {
                                if (evts.Exists(x => x.Identifier == evtId))
                                {
                                    continue;
                                }

                                var evt = new EventDto
                                {
                                    UnitId = unitId,
                                    Identifier = evtId,
                                    Number = 1,
                                };
                                units[unitId].Add(evt);
                                await hubContext.Clients.Group(unitId.ToString()).EventAdded(evt);
                                break;
                            }

                        case Operation.Remove:
                            {
                                var idx = evts.FindIndex(x => x.Identifier == evtId);
                                if (idx >= 0)
                                {
                                    var evt = evts[idx];
                                    evts.RemoveAt(idx);
                                    await hubContext.Clients.Group(unitId.ToString()).EventRemoved(evt);
                                }

                                break;
                            }

                        case Operation.Update:
                            {
                                var evt = evts.FirstOrDefault(x => x.Identifier == evtId);
                                if (evt != null)
                                {
                                    evt.Number++;
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

        private async Task Monitor(CancellationToken token)
        {
            await Task.Delay(3000, token);
            await Task.Run(() =>
            {
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

                            foreach (var unit in units.ToDictionary())
                            {
                                foreach (var evt in unit.Value)
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

            //while (!token.IsCancellationRequested)
            //{
            //    logger.Information("Event status:");
            //    logger.Information("{@Events}", units
            //        .Select(x => new
            //        {
            //            UnitId = x.Key,
            //            Events = x.Value.Select(y => $"{y.Identifier}-{y.Number}")
            //        }));

            //    await Task.Delay(2000, token);
            //}
        }

        private enum Operation
        {
            Add,
            Remove,
            Update
        }
    }
}