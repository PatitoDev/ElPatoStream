using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record PredictionBeginEvent : BusEvent<PredictionBeginEventContent>
{
    public PredictionBeginEvent() : base("prediction-begin") { }
}

public record PredictionBeginOutcome
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required PredictionColor Color { get; set; }
}

public record PredictionBeginEventContent
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<PredictionBeginOutcome> Outcomes { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime LocksAt { get; set; }
}
