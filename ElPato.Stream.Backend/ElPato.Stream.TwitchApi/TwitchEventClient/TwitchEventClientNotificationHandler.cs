using System.Text.Json;
using System.Text.Json.Nodes;

namespace ElPato.Stream.TwitchApi;

public partial class TwitchEventClient
{
    public event EventHandler<TwitchEventArgs<JsonNode>>? TwitchEvent;

    public event EventHandler<TwitchEventArgs<ChatMessageEvent>>? TwitchChatMessageEvent;
    public event EventHandler<TwitchEventArgs<UserCustomRedemption>>? UserCustomRedemption;
    public event EventHandler<TwitchEventArgs<CheerEvent>>? CheerEvent;

    public event EventHandler<TwitchEventArgs<ChannelSubscribeEvent>>? ChannelSubscribeEvent;
    public event EventHandler<TwitchEventArgs<ReSubscribeEvent>>? ReSubscribeEvent;
    public event EventHandler<TwitchEventArgs<FollowEvent>>? FollowEvent;
    public event EventHandler<TwitchEventArgs<ChannelRaidEvent>>? ChannelRaidEvent;
    public event EventHandler<TwitchEventArgs<ChannelSubscribeGiftEvent>>? GiftChannelSubscribeEvent;
    public event EventHandler<TwitchEventArgs<ChannelShoutoutEvent>>? ChannelShoutoutEvent;

    private async Task HandleNotificationPayload(JsonNode payload, CancellationToken cancellationToken)
    {
        var metadata = payload["subscription"]!.Deserialize<TwitchSubscriptionEventMetadata>(_jsonOptions);
        var type = metadata!.Type;
        var eventPayload = payload["event"];

        if (type != EventSubscriptions.ChannelChatMessage)
        {
            Console.WriteLine(type);
            Console.WriteLine(eventPayload);
        }

        TwitchEvent?.Invoke(this, new TwitchEventArgs<JsonNode>() { 
            Payload = eventPayload!
        });

        switch (type)
        {
            case EventSubscriptions.ChannelFollow:
                FollowEvent?.Invoke(this, 
                    new TwitchEventArgs<FollowEvent> { 
                        Payload = eventPayload.Deserialize<FollowEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelChatMessage:
                TwitchChatMessageEvent?.Invoke(this, 
                    new TwitchEventArgs<ChatMessageEvent> { 
                        Payload = eventPayload.Deserialize<ChatMessageEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelShoutout:
                ChannelShoutoutEvent?.Invoke(this, 
                    new TwitchEventArgs<ChannelShoutoutEvent> { 
                        Payload = eventPayload.Deserialize<ChannelShoutoutEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelRewardAdd:
                UserCustomRedemption?.Invoke(this, 
                    new TwitchEventArgs<UserCustomRedemption> { 
                        Payload = eventPayload.Deserialize<UserCustomRedemption>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelSubscribe:
                ChannelSubscribeEvent?.Invoke(this, 
                    new TwitchEventArgs<ChannelSubscribeEvent> { 
                        Payload = eventPayload.Deserialize<ChannelSubscribeEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelSubscribeGift:
                GiftChannelSubscribeEvent?.Invoke(this, 
                    new TwitchEventArgs<ChannelSubscribeGiftEvent> { 
                        Payload = eventPayload.Deserialize<ChannelSubscribeGiftEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelSubscribeMessage:
                ReSubscribeEvent?.Invoke(this, 
                    new TwitchEventArgs<ReSubscribeEvent> { 
                        Payload = eventPayload.Deserialize<ReSubscribeEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelCheer:
                CheerEvent?.Invoke(this, 
                    new TwitchEventArgs<CheerEvent> { 
                        Payload = eventPayload.Deserialize<CheerEvent>(_jsonOptions)!
                    }
                );
                return;
            case EventSubscriptions.ChannelRaid:
                ChannelRaidEvent?.Invoke(this, 
                    new TwitchEventArgs<ChannelRaidEvent> { 
                        Payload = eventPayload.Deserialize<ChannelRaidEvent>(_jsonOptions)!
                    }
                );
                return;
        }
    }
}
