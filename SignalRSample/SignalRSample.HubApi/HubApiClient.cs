using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample.HubApi
{
    public abstract class HubApiClient<TSender, TReceiver> : IDisposable
        where TSender : class
        where TReceiver : class
    {
        public HubApiClient(HubConnection connection)
        {
            Connection = connection;
            Proxy = CreateProxy(connection);
            connection.Closed += OnClosed;
            connection.Reconnecting += OnReconnecting;
            connection.Reconnected += OnReconnected;
        }

        protected HubConnection Connection { get; }

        protected TSender Proxy { get; }

        protected IDisposable? Subscription { get; init; }

        public abstract TSender CreateProxy(HubConnection connection);

        public abstract IDisposable AddReceiver(HubConnection connection, TReceiver receiver);

        public virtual Task OnClosed(Exception? exception) => Task.CompletedTask;

        public virtual Task OnReconnected(string? connectionId) => Task.CompletedTask;

        public virtual Task OnReconnecting(Exception? exception) => Task.CompletedTask;

        public void Dispose()
        {
            Subscription?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}