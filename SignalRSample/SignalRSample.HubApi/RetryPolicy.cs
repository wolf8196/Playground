using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample.HubApi
{
    internal class ExponentialBackoffWithJitter : IRetryPolicy
    {
        private readonly TimeSpan initialDelay;
        private readonly TimeSpan? maxDelay;
        private readonly int maxRetries;

        public ExponentialBackoffWithJitter(TimeSpan initialDelay, TimeSpan? maxDelay = null, int maxRetries = 5)
        {
            this.initialDelay = initialDelay;
            this.maxDelay = maxDelay;
            this.maxRetries = maxRetries;
        }

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            ArgumentNullException.ThrowIfNull(retryContext);

            if (maxRetries >= 0 && retryContext.PreviousRetryCount >= maxRetries)
            {
                return null;
            }

            var exponentialDelayMs = initialDelay.TotalMilliseconds * Math.Pow(2, retryContext.PreviousRetryCount);

            if (maxDelay.HasValue)
            {
                exponentialDelayMs = Math.Min(exponentialDelayMs, maxDelay.Value.TotalMilliseconds);
            }

            var jitteredMs = Random.Shared.NextDouble() * Math.Max(0, exponentialDelayMs);

            var delay = TimeSpan.FromMilliseconds(Math.Max(0, Math.Floor(jitteredMs)));

            return delay;
        }
    }
}