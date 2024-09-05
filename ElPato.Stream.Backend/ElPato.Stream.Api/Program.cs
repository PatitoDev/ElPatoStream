using ElPato.Stream.Api;
using ElPato.Stream.Api.Extensions;
using ElPato.Stream.Api.TwitchEventHandler;
using ElPato.Stream.Api.WebsocketServer;
using ElPato.Stream.Api.WebsocketServer.Middlewares;
using ElPato.Stream.TwitchApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(s =>
{
    var configuration = s.GetRequiredService<IConfiguration>();
    var port = configuration.GetValue<int>("Port")!;
    var clientId = configuration.GetValue<string>("Credentials:ClientId")!;
    var clientSecret = configuration.GetValue<string>("Credentials:ClientSecret")!;
    var channelId = configuration.GetValue<string>("ChannelId")!;

    var redirect = $"http://localhost:{port}/Authenticate";
    return new TwitchConfiguration(clientId, clientSecret, channelId, redirect);
});

builder.Services
    .AddSingleton<IServerConnectionManager, ServerConnectionManager>();

builder.Services
    .AddSingleton<TwitchTokenStore>()
    .AddSingleton<ITwitchApiClient, TwitchApiClient>()
    .AddSingleton(new HttpClient())
    .AddSingleton<ITwitchAuthenticationProvider, TwitchAuthenticationProvider>()
    .AddSingleton<TwitchEventClient>();

builder.Services
    .AddCustomTwitchCommands()
    .AddSingleton<ITwitchEventHandler, TwitchEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapSwagger();
app.UseAuthorization();
app.MapControllers();
app.UseWebSockets();

app.UseMiddleware<WebsocketServerMiddleware>();

app.Services
    .GetRequiredService<ITwitchAuthenticationProvider>()
    .StartAuthentication();

app.Services
    .GetRequiredService<ITwitchEventHandler>();

ChatCommands.ValidateCommands();

var configuration = app.Services.GetRequiredService<IConfiguration>();
var port = configuration.GetValue<int>("Port")!;
app.Run($"http://localhost:{port}");
