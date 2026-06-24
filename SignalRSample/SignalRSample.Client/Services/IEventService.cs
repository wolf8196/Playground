using System.Threading.Tasks;
using SignalRSample.Api;

namespace SignalRSample.Client.Services
{
    internal interface IEventService
    {
        Task Subscribe();

        void AddEvent(EventDto evt);

        void RemoveEvent(EventDto evt);

        void UpdateEvent(EventDto evt);
    }
}