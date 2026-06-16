using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Polly;

namespace SignalRSample.HubApi
{
    public class HubConnectionService : BackgroundService
    {
        private readonly IReadOnlyCollection<HubConnection> connections;
        private readonly IAsyncPolicy policy;

        public HubConnectionService(IEnumerable<HubConnection>? connections = null)
        {
            policy = new ResiliencePipelineBuilder()
                .AddRetry(new Polly.Retry.RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                    UseJitter = true,
                    BackoffType = DelayBackoffType.Exponential,
                    MaxDelay = TimeSpan.FromMinutes(1),
                    Delay = TimeSpan.FromSeconds(2),
                })
                .Build()
                .AsAsyncPolicy();

            this.connections = connections?.ToArray() ?? [];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(connections.Select(c => StartConnection(c, stoppingToken)).ToArray());
        }

        private Task StartConnection(HubConnection connection, CancellationToken token)
        {
            return policy.ExecuteAsync(t => connection.StartAsync(t), token);
        }
    }
}