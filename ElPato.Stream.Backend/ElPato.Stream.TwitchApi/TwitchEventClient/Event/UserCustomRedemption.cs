namespace ElPato.Stream.TwitchApi;
public record UserCustomRedemption
{
    public required string Id { get; set; }
    public required string BroadcasterUserId { get; set; }
    public required string BroadcasterUserLogin { get; set; }
    public required string BroadcasterUserName { get; set; }
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public required string UserInput { get; set; }
    public required string Status { get; set; }
    public required RedemptionReward Reward { get; set; }
    public required string RedeemedAt { get; set; }
}

public record RedemptionReward
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required int Cost { get; set; }
    public required string Prompt { get; set; }
}
