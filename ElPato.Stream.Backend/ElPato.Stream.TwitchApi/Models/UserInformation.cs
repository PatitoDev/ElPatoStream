namespace ElPato.Stream.TwitchApi;

public record UserInformation(string Id, string Login, string DisplayName, string ProfileImageUrl, IEnumerable<string> Tags);
