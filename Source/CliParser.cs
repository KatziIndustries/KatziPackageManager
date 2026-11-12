using KatziPackageManager.Commands;

public static class CliParser
{
    public static Command Parse(string[] args)
    {
        if (args.Length <= 0)
        {
            return new ErrorCommand(Error.WrongUsage);
        }

        switch (args[0])
        {
            case "--help" or "-h":
                return new ErrorCommand(Error.WrongUsage);

            case "install":
                if (args.Length > 1)
                    return new InstallCommand(args[1]);
                else
                    return new ErrorCommand(Error.MissingArguments, "Install: Package name required");

            default:
                return new ErrorCommand(Error.Unknown, $"Unknown Argument: {args[0]}");
        }
    }
}
