namespace KatziPackageManager.Commands;

public class InstallCommand(string packageName) : Command
{
    public readonly string PackageName = packageName;
}
