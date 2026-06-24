using System.Threading.Tasks;

namespace SignalRSample.Api
{
    public interface IEventSender
    {
        Task Subscribe(EventSubscriptionDto subscription);
    }

    public interface IEventReceiver
    {
        Task EventAdded(EventDto evt);

        Task EventRemoved(EventDto evt);

        Task EventUpdated(EventDto evt);
    }
}