using KatziPackageManager.Commands;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        Command command = CliParser.Parse(args);

        switch (command)
        {
            case ErrorCommand errorCommand:
                ErrorArticulater.ArticulateCommandError(errorCommand);
                break;

            case InstallCommand installCommand:
                {
                    Error error = await Packager.Install(installCommand.PackageName);
                    if (error != Error.Success)
                        ErrorArticulater.ArticulateCommandError(error, installCommand);
                    break;
                }

            case QueryCommand queryCommand:
                {
                    string? katzibuild = await Packager.GetKatzibuild(queryCommand.PackageName);
                    if (katzibuild != null)
                        Console.WriteLine(katzibuild);
                    else
                        ErrorArticulater.ArticulateCommandError(new(Error.PackageNotFound));
                    break;
                }
        }
    }
}
