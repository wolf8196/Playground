using System.Collections.Generic;
using SignalRSample.Api;

namespace SignalRSample.Server.Services
{
    public interface IEventService
    {
        IReadOnlyCollection<EventDto> GetEvents(int unitId);
    }
}