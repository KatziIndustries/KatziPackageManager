using Katzi.Package;
using KatziPackageManager.Commands;

public struct BuildCommand(string packagePath, string? outputDir = null) : ICommand
{
    public bool Ok { get; private set; } = false;

    public string OutputFileLocation { get; private set; } = "";
    public readonly string PackagePath = packagePath;
    public readonly string? PreferredOutputDir = outputDir;

    public void Execute()
    {
        VerifyCommand verify = new(PackagePath);
        verify.Print();

        if (!verify.Ok)
            return;

        OutputFileLocation = KatziPackager.Create(PackagePath, PreferredOutputDir) ?? "";

        if (OutputFileLocation == null)
            Ok = false;
        else
            Ok = true;
    }
}
