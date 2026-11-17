using KatziPackageManager.Commands;

internal static class Program
{
    public static string CacheDirectory => Path.Combine(Environment.GetEnvironmentVariable("XDG_CACHE_HOME")!, "kpm");

    public static async Task Main(string[] args)
    {
        // Check if XDG_CACHE_HOME is set and throw an exception if not
        {
            _ = Environment.GetEnvironmentVariable("XDG_CACHE_HOME") ?? throw new DirectoryNotFoundException("Couldn't find cache directory");
            if (!Directory.Exists(CacheDirectory))
            {
                Directory.CreateDirectory(CacheDirectory);
            }
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
                        PrintKatzibuildInfo(katzibuild.Value, queryCommand.PrintRaw);
                    else
                        ErrorArticulater.ArticulateCommandError(new(Error.PackageNotFound, $"Package '{queryCommand.PackageName}' not found"));
                    break;
                }
        }
    }

    private static void PrintKatzibuildInfo(Katzibuild katzibuild, bool raw)
    {
        if (raw)
        {
            Console.WriteLine("Not implemented yet");
            return;
        }

        Console.WriteLine($"Type: {katzibuild.Type}");
        Console.WriteLine($"Name: {katzibuild.Name}");
        Console.WriteLine($"Description: {katzibuild.Description}");
        Console.WriteLine($"Version: {katzibuild.Version}");
        Console.WriteLine($"Url: {katzibuild.Url}");

        if (katzibuild.Dependencies != null && katzibuild.Dependencies.Length > 0)
            Console.WriteLine($"Dependencies: {katzibuild.Dependencies.Aggregate((a, b) => $"{a}, {b}")}");
        else
            Console.WriteLine($"Package '{katzibuild.Name}' has no dependencies");
    }
}
