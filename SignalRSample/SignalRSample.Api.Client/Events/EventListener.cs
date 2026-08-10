using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRSample.HubApi;
using TypedSignalR.Client;

namespace SignalRSample.Api.Client.Events
{
    public abstract class EventListener : HubListener, IEventReceiver
    {
        protected EventListener(HubConnection connection)
            : base(connection)
        {
        }

        public abstract Task EventAdded(EventDto evt);

        public abstract Task EventRemoved(EventDto evt);

        public abstract Task EventUpdated(EventDto evt);

        public abstract Task SnapshotSent(int unitId, IReadOnlyCollection<EventDto> events);

        public override IDisposable AddReceiver(HubConnection connection)
        {
            return connection.Register<IEventReceiver>(this);
        }
    }
}