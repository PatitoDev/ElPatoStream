namespace ElPato.Stream.TwitchApi;

public record ChannelRaidEvent
{
    public required string FromBroadcasterUserId { get; set; }
    public required string FromBroadcasterUserLogin { get; set; }
    public required string FromBroadcasterUserName { get; set; }
    public required string ToBroadcasterUserId { get; set; }
    public required string ToBroadcasterUserLogin { get; set; }
    public required string ToBroadcasterUserName { get; set; }
    public required int Viewers { get; set; }
}
