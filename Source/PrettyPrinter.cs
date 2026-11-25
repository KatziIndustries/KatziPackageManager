public static class PrettyPrinter
{
    public static void Print(ErrorLevel errorLevel, string message)
    {
        switch (errorLevel)
        {
            case ErrorLevel.Info: Print(ConsoleColor.White, message); break;
            case ErrorLevel.Warning: Print(ConsoleColor.Yellow, message); break;
            case ErrorLevel.Error: Print(ConsoleColor.Red, message); break;
            case ErrorLevel.Yay: Print(ConsoleColor.Green, message); break;
        }
    }

    private static void Print(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;

        Console.WriteLine(text);

        Console.ResetColor();
    }
}
