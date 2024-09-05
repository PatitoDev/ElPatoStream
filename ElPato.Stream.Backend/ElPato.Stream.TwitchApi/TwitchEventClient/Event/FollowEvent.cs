namespace ElPato.Stream.TwitchApi;

public record FollowEvent
{
    public required string UserId { get; set; }
    public required string UserLogin { get; set; }
    public required string UserName { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string FollowedAt { get; set; }
}
