using Discord;
using Discord.WebSocket;
using DiscordNetApp.Interfaces;
using System.Text.Json;

namespace DiscordNetApp.Commands;

internal class DogFactCommand : IDiscordCommand
{
    public string CommandName => "dog-fact";

    public SlashCommandProperties BuildCommand()
    {
        return new SlashCommandBuilder()
            .WithName(CommandName)
            .WithDescription("Make HTTP request to get a random dog fact.")
            .Build();
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://dog-api.kinduff.com/");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("api/facts", UriKind.Relative),
        };
        var result = await client.SendAsync(request);
        var response = await result.Content.ReadAsStringAsync();
        var dogFact = JsonSerializer.Deserialize<DogFactResponse>(response);
        if (dogFact == null || !dogFact.success || dogFact.facts == null)
        {
            await command.Channel.SendMessageAsync($"Something went wrong: {result}");
        }
        else
        {
            await command.Channel.SendMessageAsync(dogFact.facts[0]);
        }
    }

    private class DogFactResponse
    {
        public string[]? facts { get; set; }
        public bool success { get; set; }
    }

}
