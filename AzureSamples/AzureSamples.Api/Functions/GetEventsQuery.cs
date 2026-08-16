using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureSamples.Api.Dtos;
using AzureSamples.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace AzureSamples.Api.Functions
{
    public class GetEventsQuery
    {
        private readonly IEventProvider eventProvider;

        public GetEventsQuery(IEventProvider eventProvider)
        {
            this.eventProvider = eventProvider;
        }

        [OpenApiOperation(operationId: "get-events", tags: ["Events"], Summary = "Get events")]
        [Function(nameof(GetEventsQuery))]
        public async Task<IReadOnlyCollection<EventDto>> HandleAsync([HttpTrigger("get", Route = "events")] FunctionContext context, CancellationToken cancellationToken)
        {
            return (await eventProvider.GetEventsAsync(cancellationToken)).Select(Map).ToList();
        }

        private static EventDto Map(Event @event)
        {
            return new EventDto
            {
                Id = @event.Id,
                UserId = @event.UserId,
                EventType = @event.EventType,
                Details = @event.Details,
                CreatedAtUtc = @event.CreatedAtUtc,
            };
        }
    }
}