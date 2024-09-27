namespace ElPato.Stream.TwitchApi;

public record ChannelPredictionEndEvent
{
    public required string Id { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string Title { get; set; }
    public required string WinningOutcomeId { get; set; }
    public required IEnumerable<ChannelPredictionOutcome> Outcomes { get; set; }
    public required PredictionStatus Status { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime EndedAt { get; set; }
}

public enum PredictionStatus
{
    Resolved,
    Canceled
}

public record ChannelPredictionProgressEvent
{
    public required string Id { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<ChannelPredictionOutcome> Outcomes { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime LocksAt { get; set; }
}

public record ChannelPredictionLockEvent
{
    public required string Id { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<ChannelPredictionOutcome> Outcomes { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime LockedAt { get; set; }
}


public enum PredictionColor
{
    Blue,
    Pink
}

public record ChannelPredictionOutcome
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required PredictionColor Color { get; set; }
    public required int Users { get; set; }
    public required int ChannelPoints { get; set; }
    /**
     * Up to 10
     */
    public required IEnumerable<ChannelPredictionUser> TopPredictors { get; set; }
}

public record ChannelPredictionUser
{
    public required string UserName { get; set; }
    public required string UserLogin { get; set; }
    public required string UserId { get; set; }
    public required int? ChannelPointsWon { get; set; } // null if refund or loss
    public required int ChannelPointsUsed { get; set; }
}
