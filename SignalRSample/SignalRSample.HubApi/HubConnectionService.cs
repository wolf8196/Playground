using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;

namespace SignalRSample.HubApi
{
    internal class HubConnectionService : BackgroundService
    {
        private const string CtxKey = "Connection";
        private readonly IReadOnlyCollection<NamedHubConnection> connections;
        private readonly IAsyncPolicy policy;

        public HubConnectionService(
            ILogger<HubConnectionService> logger,
            IEnumerable<NamedHubConnection>? connections = null,
            IEnumerable<HubListener>? listeners = null)
        {
            policy = new ResiliencePipelineBuilder()
                .AddRetry(new Polly.Retry.RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                    UseJitter = true,
                    BackoffType = DelayBackoffType.Exponential,
                    MaxDelay = TimeSpan.FromMinutes(1),
                    MaxRetryAttempts = int.MaxValue,
                    Delay = TimeSpan.FromSeconds(2),
                    OnRetry = (args) =>
                    {
                        args.Context.Properties.TryGetValue<NamedHubConnection>(new(CtxKey), out var value);
                        logger.Error("Failed to establish initial connection to {Url}", value?.Url);
                        return ValueTask.CompletedTask;
                    }
                })
                .Build()
                .AsAsyncPolicy();

            this.connections = connections?.ToArray() ?? [];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(connections.Select(c => StartConnection(c, stoppingToken)).ToArray());
        }

        private Task StartConnection(NamedHubConnection namedConnection, CancellationToken token)
        {
            return policy.ExecuteAsync(
                (ctx, t) => namedConnection.Connection.StartAsync(t),
                new Dictionary<string, object> { { CtxKey, namedConnection } },
                token);
        }
    }
}