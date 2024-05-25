using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Discord.WebSocket;
using Discord;
using DiscordNetApp.Interfaces;

namespace DiscordNetApp;

internal class Bot : IBot
{
    private readonly ILogger<Bot> _logger;
    private readonly IConfiguration _config;
    private readonly ISlashCommandHandler _slashCommandHandler;
    private readonly DiscordSocketClient _client;
    private readonly ulong _appId;

    private SocketGuild? _guild;

    public Bot(ILogger<Bot> logger, IConfiguration configuration, ISlashCommandHandler slashCommandHandler)
    {
        _logger = logger;
        _config = configuration;

        string appId = _config.GetSection("Discord")["AppID"] ?? throw new Exception("No AppID found!");
        _appId = Convert.ToUInt64(appId);

        _client = new(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });
        _client.Log += Log;
        _client.Ready += Client_Ready;
        _client.MessageReceived += Client_MessageReceived;
        _slashCommandHandler = slashCommandHandler;
        _client.SlashCommandExecuted += slashCommandHandler.Client_SlashCommandExecuted;
        _slashCommandHandler.SetClient(_client);
    }


    public async Task StartAsync(ServiceProvider services)
    {
        string token = _config.GetSection("Discord")["Token"] ?? throw new Exception("No Token found!");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
    }

    private async Task Client_Ready()
    {
        _guild = _client.Guilds.FirstOrDefault(g => g.Name == "Kennisweekend 2024") ?? _client.Guilds.First();
        _logger.LogInformation($"Client_Ready enter and _guild = {_guild.Name}");

        await _slashCommandHandler.RegisterCommands(_guild);
        _logger.LogInformation($"Client_Ready done.");
    }

    private Task Log(LogMessage msg)
    {
        _logger.LogInformation(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task Client_MessageReceived(SocketMessage arg)
    {
        _logger.LogInformation($"Received message {arg.Content} from {arg.Author.Username} at {arg.Timestamp}");

        // The bot receives message received events on the message it sends so we ignore them.
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
            await msg.Channel.SendMessageAsync($"Hey {msg.Author.GlobalName}! Do you want me to tag you? {msg.Author.Mention}");

            // only works with custom server emojis
            var serverEmotes = await _guild!.GetEmotesAsync();
            GuildEmote? emote = serverEmotes.ElementAt(Utils.Random.Next(serverEmotes.Count)); // (e => e.Name == "Emote name")
            if (emote != null)
            {
                await msg.AddReactionAsync(emote);
            }

            await msg.Author.SendMessageAsync($"Hey {msg.Author.Username}, why did you send: {msg.Content}?");
        }
    }

    public async Task StopAsync()
    {
        _logger.LogInformation("Shutting down");

        if (_client != null)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
    }

}
