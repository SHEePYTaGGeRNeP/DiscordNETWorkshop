using Discord.Net;
using Discord.WebSocket;
using DiscordNetApp.Commands;
using DiscordNetApp.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DiscordNetApp;

internal class SlashCommandHandler : ISlashCommandHandler
{
    private readonly ILogger<SlashCommandHandler> _logger;
    private DiscordSocketClient? _client;

    private readonly IDiscordCommand[] _commands = [new EchoCommand(), new BierCommand(), new ListRolesCommand(), new DogFactCommand()];

    public SlashCommandHandler(ILogger<SlashCommandHandler> logger)
    {
        _logger = logger;
    }

    public void SetClient(DiscordSocketClient client)
    {
        _client = client;
    }

    public async Task RegisterCommands(SocketGuild guild)
    {
        if (_client == null)
        {
            throw new Exception("_client has not been set yet!");
        }

        try
        {
            foreach (var command in _commands)
            {
                _logger.LogInformation($"Registering {command.CommandName}...");

                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
                await guild.CreateApplicationCommandAsync(command.BuildCommand());

                // With global commands we don't need the guild.
                // The global commands take up to an hour to register every time the CreateGlobalApplicationCommandAsync() is called for a given command.
                await _client.CreateGlobalApplicationCommandAsync(command.BuildCommand());
                // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
            }
        }
        catch (HttpException exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            string json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            _logger.LogError(json);
        }
    }

    public async Task Client_SlashCommandExecuted(SocketSlashCommand command)
    {
        foreach (var cmd in _commands)
        {
            if (command.CommandName == cmd.CommandName)
            {
                await cmd.ExecuteAsync(command);
                return;
            }
        }

        _logger.LogWarning($"No command to handle: {command.CommandName}");
    }

}
