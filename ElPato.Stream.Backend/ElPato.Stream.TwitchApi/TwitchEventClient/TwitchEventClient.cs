using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ElPato.Stream.TwitchApi;

public partial class TwitchEventClient
{
    private readonly Uri _uri = new("wss://eventsub.wss.twitch.tv/ws");
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
        }
    };
    private CancellationTokenSource? _cancellationTokenSource;
    private ClientWebSocket? _client;
    private ITwitchApiClient _apiClient;
    private TwitchConfiguration _configuration;
    private ILogger<TwitchEventClient> _logger;

    public TwitchEventClient(ITwitchApiClient apiClient, TwitchConfiguration configuration, ILogger<TwitchEventClient> logger) {
        if (!string.IsNullOrWhiteSpace(configuration.WebsocketDevelopmentUrl))
        {
            _uri = new(configuration.WebsocketDevelopmentUrl);
        }

        _apiClient = apiClient;
        _configuration = configuration;
        _logger = logger;
    }

    private static async Task<string> ReceiveMsgAsync(ClientWebSocket ws, CancellationToken ct = default)
    {
        using var ms = new MemoryStream();
        var buff = new ArraySegment<byte>(new byte[5024]);
        WebSocketReceiveResult result;
        do
        {
            result = await ws.ReceiveAsync(buff, ct);
            ms.Write(buff.Array!, buff.Offset, result.Count);
        }
        while (!result.EndOfMessage);
        ms.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(ms, Encoding.UTF8);
        return await reader.ReadToEndAsync(ct);
    }

    public void Connect()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        Task.Run(async () =>
        {
            var cancellationToken = _cancellationTokenSource.Token;
            var ws = new ClientWebSocket();
            _client = ws;
            await ws.ConnectAsync(_uri, CancellationToken.None);

            while (ws.State == WebSocketState.Open)
            {
                try
                {
                    var data = await ReceiveMsgAsync(ws);
                    await HandleEvent(data, cancellationToken);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // log exception but keep connection open
                    if (ex is TaskCanceledException && ws.State == WebSocketState.Open)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                }
            }

            _logger.LogInformation("Connection to twitch lost. Reconnecting in 30s");
            await Task.Delay(1000 * 30);
            Connect();
        });
    }

    private async Task HandleEvent(string dataAsString, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var data = JsonSerializer.Deserialize<JsonObject>(dataAsString, _jsonOptions)!;
        var metadata = data["metadata"].Deserialize<TwitchEventMetadata>(_jsonOptions)!;
        var payload = data["payload"]!;

        switch(metadata.MessageType)
        {
            case "session_welcome":
                _logger.LogInformation("Received welcome event");
                var id = payload?.Deserialize<WelcomeEventPayload>(_jsonOptions)?.Session.Id;
                if (id == null) throw new Exception($"Unable to extract id from welcome message {payload}");

                var subscriptionTasks = EventSubscriptions
                    .GetSubscriptionList(_configuration.UserId)
                    .Select(async ev =>
                        {
                            _logger.LogInformation("Subscribe to: {key}", ev.Key);
                            await _apiClient.SubscribeToEvent(id, ev.Key, ev.Condition, ev.Version);
                        }
                    );
                await Task.WhenAll(subscriptionTasks);
                break;
            case "session_keepalive":
                return;
            case "notification":
                HandleNotificationPayload(payload, cancellationToken);
                break;
        }
    }
}
