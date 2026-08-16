namespace AzureSamples.Api.Options
{
    public class ApiOptions
    {
        public required string DbConnectionString { get; init; }

        public required string ServiceBusConnectionString { get; init; }

        public required string EventsQueue { get; init; }
    }
}