using KatziPackageManager.Commands;

public static class PackageInstaller
{
    public static void Install(InstallCommand installCommand)
    {
        Console.WriteLine(installCommand.PackageName);
    }
}
