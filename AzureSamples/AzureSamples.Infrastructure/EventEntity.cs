using System;
using AzureSamples.Core;

namespace AzureSamples.Infrastructure
{
    public class EventEntity
    {
        public int Id { get; init; }

        public int UserId { get; init; }

        public EventType EventType { get; init; }

        public string? Details { get; init; }

        public DateTime CreatedAtUtc { get; init; }
    }
}