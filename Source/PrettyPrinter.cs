public static class PrettyPrinter
{
    public static void PrintInfo(string text) => Print(ConsoleColor.White, text);
    public static void PrintWarning(string text) => Print(ConsoleColor.Yellow, text);
    public static void PrintError(string text) => Print(ConsoleColor.Red, text);
    public static void PrintYay(string text) => Print(ConsoleColor.Green, text);

    private static void Print(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;

        Console.WriteLine(text);

        Console.ResetColor();
    }
}
