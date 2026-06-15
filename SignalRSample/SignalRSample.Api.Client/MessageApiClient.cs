using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace SignalRSample.Api.Client
{
    internal sealed class MessageApiClient
        : BidirectionalApiClient<IMessageSender, IMessageReceiver>, IMessageSender
    {
        public MessageApiClient(
            HubConnection connection,
            IEnumerable<IMessageReceiver> receivers)
            : base(connection, receivers)
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

        public override void AddReceiver(HubConnection connection, IMessageReceiver receiver)
        {
            connection.Register(receiver);
        }
    }
}