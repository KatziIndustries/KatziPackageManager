using KatziPackageManager.Commands;

internal static class Program
{
    public static void Main(string[] args)
    {
        Command command = CliParser.Parse(args);

        if (command is ErrorCommand errorCommand)
        {
            switch (errorCommand.Error)
            {
                case Error.WrongUsage:
                    PrintUsage();
                    return;

                case Error.MissingArguments or Error.Unknown:
                    Console.WriteLine(errorCommand.Description);
                    return;
            }
        }

        if (command is InstallCommand installCommand)
        {
            PackageInstaller.Install(installCommand);
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: kpm [Action] [Options]");
    }
}
