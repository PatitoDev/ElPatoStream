using ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;
using ElPato.Stream.Api.TwitchEventHandler.CustomCommandResolver;
using ElPato.Stream.Api.WebsocketServer;
using ElPato.Stream.BusEvents;
using ElPato.Stream.TwitchApi;
using Bus = ElPato.Stream.BusEvents;

namespace ElPato.Stream.Api.TwitchEventHandler;

public class TwitchEventHandler: ITwitchEventHandler
{
    private ITwitchApiClient _apiClient;
    private IServerConnectionManager _serverConnectionManager;
    private ICustomCommandResolver _customCommandResolver;

    public TwitchEventHandler(
        TwitchEventClient eventClient,
        ITwitchApiClient apiClient,
        IServerConnectionManager serverConnectionManager,
        ICustomCommandResolver customCommandResolver)
    {
        eventClient.TwitchChatMessageEvent += Client_TwitchChatMessageEvent;
        eventClient.TwitchEvent += EventClient_TwitchEvent;
        eventClient.UserCustomRedemption += EventClient_UserCustomRedemption;
        eventClient.CheerEvent += EventClient_CheerEvent;

        eventClient.FollowEvent += EventClient_FollowEvent;
        eventClient.ChannelRaidEvent += EventClient_ChannelRaidEvent;
        eventClient.ChannelSubscribeEvent += EventClient_ChannelSubscribeEvent;
        eventClient.GiftChannelSubscribeEvent += EventClient_GiftChannelSubscribeEvent;
        eventClient.ReSubscribeEvent += EventClient_ReSubscribeEvent;
        eventClient.ChannelShoutoutEvent += EventClient_ChannelShoutoutEvent;

        _apiClient = apiClient;
        _serverConnectionManager = serverConnectionManager;
        _customCommandResolver = customCommandResolver;
    }

    private async void EventClient_ChannelShoutoutEvent(object? sender, TwitchEventArgs<ChannelShoutoutEvent> e)
    {
        var channelId = e.Payload.ToBroadcasterUserId;
        var channelInformation = await _apiClient.GetChannelInformationAsync(channelId);
        var userInformation = await _apiClient.GetUserByName(e.Payload.ToBroadcasterUserName);

        if (userInformation == null || channelInformation == null) return;

        var imageAsBase64 = await _apiClient.GetImageAsBase64(userInformation.ProfileImageUrl);

        var shoutoutEvent = new ShoutoutEvent()
        {
            Content = new()
            {
                GameName = channelInformation.GameName,
                ProfileImgAsBase64 = imageAsBase64,
                Tags = channelInformation.Tags,
                Title = channelInformation.Title,
                UserId = userInformation.Id,
                UserName = userInformation.DisplayName,
            }
        };
        await _serverConnectionManager.Broadcast(shoutoutEvent);
    }

    private async void EventClient_ReSubscribeEvent(object? sender, TwitchEventArgs<ReSubscribeEvent> e)
    {
        await _serverConnectionManager.Broadcast<Bus.ReSubEvent>(new ()
        {
            Content = new() {
                Duration = e.Payload.DurationMonths,
                Message = e.Payload.Message.Text,
                Streak = e.Payload.StreakMonths,
                TotalMonths = e.Payload.CumulativeMonths,
                UserLogin = e.Payload.UserLogin,
                Tier = e.Payload.Tier,
                UserId = e.Payload.UserId,
                UserName = e.Payload.UserName
            }
        });
    }

    private async void EventClient_GiftChannelSubscribeEvent(object? sender, TwitchEventArgs<ChannelSubscribeGiftEvent> e)
    {
        await _serverConnectionManager.Broadcast<Bus.SubGiftEvent>(new ()
        {
            Content = new() {
                CumulativeTotal = e.Payload.CumulativeTotal,
                IsAnon = e.Payload.IsAnonymous,
                Total = e.Payload.Total,
                UserLogin = e.Payload.UserLogin,
                Tier = e.Payload.Tier,
                UserId = e.Payload.UserId,
                UserName = e.Payload.UserName
            }
        });
    }

