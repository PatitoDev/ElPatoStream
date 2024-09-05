namespace ElPato.Stream.TwitchApi;

public record ChannelInformation(
    string BroadcasterId,
    string BroadcasterLogin,
    string BroadcasterName,
    string BroadcasterLanguage,
    string GameId,
    string GameName,
    string Title,
    int Delay,
    IEnumerable<string> Tags,
    IEnumerable<string> ContentClassificationLabels,
    bool IsBrandedContent
);
