using AzureSamples.Infrastructure.Messaging;

namespace AzureSamples.Infrastructure
{
    public class InfrastructureOptions
    {
        public required string DbConnectionString { get; init; }

        public required ServiceBusPublisherOptions EventPublishOptions { get; init; }
    }
}