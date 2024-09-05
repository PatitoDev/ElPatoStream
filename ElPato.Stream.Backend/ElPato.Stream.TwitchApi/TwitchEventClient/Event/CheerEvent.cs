namespace ElPato.Stream.TwitchApi;

public record CheerEvent
{
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string Message { get; set; }
    /**
     * Null if anon
     */
    public required string? UserId { get; set; }
    /**
     * Null if anon
     */
    public required string? UserLogin { get; set; }
    /**
     * Null if anon
     */
    public required string? UserName { get; set; }
    public required int Bits { get; set; }
    public required bool IsAnonymous { get; set; }
}
