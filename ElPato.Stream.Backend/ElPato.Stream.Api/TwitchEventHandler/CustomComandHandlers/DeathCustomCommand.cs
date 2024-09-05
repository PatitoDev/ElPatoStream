using ElPato.Stream.Api.WebsocketServer;
using ElPato.Stream.BusEvents;
using ElPato.Stream.TwitchApi;

namespace ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;

public class DeathCustomCommand : ICustomCommand
{
    private readonly IServerConnectionManager _connectionManager;

    public DeathCustomCommand(IServerConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public bool AppliesTo(string commandType)
    {
        return ChatCommands.Death.Any(s => string.Equals(commandType, s, StringComparison.OrdinalIgnoreCase));
    }

    public async Task Handle(IEnumerable<string> args, ChatMessageEvent msg, CancellationToken cancellation = default)
    {
        if (!(msg.IsMod || msg.IsBroadcaster)) return;

        var value = args.FirstOrDefault();

        if (!int.TryParse(value, out var amount))
        {
            amount = 1;
        };

        var busEvent = new IncreaseDeathCounterEvent()
        {
            Content = new(amount)
        };
        await _connectionManager.Broadcast(busEvent, cancellation);
    }
}
