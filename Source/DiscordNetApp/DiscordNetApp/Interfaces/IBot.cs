using Microsoft.Extensions.DependencyInjection;

namespace DiscordNetApp.Interfaces;

internal interface IBot
{
    Task StartAsync(ServiceProvider services);

    Task StopAsync();
}
