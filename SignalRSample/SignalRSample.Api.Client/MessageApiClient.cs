using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRSample.HubApi;
using TypedSignalR.Client;

namespace SignalRSample.Api.Client
{
    public class MessageApiClient
        : HubApiClient<IMessageSender, IMessageReceiver>, IMessageSender, IMessageReceiver
    {
        public MessageApiClient(HubConnection connection)
            : base(connection)
        {
            Subscription = AddReceiver(connection, this);
        }

        public Task SendMessageAsync(MessageDto message)
        {
            return Proxy.SendMessageAsync(message);
        }

        public virtual Task ReceiveMessageAsync(MessageDto message) => Task.CompletedTask;

        // required to specify real interfaces
        // otherwise proxy won't generate
        public override IMessageSender CreateProxy(HubConnection connection)
        {
            return connection.CreateHubProxy<IMessageSender>();
        }

        public override IDisposable AddReceiver(HubConnection connection, IMessageReceiver receiver)
        {
            return connection.Register<IMessageReceiver>(this);
        }
    }
}