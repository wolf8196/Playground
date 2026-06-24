using Spectre.Console;

namespace SignalRSample.Shared
{
    public static class EventMonitor
    {
        public static Task SpectreConsole(EventCollection collection, CancellationToken token)
        {
            return Task.Run(() =>
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

                            foreach (var evt in collection.GetEvents().OrderBy(x => x.UnitId).ThenBy(x => x.Identifier))
                            {
                                table.AddRow(
                                    evt.UnitId.ToString(),
                                    evt.Identifier,
                                    evt.Number.ToString());
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