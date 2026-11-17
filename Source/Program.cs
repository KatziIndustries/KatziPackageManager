using KatziPackageManager.Commands;

internal static class Program
{
    public static string CacheDirectory { get; private set; } = "";

    public static async Task Main(string[] args)
    {
        // Check if XDG_CACHE_HOME is set and throw an exception if not
        {
            CacheDirectory = Environment.GetEnvironmentVariable("XDG_CACHE_HOME") ?? throw new DirectoryNotFoundException("Couldn't find cache directory");
        }

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
                    Katzibuild? katzibuild = await Packager.GetKatzibuild(queryCommand.PackageName);
                    if (katzibuild.HasValue)
                        PrintKatzibuildInfo(katzibuild.Value);
                    else
                        ErrorArticulater.ArticulateCommandError(new(Error.PackageNotFound, $"Package '{queryCommand.PackageName}' not found"));
                    break;
                }
        }
    }

    private static void PrintKatzibuildInfo(Katzibuild katzibuild)
    {
        Console.WriteLine($"Type: {katzibuild.Type}");
        Console.WriteLine($"Name: {katzibuild.Name}");
        Console.WriteLine($"Description: {katzibuild.Description}");
        Console.WriteLine($"Url: {katzibuild.Url}");

        if (katzibuild.Dependencies != null && katzibuild.Dependencies.Length > 0)
        {
            Console.WriteLine($"Dependencies: {katzibuild.Dependencies.Aggregate((a, b) => $"{a}, {b}")}");
        }
        else
        {
            Console.WriteLine($"Package '{katzibuild.Name}' has no dependencies");
        }
    }
}
