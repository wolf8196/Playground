using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Retry;

namespace SignalRSample.HubApi
{
    public class HubConnectionManager : BackgroundService
    {
        private readonly IReadOnlyCollection<HubConnection> connections;
        private readonly AsyncRetryPolicy retryPolicy;

        public HubConnectionManager(IEnumerable<HubConnection>? connections = null)
        {
            this.connections = connections?.ToArray() ?? [];

            retryPolicy = Policy.Handle<Exception>().WaitAndRetryForeverAsync(
                attempt => TimeSpan.FromSeconds(Math.Min(Math.Pow(2, attempt), 60)));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(connections.Select(c => StartConnection(c, stoppingToken)).ToArray());
        }

        private Task StartConnection(HubConnection connection, CancellationToken token)
        {
            return retryPolicy.ExecuteAsync(t => connection.StartAsync(t), token);
        }
    }
}