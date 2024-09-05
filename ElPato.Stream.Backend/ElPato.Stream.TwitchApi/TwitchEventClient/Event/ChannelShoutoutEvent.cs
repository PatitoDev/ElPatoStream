namespace ElPato.Stream.TwitchApi;

public record ChannelShoutoutEvent
{
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string ModeratorUserId { get; set; }
    public required string ModeratorUserLogin { get; set; }
    public required string ModeratorUserName { get; set; }
    public required string ToBroadcasterUserId { get; set; }
    public required string ToBroadcasterUserLogin { get; set; }
    public required string ToBroadcasterUserName { get; set; }
    public required string StartedAt { get; set; }
    public required int ViewerCount { get; set; }
    public required DateTime CooldownEndsAt { get; set; }
    public required DateTime TargetCooldownEndsAt { get; set; }
}
