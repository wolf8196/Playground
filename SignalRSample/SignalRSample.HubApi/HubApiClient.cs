using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample.HubApi
{
    public abstract class HubApiClient<TSender, TReceiver>
        where TSender : class
        where TReceiver : class
    {
        public HubApiClient(
            HubConnection connection,
            IEnumerable<TReceiver>? receivers = null)
        {
            Connection = connection;
            Proxy = CreateProxy(connection);
            receivers ??= [];

            foreach (var receiver in receivers)
            {
                AddReceiver(connection, receiver);
            }
        }

        protected HubConnection Connection { get; }

        protected TSender Proxy { get; }

        public abstract TSender CreateProxy(HubConnection connection);

        public abstract void AddReceiver(HubConnection connection, TReceiver receiver);
    }
}