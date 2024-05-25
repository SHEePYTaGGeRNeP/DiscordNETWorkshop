using Microsoft.Extensions.DependencyInjection;

namespace DiscordNetApp;

internal interface IBot
{
    Task StartAsync(ServiceProvider services);

    Task StopAsync();
}
