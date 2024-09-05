namespace ElPato.Stream.TwitchApi;

public record TwitchEvent<T>
{
    public required TwitchEventMetadata Metadata { get; set; }
    public required T Payload { get; set; }
}

public record TwitchEventMetadata
{
    public required string MessageId { get; set; }
    public required string MessageType { get; set; }
    public required string MessageTimestamp { get; set; }
}

public record TwitchSubscriptionEventMetadata
{
    public required string Id { get; set; }
    public required string Status { get; set; }
    public required string Type { get; set; }
    public required string Version { get; set; }
}
