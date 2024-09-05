namespace ElPato.Stream.TwitchApi;

public record EventSubscription(string Key, Dictionary<string, string> Condition, int Version = 1);

