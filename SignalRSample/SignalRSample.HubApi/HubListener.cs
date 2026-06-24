using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample.HubApi
{
    public abstract class HubListener : IDisposable
    {
        public HubListener(HubConnection connection)
        {
            Connection = connection;
            Subscription = AddReceiver(connection);

            connection.Closed += OnClosed;
            connection.Reconnecting += OnReconnecting;
            connection.Reconnected += OnReconnected;
        }

        protected HubConnection Connection { get; }

        protected IDisposable Subscription { get; }

        public abstract IDisposable AddReceiver(HubConnection connection);

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