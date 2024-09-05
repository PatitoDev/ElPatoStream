using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElPato.Stream.TwitchApi;

public class TwitchApiClient : ITwitchApiClient
{
    private readonly TwitchTokenStore _tokenStore;
    private readonly TwitchConfiguration _twitchConfiguration;
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
        }
    };

    public TwitchApiClient(
        TwitchTokenStore tokenStore, 
        TwitchConfiguration twitchConfiguration,
        HttpClient httpClient)
    {
        _tokenStore = tokenStore;
        _twitchConfiguration = twitchConfiguration;
        _httpClient = httpClient;
    }

    public async Task<AuthorizationResponse> Authorize(string code)
    {
        var uri = "https://id.twitch.tv/oauth2/token";
        uri += $"?client_secret={_twitchConfiguration.ClientSecret}";
        uri += $"&client_id={_twitchConfiguration.ClientId}";
        uri += $"&code={code}";
        uri += $"&redirect_uri=http://localhost:5131/Authenticate";
        uri += $"&grant_type=authorization_code";

        var res = await _httpClient.PostAsync(uri, null);
        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine(res.StatusCode);
            Console.WriteLine(await res.Content.ReadAsStringAsync());
            throw new Exception("Unable to authenticate");
        }

        var data = await res.Content.ReadFromJsonAsync<AuthorizationResponse>(_options);

        return data == null ? throw new Exception("Failed authorization with twitch api") : data;
    }

    public async Task<AuthorizationResponse> RefreshToken(string token)
    {
        var uri = "https://id.twitch.tv/oauth2/token";
        uri += $"?client_secret={_twitchConfiguration.ClientSecret}";
        uri += $"&client_id={_twitchConfiguration.ClientId}";
        uri += "&grant_type=refresh_token";
        uri += $"&refresh_token={token}";
        var res = await _httpClient.PostAsync(uri, null);
        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine(res.StatusCode);
            Console.WriteLine(await res.Content.ReadAsStringAsync());
            throw new Exception("Unable to refresh token");
        }
        var data = await res.Content.ReadFromJsonAsync<AuthorizationResponse>(_options);

        return data == null ? throw new Exception("Failed authorization with twitch api") : data;
    }

    public async Task SubscribeToEvent(string sessionId, string type, Dictionary<string, string> condition, int version = 1)
    {
        var uri = "https://api.twitch.tv/helix/eventsub/subscriptions";

        var requestMsg = new HttpRequestMessage();
        AddAuthenticationHeaders(requestMsg);
        requestMsg.Method = HttpMethod.Post;
        requestMsg.RequestUri = new Uri(uri);

        var payload = new
        {
            type,
            version,
            condition,
            Transport = new
            {
                Method = "websocket",
                sessionId
            }
        };

        var bodyContent = new StringContent(JsonSerializer.Serialize(payload, _options), Encoding.UTF8, "application/json");
        requestMsg.Content = bodyContent;
        var resp = await _httpClient.SendAsync(requestMsg);
        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception($"Error subscribing to event {type}");
        }
    }

    public async Task SendChatMessage(string message, string? replyToId = null, CancellationToken cancellationToken = default)
    {
        var uri = "https://api.twitch.tv/helix/chat/messages";
        var request = new HttpRequestMessage();
        AddAuthenticationHeaders(request);
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(uri);

        var payload = new
        {
            BroadcasterId = _twitchConfiguration.UserId,
            SenderId = _twitchConfiguration.UserId,
            Message = message,
            ReplyParentMessageId = replyToId
        };

        var bodyContent = new StringContent(JsonSerializer.Serialize(payload, _options), Encoding.UTF8, "application/json");
        request.Content = bodyContent;
        var resp = await _httpClient.SendAsync(request);
        if (!resp.IsSuccessStatusCode) {
            throw new Exception("Unable to send message");
        }
    }

    public async Task<bool> AddUserVip(string userId, CancellationToken cancellationToken = default)
    {
        var url = $"https://api.twitch.tv/helix/channels/vips?broadcaster_id={_twitchConfiguration.UserId}&user_id={userId}";

        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(url);
        AddAuthenticationHeaders(request);

        var resp = await _httpClient.SendAsync(request, cancellationToken);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveUserVip(string userId, CancellationToken cancellationToken = default)
    {
        var url = $"https://api.twitch.tv/helix/channels/vips?broadcaster_id={_twitchConfiguration.UserId}&user_id={userId}";

        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Delete;
        request.RequestUri = new Uri(url);
        AddAuthenticationHeaders(request);

        var resp = await _httpClient.SendAsync(request, cancellationToken);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateRedemptionStatus(string rewardId, string redeemId, RedemptionStatus status, CancellationToken cancellationToken = default)
    {
        var uri = $"https://api.twitch.tv/helix/channel_points/custom_rewards/redemptions?broadcaster_id={_twitchConfiguration.UserId}&reward_id={rewardId}&id={redeemId}";
        var request = new HttpRequestMessage();
        AddAuthenticationHeaders(request);
        request.Method = HttpMethod.Patch;
        request.RequestUri = new Uri(uri);

        var payload = new
        {
            Status = status.ToString().ToUpper()
        };

        var bodyContent = new StringContent(JsonSerializer.Serialize(payload, _options), Encoding.UTF8, "application/json");
        request.Content = bodyContent;
        var resp = await _httpClient.SendAsync(request);

        if (!resp.IsSuccessStatusCode)
        {
            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateChannelRewardCost(string rewardId, int cost, CancellationToken cancellationToken = default)
    {
        var uri = $"https://api.twitch.tv/helix/channel_points/custom_rewards?broadcaster_id={_twitchConfiguration.UserId}&id={rewardId}";
        var request = new HttpRequestMessage();
        AddAuthenticationHeaders(request);
        request.Method = HttpMethod.Patch;
        request.RequestUri = new Uri(uri);

        var payload = new
        {
            cost
        };

        var bodyContent = new StringContent(JsonSerializer.Serialize(payload, _options), Encoding.UTF8, "application/json");
        request.Content = bodyContent;
        var resp = await _httpClient.SendAsync(request);
        return resp.IsSuccessStatusCode;
    }

    public async Task<ChannelInformation?> GetChannelInformationAsync(string channelId, CancellationToken cancellationToken = default)
    {
        var uri = $"https://api.twitch.tv/helix/channels?broadcaster_id={channelId}";
        var request = new HttpRequestMessage();
        AddAuthenticationHeaders(request);
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri(uri);
        var resp = await _httpClient.SendAsync(request);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception($"Unable to gather channel information for user {channelId}");
        }

        var channelInformationResponseData = await resp.Content
            .ReadFromJsonAsync<PaginationResponse<ChannelInformation>>(_options, cancellationToken);
        return channelInformationResponseData?.Data.FirstOrDefault();
    }

    public async Task<bool> SendChatAnnouncementAsync(string message, AnnouncementColor color = AnnouncementColor.Primary, CancellationToken cancellationToken = default)
    {
        var uri = $"https://api.twitch.tv/helix/chat/announcements?broadcaster_id={_twitchConfiguration.UserId}&moderator_id={_twitchConfiguration.UserId}";
        var request = new HttpRequestMessage();
        AddAuthenticationHeaders(request);
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(uri);

        var payload = new
        {
            message, 
            color
        };
        Console.WriteLine(payload);

        var bodyContent = new StringContent(JsonSerializer.Serialize(payload, _options), Encoding.UTF8, "application/json");
        request.Content = bodyContent;
        var resp = await _httpClient.SendAsync(request, cancellationToken);
        return resp.IsSuccessStatusCode;
    }

    public async Task<UserInformation?> GetUserByName(string userName, CancellationToken cancellationToken = default)
    {
        var uri = $"https://api.twitch.tv/helix/users?login={userName.ToLower()}";
        var request = new HttpRequestMessage();
        AddAuthenticationHeaders(request);
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri(uri);

        var resp = await _httpClient.SendAsync(request, cancellationToken);


        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch user information for {userName}");
        }

        var data = await resp.Content.ReadFromJsonAsync<PaginationResponse<UserInformation>>(_options, cancellationToken);
        return data?.Data.FirstOrDefault();
    }

    public async Task<string> GetImageAsBase64(string imageUrl, CancellationToken cancellationToken = default)
    {
        var resp = await _httpClient.GetAsync(imageUrl, cancellationToken);
        var byteArray = await resp.Content.ReadAsByteArrayAsync();
        var contentType = resp.Content.Headers.ContentType;
        return $"{contentType};base64," + Convert.ToBase64String(byteArray);
    }

    private void AddAuthenticationHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("Client-Id", _twitchConfiguration.ClientId);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
    }
}
