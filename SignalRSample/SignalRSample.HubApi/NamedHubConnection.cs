using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample.HubApi
{
    internal class NamedHubConnection
    {
        public required string Route { get; init; }

        public required string Url { get; init; }

        public required HubConnection Connection { get; init; }
    }
}