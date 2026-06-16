using System;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace SignalRSample.HubApi
{
    internal class ExponentialBackoffWithJitter : IRetryPolicy
    {
        private readonly string url;
        private readonly TimeSpan initialDelay;
        private readonly TimeSpan? maxDelay;
        private readonly int? maxRetries;
        private readonly ILogger<ExponentialBackoffWithJitter>? logger;

        public ExponentialBackoffWithJitter(
            string url,
            TimeSpan initialDelay,
            TimeSpan? maxDelay = null,
            int? maxRetries = null,
            ILogger<ExponentialBackoffWithJitter>? logger = null)
        {
            this.url = url;
            this.initialDelay = initialDelay;
            this.maxDelay = maxDelay;
            this.maxRetries = maxRetries;
            this.logger = logger;
        }

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            ArgumentNullException.ThrowIfNull(retryContext);

            if (maxRetries.HasValue && retryContext.PreviousRetryCount >= maxRetries.Value)
            {
                return null;
            }

            var exponentialDelayMs = initialDelay.TotalMilliseconds * Math.Pow(2, retryContext.PreviousRetryCount);

            if (maxDelay.HasValue)
            {
                exponentialDelayMs = Math.Min(exponentialDelayMs, maxDelay.Value.TotalMilliseconds);
            }

            var jitteredMs = Random.Shared.NextDouble() * Math.Max(0, exponentialDelayMs);
            var delay = TimeSpan.FromMilliseconds(exponentialDelayMs + jitteredMs);

            logger?.Information("Reconnecting to {Url}... Next try in {Delay}", url, delay);
            return delay;
        }
    }
}