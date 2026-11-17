namespace KatziPackageManager.Commands;

public class QueryCommand(string packageName, bool printRaw) : Command
{
    public readonly string PackageName = packageName;
    public readonly bool PrintRaw = printRaw;
}
