using ElPato.Stream.BusEvents.Base;
using System.Net.WebSockets;

namespace ElPato.Stream.Api.WebsocketServer;

public interface IServerConnectionManager
{
    void AddConnection(WebSocket connection);

    void RemoveConnection(WebSocket connection);

    IList<WebSocket> GetActiveConnections();

    Task SendMessage(WebSocket connection, string payload, CancellationToken cancellationToken = default);

    Task Broadcast(string payload, CancellationToken cancellationToken = default);
    Task Broadcast<T>(T payload, CancellationToken cancellationToken = default);
}
