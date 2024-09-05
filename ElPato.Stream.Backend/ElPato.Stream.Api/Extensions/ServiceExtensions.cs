using ElPato.Stream.Api.TwitchEventHandler.CustomComandHandlers;
using ElPato.Stream.Api.TwitchEventHandler.CustomCommandResolver;

namespace ElPato.Stream.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomTwitchCommands(this IServiceCollection services){

        services.AddSingleton<ICustomCommand, ShoutoutCustomCommand>();
        services.AddSingleton<ICustomCommand, DeathCustomCommand>();
        services.AddSingleton<ICustomCommand, DeathResetCustomCommand>();

        services.AddSingleton<ICustomCommandResolver>(s =>
        {
            var commands = s.GetServices<ICustomCommand>();
            return new CustomCommandResolver(commands);
        });

        return services;
    }
}
