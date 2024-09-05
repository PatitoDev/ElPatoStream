using System.Net.WebSockets;
using System.Text;

namespace ElPato.Stream.Api.WebsocketServer.Middlewares;

public class WebsocketServerMiddleware
{

    private readonly RequestDelegate _next;
    private readonly IServerConnectionManager _connectionManager;

    public WebsocketServerMiddleware(RequestDelegate next, IServerConnectionManager connectionManager)
    {
        _next = next;
        _connectionManager = connectionManager;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            await _next.Invoke(context);
            return;
        }

        var ct = context.RequestAborted;
        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        _connectionManager.AddConnection(socket);
        Console.WriteLine("WS: New client connected");

        while (socket.State == WebSocketState.Open)
        {
            try
            {
                var response = await ReceiveStringAsync(socket, ct);
                await _connectionManager.SendMessage(socket, $"Received: {response}", ct);
            } catch (Exception ex) {
                if (ex is not OperationCanceledException)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        Console.WriteLine("WS: Lost connection to a client");
        _connectionManager.RemoveConnection(socket);
    }

    private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default)
    {
        // Message can be sent by chunk.
        // We must read all chunks before decoding the content
        var buffer = new ArraySegment<byte>(new byte[8192]);
        using var ms = new MemoryStream();
        WebSocketReceiveResult result;
        do
        {
            ct.ThrowIfCancellationRequested();

            result = await socket.ReceiveAsync(buffer, ct);
            ms.Write(buffer.Array!, buffer.Offset, result.Count);
        }
        while (!result.EndOfMessage);

        ms.Seek(0, SeekOrigin.Begin);
        if (result.MessageType != WebSocketMessageType.Text)
            throw new Exception("Unexpected message");

        // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
        using var reader = new StreamReader(ms, Encoding.UTF8);
        return await reader.ReadToEndAsync(ct);
    }
}
