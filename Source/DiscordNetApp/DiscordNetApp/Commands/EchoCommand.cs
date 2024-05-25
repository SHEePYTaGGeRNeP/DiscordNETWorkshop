using Discord;
using Discord.WebSocket;
using DiscordNetApp.Interfaces;

namespace DiscordNetApp.Commands;

internal class EchoCommand : IDiscordCommand
{
    public string CommandName => "echo";

    public SlashCommandProperties BuildCommand()
    {
        return new SlashCommandBuilder()
            .WithName(CommandName)
            .WithDescription("Echoes back what was said.")
            .AddOption("phrase", ApplicationCommandOptionType.String, "the phrase to be echoed", isRequired: true)
            //.AddOption("user", ApplicationCommandOptionType.User, "specific user", isRequired: true)
            .Build();
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        bool onlyVisibileForUser = true;

        string? phrase = command.Data.Options.First().Value.ToString();
        if (string.IsNullOrEmpty(phrase))
        {
            await command.RespondAsync("insert a phrase");
            return;
        }

        //command.CommandName
        await command.RespondAsync(phrase, ephemeral: onlyVisibileForUser);
    }

}
