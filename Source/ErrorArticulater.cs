using KatziPackageManager.Commands;

public static class ErrorArticulater
{
    public static void ArticulateCommandError(Error error, Command command)
    {
        switch (command)
        {
            case InstallCommand installCommand:
                if (error == Error.PackageNotFound)
                    Console.WriteLine($"Package '{installCommand.PackageName}' not found");
                break;

            case ErrorCommand errorCommand:
                switch (error)
                {
                    case Error.WrongUsage:
                        PrintUsage();
                        break;

                    case Error.MissingArguments or Error.Unknown:
                        Console.WriteLine(errorCommand.Description);
                        break;
                }
                break;
        }
    }

    public static void ArticulateCommandError(ErrorCommand command)
    {
        ArticulateCommandError(command.Error, command);
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: kpm [Action] [Options]");
    }
}
