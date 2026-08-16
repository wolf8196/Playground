using System.Threading;
using System.Threading.Tasks;
using AzureSamples.Api.Dtos;
using AzureSamples.Core;
using Microsoft.Azure.Functions.Worker;

namespace AzureSamples.Api.Functions
{
    public class CreateEventCommand
    {
        private readonly IEventProvider eventProvider;

        public CreateEventCommand(IEventProvider eventProvider)
        {
            this.eventProvider = eventProvider;
        }

        [Function(nameof(CreateEventCommand))]
        public async Task HandleAsync(
            [ServiceBusTrigger("%EventsQueue%", Connection = "ServiceBusConnectionString", AutoCompleteMessages = false)]
            EventDto dto,
            CancellationToken cancellationToken)
        {
            await eventProvider.CreateEventAsync(new Event
            {
                Id = dto.Id,
                UserId = dto.UserId,
                EventType = dto.EventType,
                Details = dto.Details,
                CreatedAtUtc = dto.CreatedAtUtc
            },
            cancellationToken);
        }
    }
}