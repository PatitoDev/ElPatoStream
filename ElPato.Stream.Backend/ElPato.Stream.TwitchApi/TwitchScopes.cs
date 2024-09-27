namespace ElPato.Stream.TwitchApi;

public static class TwitchScopes
{
    public static readonly string[] Scopes = [
        "bits:read",

        "user:write:chat",
        "user:read:chat",

        "chat:read",
        "chat:edit",

        "moderator:read:followers",
        "moderator:manage:banned_users",
        "moderator:manage:announcements",
        "moderator:manage:shoutouts",

        "channel:read:subscriptions",
        "channel:read:redemptions",
        "channel:manage:redemptions",
        "channel:manage:predictions",
        "channel:manage:redemptions",
        "channel:manage:vips",
    ];
}
