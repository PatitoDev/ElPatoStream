using ElPato.Stream.TwitchApi;
using Microsoft.AspNetCore.Mvc;

namespace ElPato.Stream.Api.Controllers;

[ApiController]
[Route("Authenticate")]
public class AuthenticationController : Controller
{
    private ITwitchApiClient _twitchApiClient { get; }
    private TwitchEventClient _twitchEventClient { get; }
    private ITwitchAuthenticationProvider _twitchAuthenticationProvider { get; }

    public AuthenticationController(ITwitchApiClient twitchApiClient, TwitchEventClient twitchEventClient, ITwitchAuthenticationProvider twitchAuthenticationProvider)
    {
        _twitchApiClient = twitchApiClient;
        _twitchEventClient = twitchEventClient;
        _twitchAuthenticationProvider = twitchAuthenticationProvider;
    }

    [HttpGet]
    public async Task<ActionResult> Authenticate([FromQuery] string code)
    {
        await _twitchAuthenticationProvider.HandleCode(code);
        _twitchEventClient.Connect();
        return Ok("Success");
    }
}
