namespace SignalRSample.Api
{
    public class EventDto
    {
        public required string Identifier { get; init; }

        public required int UnitId { get; init; }

        public required int Number { get; set; }
    }
}