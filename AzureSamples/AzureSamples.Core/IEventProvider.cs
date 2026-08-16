using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AzureSamples.Core
{
    public interface IEventProvider
    {
        Task<IReadOnlyCollection<Event>> GetEventsAsync(CancellationToken token);

        Task CreateEventAsync(Event @event, CancellationToken token);
    }
}