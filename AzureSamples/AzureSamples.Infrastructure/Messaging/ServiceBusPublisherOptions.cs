namespace AzureSamples.Infrastructure.Messaging
{
    public class ServiceBusPublisherOptions
    {
        public string ConnectionString { get; init; } = string.Empty;

        public string QueueOrTopicName { get; init; } = string.Empty;
    }
}