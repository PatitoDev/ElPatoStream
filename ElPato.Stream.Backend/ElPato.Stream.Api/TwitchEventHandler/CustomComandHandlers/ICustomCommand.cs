using ElPato.Stream.TwitchApi;

namespace ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;

public interface ICustomCommand
{
    public bool AppliesTo(string commandType);
    public Task Handle(IEnumerable<string> args, ChatMessageEvent msg, CancellationToken cancellationToken = default);
}
