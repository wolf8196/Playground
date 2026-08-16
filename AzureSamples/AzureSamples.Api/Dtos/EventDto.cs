using System;
using AzureSamples.Core;

namespace AzureSamples.Api.Dtos
{
    public class EventDto
    {
        public required int Id { get; init; }

        public required int UserId { get; init; }

        public required EventType EventType { get; init; }

        public string? Details { get; init; }

        public required DateTime CreatedAtUtc { get; init; }
    }
}
