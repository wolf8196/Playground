using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using AzureSamples.Core;
using AzureSamples.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace AzureSamples.Infrastructure.Providers
{
    internal class EventProvider : IEventProvider
    {
        private readonly AzureSamplesDbContext context;

        public EventProvider(AzureSamplesDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<Event>> GetEventsAsync(CancellationToken token)
        {
            return await context.Events
                .Select(entity => new Event
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    EventType = entity.EventType,
                    Details = entity.Details,
                    CreatedAtUtc = entity.CreatedAtUtc,
                })
                .ToListAsync(token);
        }

        public async Task CreateEventAsync(Event @event, CancellationToken token)
        {
            context.Events.Add(new EventEntity
            {
                UserId = @event.UserId,
                EventType = @event.EventType,
                Details = @event.Details,
                CreatedAtUtc = @event.CreatedAtUtc
            });

            await context.SaveChangesAsync(token);
        }
    }
}