using System;

namespace AzureSamples.Core
{
    public class Event
    {
        public required int Id { get; init; }

        public required int UserId { get; init; }

        public required EventType EventType { get; init; }

        public required string? Details { get; init; }

        public required DateTime CreatedAtUtc { get; init; }
    }
}