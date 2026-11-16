namespace KatziPackageManager.Commands;

public class QueryCommand(string packageName) : Command
{
    public readonly string PackageName = packageName;
}
