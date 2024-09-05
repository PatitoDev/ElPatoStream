namespace ElPato.Stream.TwitchApi;

public interface ITwitchAuthenticationProvider
{
    public void StartAuthentication();
    public string GetToken();
    public Task HandleCode(string code);
}
