using Discord;
using Discord.WebSocket;
using DiscordNetApp.Interfaces;

namespace DiscordNetApp.Commands;

internal class BierCommand : IDiscordCommand
{
    public string CommandName => "bierhalen";

    public SlashCommandProperties BuildCommand()
    {
        return new SlashCommandBuilder()
            .WithName(CommandName)
            .WithDescription("Tag een willekeurig iemand om bier te halen.")
            .Build();
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        IEnumerable<IUser> users = await command.Channel.GetUsersAsync(CacheMode.AllowDownload, RequestOptions.Default).FlattenAsync();
        var randomUser = users.ElementAt(Utils.Random.Next(users.Count()));
        await command.RespondAsync($"Hey {randomUser.Mention}! Je moet bier halen van {command.User.GlobalName}");
    }

}
