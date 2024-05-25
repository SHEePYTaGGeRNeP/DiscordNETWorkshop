using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscordNetApp;

public static class Program
{
    public static async Task Main(string[] _)
    {
        Utils.WriteWelcomeMessage();
        
        // Retrieve App Secrets
        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets<Bot>()
            .Build();

        var serviceProvider = new ServiceCollection()
               .AddLogging(options =>
               {
                   options.ClearProviders();
                   options.AddConsole();
               })
               .AddSingleton(config)
               .AddScoped<IBot, Bot>()
               .BuildServiceProvider();

        try
        {
            IBot bot = serviceProvider.GetRequiredService<IBot>();
            await bot.StartAsync(serviceProvider);

            do
            {
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Q)
                {
                    await bot.StopAsync();
                    return;
                }
            } while (true);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            Environment.Exit(-1);
        }
    }
}