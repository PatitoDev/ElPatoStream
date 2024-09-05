using ElPato.Stream.TwitchApi;
using ElPato.Stream.Api.WebsocketServer;
using ElPato.Stream.BusEvents;

namespace ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;

public class ShoutoutCustomCommand: ICustomCommand
{
    private ITwitchApiClient _apiClient;
    private IServerConnectionManager _connectionManager;

    public ShoutoutCustomCommand(ITwitchApiClient apiClient, IServerConnectionManager serverConnectionManager)
    {
        _apiClient = apiClient;
        _connectionManager = serverConnectionManager;
    }

    public bool AppliesTo(string commandType)
    {
        return ChatCommands.Shoutout.Any(s => string.Equals(commandType, s, StringComparison.OrdinalIgnoreCase));
    }

    public async Task Handle(IEnumerable<string> args, ChatMessageEvent msg, CancellationToken cancellationToken = default)
    {
        if (!(msg.IsMod || msg.IsBroadcaster)) return;

        var userName = args.FirstOrDefault()?.Replace("@", "");
        if (string.IsNullOrWhiteSpace(userName))
        {
            await _apiClient.SendChatMessage("Missing user name param", null, cancellationToken);
            return;
        }

        var user = await _apiClient.GetUserByName(userName);
        if (user == null)
        {
            await _apiClient.SendChatMessage($"Usuario {userName} no existe", null, cancellationToken);
            return;

        }

        var channelData = await _apiClient.GetChannelInformationAsync(user.Id);
        if (channelData == null)
        {
            await _apiClient.SendChatMessage($"Usuario {userName} no existe", null, cancellationToken);
            return;
        }

        var announcementMsg = $"Sigan, persigan y stalkeen a {user.DisplayName} -> https://twitch.tv/{user.Login}";
        if (!(string.IsNullOrWhiteSpace(channelData.Title) && string.IsNullOrEmpty(channelData.GameName)))
        {
            announcementMsg += $" ultimo stream 🎥: { channelData.Title } en { channelData.GameName }";
        }

        await _apiClient.SendChatAnnouncementAsync(announcementMsg, AnnouncementColor.Orange, cancellationToken);

        var imageAsBase64 = await _apiClient.GetImageAsBase64(user.ProfileImageUrl, cancellationToken);
        var busEvent = new ShoutoutEvent()
        {
            Content = new()
            {
                GameName = channelData.GameName,
                ProfileImgAsBase64 = imageAsBase64,
                Tags = channelData.Tags,
                Title = channelData.Title,
                UserId = user.Id,
                UserName = user.DisplayName
            }
        };

        await _connectionManager.Broadcast(busEvent, cancellationToken);
        return;
    }
}
