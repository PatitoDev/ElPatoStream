
namespace ElPato.Stream.TwitchApi;

public static class EventSubscriptions
{
    public const string ChannelUpdate = "channel.update";
    public const string ChannelRewardAdd = "channel.channel_points_custom_reward_redemption.add";
    public const string ChannelFollow = "channel.follow";
    public const string ChannelSubscribe = "channel.subscribe";
    public const string ChannelSubscribeGift = "channel.subscription.gift";
    public const string ChannelSubscribeMessage = "channel.subscription.message";
    public const string ChannelCheer = "channel.cheer";
    public const string ChannelRaid = "channel.raid";
    public const string ChannelShoutout = "channel.shoutout.create";
    public const string ChannelChatMessage = "channel.chat.message";

    public const string ChannelPredictionBegin = "channel.prediction.begin";
    public const string ChannelPredictionEnd = "channel.prediction.end";
    public const string ChannelPredictionProgress = "channel.prediction.progress";


    public static IEnumerable<EventSubscription> GetSubscriptionList(string ChannelId) =>
        new List<EventSubscription>() {
            new(ChannelUpdate, new() { { "broadcaster_user_id", ChannelId } }),

            new(ChannelRewardAdd, new() { { "broadcaster_user_id", ChannelId } }),

            new (ChannelFollow, new (){ { "broadcaster_user_id", ChannelId }, { "moderator_user_id", ChannelId } }, 2),

            new (ChannelSubscribe, new(){ { "broadcaster_user_id", ChannelId } }),

            new (ChannelSubscribeGift, new() { { "broadcaster_user_id", ChannelId } }),

            new (ChannelSubscribeMessage, new() { { "broadcaster_user_id", ChannelId } }),

            new (ChannelCheer, new() { { "broadcaster_user_id", ChannelId } }),

            new (ChannelRaid, new() { { "to_broadcaster_user_id", ChannelId } }),

            new (ChannelShoutout, new() { { "broadcaster_user_id", ChannelId } , { "moderator_user_id", ChannelId } }),

            new (ChannelChatMessage, new() { { "broadcaster_user_id", ChannelId }, { "user_id", ChannelId } }),

            new (ChannelPredictionBegin, new() { { "broadcaster_user_id", ChannelId } }),

            new (ChannelPredictionEnd, new() { { "broadcaster_user_id", ChannelId } }),

            new (ChannelPredictionProgress, new() { { "broadcaster_user_id", ChannelId } }),
    };
}
