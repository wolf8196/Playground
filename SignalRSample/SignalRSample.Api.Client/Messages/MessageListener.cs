using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRSample.HubApi;
using TypedSignalR.Client;

namespace SignalRSample.Api.Client.Messages
{
    public abstract class MessageListener : HubListener, IMessageReceiver
    {
        protected MessageListener(HubConnection connection)
            : base(connection)
        {
        }

        public abstract Task ReceiveMessageAsync(MessageDto message);

        public override IDisposable AddReceiver(HubConnection connection)
        {
            return connection.Register<IMessageReceiver>(this);
        }
    }
}