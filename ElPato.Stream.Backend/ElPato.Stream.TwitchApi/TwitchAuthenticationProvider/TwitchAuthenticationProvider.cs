using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ElPato.Stream.TwitchApi;

public class TwitchAuthenticationProvider: ITwitchAuthenticationProvider
{
    private Timer? _refreshTimer;
    private readonly TwitchTokenStore _tokenStore;
    private readonly ITwitchApiClient _twitchApi;
    private readonly TwitchConfiguration _twitchConfiguration;
    private readonly ILogger<TwitchAuthenticationProvider> _logger;

    public TwitchAuthenticationProvider(
        TwitchConfiguration twitchConfiguration,
        ITwitchApiClient twitchApi,
        TwitchTokenStore twitchTokenStore,
        ILogger<TwitchAuthenticationProvider> logger)
    {
        _twitchConfiguration = twitchConfiguration;
        _twitchApi = twitchApi;
        _tokenStore = twitchTokenStore;
        _logger = logger;
    }

    private string GetAuthenticationUrl()
    {
        var scopes = string.Join(" ", TwitchScopes.Scopes);
        var authUrl = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={_twitchConfiguration.ClientId}&redirect_uri={_twitchConfiguration.RedirectUrl}&scope={scopes}";
        return authUrl;
    }

    public async Task HandleCode(string code)
    {
        var resp = await _twitchApi.Authorize(code);
        _tokenStore.Token = resp.AccessToken;
        _tokenStore.RefreshToken = resp.RefreshToken;

        if (_refreshTimer != null)
        {
            _refreshTimer.Dispose();
        }

        var timeout = (int) TimeSpan.FromSeconds(120).TotalMilliseconds;
        _refreshTimer = new Timer(RefreshTokenCallback, null, timeout, timeout);
    }

    private async void RefreshTokenCallback(object? state)
    {
        if (_tokenStore.RefreshToken == null)
        {
            return;
        }

        _logger.LogInformation("Refreshing token");
        var resp = await _twitchApi.RefreshToken(_tokenStore.RefreshToken);
        _tokenStore.RefreshToken = resp.RefreshToken;
        _tokenStore.Token = resp.AccessToken;
    }

    public void StartAuthentication()
    {
        var authUrl = GetAuthenticationUrl();
        Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
    }

    public string GetToken()
    {
        if (_tokenStore.Token == null)
        {
            throw new Exception("Missing authentication token");
        }

        return _tokenStore.Token;
    }
}
