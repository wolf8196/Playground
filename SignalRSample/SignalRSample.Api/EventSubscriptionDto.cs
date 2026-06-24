using System.Collections.Generic;

namespace SignalRSample.Api
{
    public class EventSubscriptionDto
    {
        public IReadOnlyCollection<int> UnitIds { get; init; } = [];
    }
}