using Discord.WebSocket;

namespace DiscordNetApp.Interfaces;
internal interface ISlashCommandHandler
{
    public void SetClient(DiscordSocketClient client);
    public Task RegisterCommands(SocketGuild guild);

    public Task Client_SlashCommandExecuted(SocketSlashCommand command);
}
