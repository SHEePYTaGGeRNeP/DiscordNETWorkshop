using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace DiscordNetApp;

public static class Program
{
    private static SocketGuild? _guild;
    private static DiscordSocketClient? _client;
    private readonly static Random _random = new();
    private static ulong _appId;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to Kennisweekend 2024!");

        // Retrieve App Secrets
        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets<TestClass>()
            .Build();

        string token = config.GetSection("Discord")["Token"] ?? throw new Exception("No Token found!") ;
        var appId = config.GetSection("Discord")["AppID"] ?? throw new Exception("No AppID found!");
        _appId = Convert.ToUInt64(appId);

        Console.WriteLine("Starting up Discord Socket Client!");

        _client = new(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.AllUnprivileged
        });
        _client.Log += Log;
        _client.MessageReceived += Client_MessageReceived;
        _client.Ready += Client_Ready;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    static Task Client_Ready()
    {
        _guild = _client!.Guilds.FirstOrDefault(g => g.Name == "Kennisweekend 2024");
        return Task.CompletedTask;
    }

    static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    static async Task Client_MessageReceived(SocketMessage arg)
    {
        Console.WriteLine($"Received message {arg.Content} from {arg.Author.Username} at {arg.Timestamp}");

        // The bot receives message received events on message it sends itself.
        if (arg.Author.Id == _appId)
        {
            return;
        }

        if (arg is not SocketUserMessage msg || msg.Author.IsBot)
        {
            return;
        }

        if (msg.Channel.Name == "bot-gekte")
        {
            await msg.Channel.TriggerTypingAsync();
            await msg.Channel.SendMessageAsync($"Hey {msg.Author.GlobalName}!");

            // only works with custom server emojis
            var serverEmotes = await _guild!.GetEmotesAsync();
            GuildEmote? emote = serverEmotes.ElementAt(_random.Next(serverEmotes.Count)); // (e => e.Name == "Emote name")
            if (emote != null)
            {
                await msg.AddReactionAsync(emote);
            }

            await msg.Author.SendMessageAsync($"Hey {msg.Author.Username}, why did you send: {msg.Content}?");
        }
    }
}