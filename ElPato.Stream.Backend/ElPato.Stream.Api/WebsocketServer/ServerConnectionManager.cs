using ElPato.Stream.BusEvents.Base;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ElPato.Stream.Api.WebsocketServer;

public class ServerConnectionManager : IServerConnectionManager
{
    private readonly IList<WebSocket> _sockets = [];

    public void AddConnection(WebSocket connection)
    {
        _sockets.Add(connection);
    }

    public void RemoveConnection(WebSocket connection)
    {
        _sockets.Remove(connection);
    }

    public async Task Broadcast(string payload, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        foreach (var socket in _sockets)
        {
            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            tasks.Add(SendStringAsync(socket, payload, cancellationToken));
        }

        await Task.WhenAll([.. tasks]);
    }

    public IList<WebSocket> GetActiveConnections()
    {
        return [.. _sockets];
    }

    public async Task SendMessage(WebSocket connection, string payload, CancellationToken cancellationToken = default)
    {
        await SendStringAsync(connection, payload, cancellationToken);
    }

    private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
    {
        var buffer = Encoding.UTF8.GetBytes(data);
        var segment = new ArraySegment<byte>(buffer);
        return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
    }

    public async Task Broadcast<T>(T payload, CancellationToken cancellationToken = default)
    {
        var data = JsonSerializer.Serialize(payload, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        })!;

        await Broadcast(data, cancellationToken);
    }
}
