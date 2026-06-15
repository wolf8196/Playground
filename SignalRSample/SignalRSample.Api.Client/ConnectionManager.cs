using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace SignalRSample.Api.Client
{
    public class ConnectionManager : BackgroundService
    {
        private readonly IReadOnlyCollection<HubConnection> connections;

        public ConnectionManager(IEnumerable<HubConnection>? connections = null)
        {
            this.connections = connections?.ToArray() ?? [];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var connection in connections)
            {
                await connection.StartAsync(stoppingToken);
            }
        }
    }
}