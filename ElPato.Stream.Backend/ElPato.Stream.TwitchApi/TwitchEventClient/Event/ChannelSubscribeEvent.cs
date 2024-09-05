namespace ElPato.Stream.TwitchApi;

public record ChannelSubscribeEvent
{
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string UserId { get; set; }
    public required string UserLogin { get; set; }
    public required string UserName { get; set; }
    public required string Tier { get; set; }
    public required bool IsGift { get; set; }
}
