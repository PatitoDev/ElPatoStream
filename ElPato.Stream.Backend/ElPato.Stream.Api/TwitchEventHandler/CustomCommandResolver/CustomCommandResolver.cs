using ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;

namespace ElPato.Stream.Api.TwitchEventHandler.CustomCommandResolver;

public class CustomCommandResolver: ICustomCommandResolver
{
    private readonly IEnumerable<ICustomCommand> _customCommands;

    public CustomCommandResolver(IEnumerable<ICustomCommand> commands)
    {
        _customCommands = commands;
    }

    public ICustomCommand? GetCommand(string command) =>
        _customCommands.FirstOrDefault(c => c.AppliesTo(command));
}
