namespace DiscordNetApp;

internal static class Utils
{
    public readonly static Random Random = new();

    internal static void WriteWelcomeMessage()
    {
        var previousForeground = Console.ForegroundColor;
        var previousBackground = Console.BackgroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine("- - - - - - ! Welcome to Kennisweekend 2024 ! - - - - - -");
        Console.ForegroundColor = previousForeground;
        Console.BackgroundColor = previousBackground;
    }
}
