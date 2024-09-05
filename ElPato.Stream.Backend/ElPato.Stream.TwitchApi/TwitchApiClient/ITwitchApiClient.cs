namespace ElPato.Stream.TwitchApi;

public interface ITwitchApiClient
{
    Task<AuthorizationResponse> Authorize(string code);
    Task<AuthorizationResponse> RefreshToken(string token);
    Task SubscribeToEvent(string sessionId, string type, Dictionary<string, string> condition, int version = 1);
    Task SendChatMessage(string message, string? replyToId = null, CancellationToken cancellationToken = default);
    Task<bool> AddUserVip(string userId, CancellationToken cancellationToken = default);
    Task<bool> RemoveUserVip(string userId, CancellationToken cancellationToken = default);
    Task<bool> UpdateRedemptionStatus(string rewardId, string redeemId, RedemptionStatus status, CancellationToken cancellationToken = default);
    Task<bool> UpdateChannelRewardCost(string rewardId, int cost, CancellationToken cancellationToken = default);
    Task<UserInformation?> GetUserByName(string userName, CancellationToken cancellationToken = default);
    Task<ChannelInformation?> GetChannelInformationAsync(string channelId, CancellationToken cancellationToken = default);
    Task<bool> SendChatAnnouncementAsync(string message, AnnouncementColor color = AnnouncementColor.Primary, CancellationToken cancellationToken = default);
    Task<string> GetImageAsBase64(string imageUrl, CancellationToken cancellation = default);
}

public enum RedemptionStatus
{
    Canceled,
    Fulfilled
}

public enum AnnouncementColor
{
    Blue,
    Green,
    Orange,
    Purple,
    Primary
}
