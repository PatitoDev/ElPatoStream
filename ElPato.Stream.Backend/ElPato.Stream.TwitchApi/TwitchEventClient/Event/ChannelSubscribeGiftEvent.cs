namespace ElPato.Stream.TwitchApi;

public record ChannelSubscribeGiftEvent
{
    public required string? UserId { get; set; }
    public required string? UserLogin { get; set; }
    public required string? UserName { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required int Total { get; set; }
    public required string Tier { get; set; }
    public required int? CumulativeTotal { get; set; }
    public required bool IsAnonymous { get; set; }
}
