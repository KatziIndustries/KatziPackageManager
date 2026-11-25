using KatziPackageManager.Commands;

public static class CliParser
{
    private const string UsageText =
    """
    KatziPackageManager
    Usage:
        build [directory]: builds a .kpg from the specified directory
        verify [directory]: verifies if a directory could be built as a .kpg
    """;

    public static ICommand Parse(string[] args)
    {
        if (args.Length == 0)
            return new ErrorCommand(ErrorLevel.Info, UsageText);

        return new ErrorCommand(ErrorLevel.Error, "LOL");
    }
}
