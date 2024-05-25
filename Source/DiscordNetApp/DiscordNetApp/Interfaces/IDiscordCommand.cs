using Discord;
using Discord.WebSocket;

namespace DiscordNetApp.Interfaces;
internal interface IDiscordCommand
{
    public string CommandName { get; }

    public SlashCommandProperties BuildCommand();
    public Task ExecuteAsync(SocketSlashCommand command);
}
