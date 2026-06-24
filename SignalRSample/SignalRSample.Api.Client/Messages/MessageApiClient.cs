using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRSample.HubApi;
using TypedSignalR.Client;

namespace SignalRSample.Api.Client.Messages
{
    public class MessageApiClient
        : HubApiClient<IMessageSender>, IMessageSender
    {
        public MessageApiClient(HubConnection connection)
            : base(connection)
        {
        }

        public Task SendMessageAsync(MessageDto message)
        {
            return Proxy.SendMessageAsync(message);
        }

        public override IMessageSender CreateProxy(HubConnection connection)
        {
            return connection.CreateHubProxy<IMessageSender>();
        }
    }
}