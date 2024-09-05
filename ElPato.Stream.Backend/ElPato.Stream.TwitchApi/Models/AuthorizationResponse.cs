namespace ElPato.Stream.TwitchApi;

public record AuthorizationResponse(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken,
    IEnumerable<string> Scope,
    string TokenType
);
