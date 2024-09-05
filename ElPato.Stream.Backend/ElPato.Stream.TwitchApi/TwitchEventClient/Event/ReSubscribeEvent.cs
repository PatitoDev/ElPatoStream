namespace ElPato.Stream.TwitchApi;

public record ReSubscribeEvent
{
    public required string UserId { get; set; }
    public required string UserLogin { get; set; }
    public required string UserName { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string Tier { get; set; }
    public required int CumulativeMonths { get; set; }
    public required int? StreakMonths { get; set; }
    public required int DurationMonths { get; set; }
    public required ReSubscribeEventMessage Message { get; set; }
}

public record ReSubscribeEventMessage
{
    public required string Text { get; set; }
    public required List<ReSubscribeEventMessageEmote> Emotes { get; set; }
}

public record ReSubscribeEventMessageEmote
{
    public required string Id { get; set; }
    public required int Begin { get; set; }
    public required int End { get; set; }
}
