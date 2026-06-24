using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRSample.HubApi;
using TypedSignalR.Client;

namespace SignalRSample.Api.Client.Events
{
    public class EventApiClient
        : HubApiClient<IEventSender>, IEventSender
    {
        public EventApiClient(HubConnection connection)
            : base(connection)
        {
        }

        public Task Subscribe(EventSubscriptionDto subscription) => Proxy.Subscribe(subscription);

        public Task Unsubscribe(EventSubscriptionDto subscription) => Proxy.Unsubscribe(subscription);

        public override IEventSender CreateProxy(HubConnection connection)
        {
            return connection.CreateHubProxy<IEventSender>();
        }
    }
}