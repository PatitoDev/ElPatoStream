namespace ElPato.Stream.TwitchApi;

public record WelcomeEventPayload 
{
    public required WelcomeSessionData Session { get; set; }
}

public record WelcomeSessionData
{
    public required string Id { get; set; }
    public required string Status { get; set; }
    public required string ConnectedAt { get; set; }
    public required int KeepaliveTimeoutSeconds { get; set; }
    public string? ReconnectUrl { get; set; }
}
