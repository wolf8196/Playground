using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample.HubApi
{
    public abstract class HubApiClient<TSender>
        where TSender : class
    {
        public HubApiClient(HubConnection connection)
        {
            Connection = connection;
            Proxy = CreateProxy(connection);
        }

        protected HubConnection Connection { get; }

        protected TSender Proxy { get; }

        public abstract TSender CreateProxy(HubConnection connection);
    }
}