    private async void EventClient_ChannelSubscribeEvent(object? sender, TwitchEventArgs<ChannelSubscribeEvent> e)
    {
        await _serverConnectionManager.Broadcast<Bus.NewSubscriberEvent>(new ()
        {
            Content = new() {
                IsGift = e.Payload.IsGift,
                Tier = e.Payload.Tier,
                UserId = e.Payload.UserId,
                UserName = e.Payload.UserName
            }
        });
    }

    private async void EventClient_ChannelRaidEvent(object? sender, TwitchEventArgs<ChannelRaidEvent> e)
    {
        await _serverConnectionManager.Broadcast<Bus.RaidEvent>(new ()
        {
            Content = new() {
                Id = e.Payload.FromBroadcasterUserId,
                Login = e.Payload.FromBroadcasterUserLogin,
                Name = e.Payload.FromBroadcasterUserName,
                Viewers = e.Payload.Viewers,
            }
        });
    }

    private async void EventClient_FollowEvent(object? sender, TwitchEventArgs<TwitchApi.FollowEvent> e)
    {
        await _serverConnectionManager.Broadcast<Bus.FollowEvent>(new ()
        {
            Content = new() {
                Id = e.Payload.UserId,
                Login = e.Payload.UserLogin,
                Name = e.Payload.UserName
            }
        });
    }

    private async void EventClient_CheerEvent(object? sender, TwitchEventArgs<CheerEvent> e)
    {

        var busEvent = new BitEvent()
        {
            Content = new()
            {
                Bits = e.Payload.Bits,
                IsAnonymous = e.Payload.IsAnonymous,
                Message = e.Payload.Message,
                UserId = e.Payload.UserId,
                UserLogin = e.Payload.UserLogin,
                UserName = e.Payload.UserName,
            }
        };
        await _serverConnectionManager.Broadcast(busEvent);
    }

    private async void EventClient_UserCustomRedemption(object? sender, TwitchEventArgs<UserCustomRedemption> e)
    {
        if (e.Payload.Reward.Id == ChannelRewards.Vip)
        {
            var vipSuccess = await _apiClient.AddUserVip(e.Payload.UserId);

            if (!vipSuccess)
            {
                await _apiClient.UpdateRedemptionStatus(e.Payload.Reward.Id, e.Payload.Id, RedemptionStatus.Canceled);
                return;
            }

            await _apiClient.UpdateRedemptionStatus(e.Payload.Reward.Id, e.Payload.Id, RedemptionStatus.Fulfilled);
            var newCost = e.Payload.Reward.Cost + ChannelRewards.VipIncrementCost;
            await _apiClient.UpdateChannelRewardCost(e.Payload.Reward.Id, newCost);

            var vipEvent = new VipRedeemEvent()
            {
                Content = new() {
                    IncreasedTo = newCost,
                    UserId = e.Payload.UserId,
                    UserName = e.Payload.UserName,
                }
            };
            await _serverConnectionManager.Broadcast(vipEvent);
        }
    }

    private void EventClient_TwitchEvent(object? sender, TwitchEventArgs<System.Text.Json.Nodes.JsonNode> e)
    {
        return;
    }

    private async void Client_TwitchChatMessageEvent(object? sender, TwitchEventArgs<ChatMessageEvent> e)
    {
        var parts = e.Payload.Message.Text.Split(" ");
        if (parts.Length == 0)
            return;

        var commandPart = parts[0];
        var isCommand = commandPart.StartsWith("!");

        if (!isCommand)
            return;

        var command = commandPart.Replace("!", "");
        var result = ChatCommands
            .Commands
            .FirstOrDefault(c => c.Commands.Any(cc => cc.Equals(command, StringComparison.CurrentCultureIgnoreCase)))
            ?.Result;

        if (result == null)
        {
            var matchedHandler = _customCommandResolver.GetCommand(command);
            if (matchedHandler != null)
            {
                await matchedHandler.Handle(parts.Skip(1), e.Payload);
            }
            return;
        }

        await _apiClient.SendChatMessage(result);
    }
}
