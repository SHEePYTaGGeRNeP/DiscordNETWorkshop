using Discord;
using Discord.WebSocket;
using DiscordNetApp.Interfaces;

namespace DiscordNetApp.Commands;

internal class ListRolesCommand : IDiscordCommand
{
    public string CommandName => "list-roles";

    public SlashCommandProperties BuildCommand()
    {
        return new SlashCommandBuilder()
            .WithName(CommandName)
            .WithDescription("Lists all roles of a user.")
            .AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true)
            .Build();
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        // We need to extract the user parameter from the command. since we only have one option and it's required, we can just use the first option.
        var guildUser = (SocketGuildUser)command.Data.Options.First().Value;

        // We remove the everyone role and select the mention of each role.
        var roleList = string.Join(",\n", guildUser.Roles.Where(x => !x.IsEveryone).Select(x => x.Mention));

        var embedBuiler = new EmbedBuilder()
            .WithAuthor(guildUser.ToString(), guildUser.GetAvatarUrl() ?? guildUser.GetDefaultAvatarUrl())
            .WithTitle("Roles")
            .WithDescription(roleList)
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        // Now, Let's respond with the embed.
        await command.RespondAsync(embed: embedBuiler.Build());
    }

}
