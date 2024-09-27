using ElPato.Stream.BusEvents.Base;

namespace ElPato.Stream.BusEvents;

public record PredictionEndEvent : BusEvent<PredictionEndEventContent>
{
    public PredictionEndEvent() : base("prediction-end") { }
}

public record PredictionEndEventContent
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string WinningOutcomeId { get; set; }
    public required IEnumerable<PredictionOutcome> Outcomes { get; set; }
    public required PredictionStatus Status { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime EndedAt { get; set; }
}

public enum PredictionStatus
{
    Resolved,
    Canceled
}

public record PredictionProgressEvent : BusEvent<PredictionProgressEventContent>
{
    public PredictionProgressEvent() : base("prediction-progress") { }
}

public record PredictionProgressEventContent
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<PredictionOutcome> Outcomes { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime LocksAt { get; set; }
}

public record PredictionOutcome
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required PredictionColor Color { get; set; }
    public required int Users { get; set; }
    public required int ChannelPoints { get; set; }
    /**
     * Up to 10
     */
    public required IEnumerable<PredictionUser> TopPredictors { get; set; }
}

public record PredictionUser
{
    public required string UserName { get; set; }
    public required string UserLogin { get; set; }
    public required string UserId { get; set; }
    public required int? ChannelPointsWon { get; set; }
    public required int ChannelPointsUsed { get; set; }
}

public record PredictionLockEvent : BusEvent<PredictionLockEventContent>
{
    public PredictionLockEvent() : base("prediction-lock") { }
}

public record PredictionLockEventContent
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<PredictionOutcome> Outcomes { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime LockedAt { get; set; }
}
