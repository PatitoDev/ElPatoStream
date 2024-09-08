namespace ElPato.Stream.TwitchApi;

public record ChannelPredictionBeginOutcome
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required PredictionColor Color { get; set; }
}

public record ChannelPredictionBeginEvent
{
    public required string Id { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string Title { get; set; }
    public required IEnumerable<ChannelPredictionBeginOutcome> Outcomes { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime LocksAt { get; set; }
}
