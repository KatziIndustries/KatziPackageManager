using KatziPackageManager.Commands;

public static class CliParser
{
    private const string UsageText =
    """
    KatziPackageManager
    Usage:
        build (directory) [-o (output dir)]: 
            builds a .kpg from the specified directory
            
        verify (directory): 
            verifies if a directory could be built as a .kpg
            
        install (package path/package name in kur): 
            installs a .kpg from the kur or from a file on the system
    """;

    public static ICommand Parse(string[] args)
    {
        if (args.Length == 0)
            return new ErrorCommand(ErrorLevel.Info, UsageText);

        switch (args[0])
        {
            case "verify":
                if (args.Length > 1)
                    return new VerifyCommand(Path.GetFullPath(args[1]));
                else
                    return new ErrorCommand(ErrorLevel.Error, "No directory specified");

            case "build":
                if (args.Length > 1)
                {
                    string? outputDir = GetPositionalParameter("-o", args);
                    return new BuildCommand(Path.GetFullPath(args[1]), outputDir == null ? null : Path.GetFullPath(outputDir));
                }
                else
                    return new ErrorCommand(ErrorLevel.Error, "No directory specified");

            case "install":
                if (args.Length > 1)
                    return new InstallCommand(Path.GetFullPath(args[1]));
                else
                    return new ErrorCommand(ErrorLevel.Error, "No package specified");

            default:
                return new ErrorCommand(ErrorLevel.Error, $"Unknown command '{args[1]}'");
        }
    }

    private static string? GetPositionalParameter(string parameter, string[] args)
    {
        int index = args.IndexOf(parameter);

        if (index == -1)
            return null;

        if (index == args.Length - 1)
            return null;

        return args[index + 1];
    }
}
