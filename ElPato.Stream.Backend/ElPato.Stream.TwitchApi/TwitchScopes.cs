namespace ElPato.Stream.TwitchApi;

public static class TwitchScopes
{
    public static readonly string[] Scopes = [
        "user:write:chat",
        "user:read:chat",
        "chat:read",
        "chat:edit",
        "channel:manage:redemptions",
        "channel:manage:vips",
        "moderator:manage:banned_users",
        "moderator:manage:announcements",
        "moderator:read:followers",
        "channel:read:subscriptions",
        "bits:read",
        "channel:read:redemptions",
        "channel:manage:redemptions",
        "moderator:manage:shoutouts",
    ];
}
