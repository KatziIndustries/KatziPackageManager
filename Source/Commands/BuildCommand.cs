using Katzi.Package;
using KatziPackageManager.Commands;

public readonly struct BuildCommand : ICommand
{
    public bool Ok { get; init; } = false;
    public readonly string OutputFileLocation = "";

    public BuildCommand(string packagePath, string? outputDir = null)
    {
        VerifyCommand verify = new(packagePath);
        verify.Print();

        if (!verify.Ok)
            return;

        OutputFileLocation = KatziPackager.Create(packagePath, outputDir) ?? "";

        if (OutputFileLocation == null)
            Ok = false;
        else
            Ok = true;
    }
}
