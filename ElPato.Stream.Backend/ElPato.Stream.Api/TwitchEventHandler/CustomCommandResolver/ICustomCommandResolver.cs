using ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;

namespace ElPato.Stream.Api.TwitchEventHandler.CustomCommandResolver;

public interface ICustomCommandResolver
{
    public ICustomCommand? GetCommand(string command);
}
