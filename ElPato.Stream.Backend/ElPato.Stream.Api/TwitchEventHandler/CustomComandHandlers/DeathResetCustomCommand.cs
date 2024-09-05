using ElPato.Stream.Api.WebsocketServer;
using ElPato.Stream.BusEvents;
using ElPato.Stream.TwitchApi;

namespace ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;

public class DeathResetCustomCommand : ICustomCommand
{
    private readonly IServerConnectionManager _connectionManager;

    public DeathResetCustomCommand(IServerConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public bool AppliesTo(string commandType)
    {
        return ChatCommands.DeathReset.Any(s => string.Equals(commandType, s, StringComparison.OrdinalIgnoreCase));
    }

    public async Task Handle(IEnumerable<string> args, ChatMessageEvent msg, CancellationToken cancellation = default)
    {
        var busEvent = new DeathResetEvent();
        await _connectionManager.Broadcast(busEvent, cancellation);
    }
}
