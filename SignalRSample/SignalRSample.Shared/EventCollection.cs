using System.Collections.Concurrent;
using SignalRSample.Api;

namespace SignalRSample.Shared
{
    public class EventCollection
    {
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, EventDto>> units;

        public EventCollection()
        {
            units = new ConcurrentDictionary<int, ConcurrentDictionary<string, EventDto>>();
        }

        public void AddEvent(EventDto evt)
        {
            units.AddOrUpdate(
                evt.UnitId,
                key => new ConcurrentDictionary<string, EventDto>(),
                (key, existing) =>
                {
                    existing.TryAdd(evt.Identifier, evt);
                    return existing;
                });
        }

        public void RemoveEvent(EventDto evt)
        {
            units.AddOrUpdate(
                evt.UnitId,
                key => new ConcurrentDictionary<string, EventDto>(),
                (key, existing) =>
                {
                    existing.TryRemove(evt.Identifier, out _);
                    return existing;
                });
        }

        public void UpdateEvent(EventDto evt)
        {
            units.AddOrUpdate(
                evt.UnitId,
                key => new ConcurrentDictionary<string, EventDto>(),
                (key, existing) =>
                {
                    existing.AddOrUpdate(evt.Identifier,
                        key => evt,
                        (key, existing) => evt);
                    return existing;
                });
        }

        public void SetEvents(int unitId, IReadOnlyCollection<EventDto> events)
        {
            units.AddOrUpdate(unitId,
                key => new ConcurrentDictionary<string, EventDto>(events.ToDictionary(x => x.Identifier)),
                (key, existing) => new ConcurrentDictionary<string, EventDto>(events.ToDictionary(x => x.Identifier)));
        }

        public IReadOnlyCollection<EventDto> GetEvents()
        {
            return units.SelectMany(x => x.Value.Values).ToList();
        }

        public IReadOnlyCollection<EventDto> GetEvents(int unitId)
        {
            return units.GetValueOrDefault(unitId)?.Values?.ToList() ?? [];
        }
    }
}