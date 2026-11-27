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

        try
        {
            OutputFileLocation = KatziPackager.Create(PackagePath, PreferredOutputDir) ?? "";
        }
        catch (IOException)
        {
            PrettyPrinter.Print(ErrorLevel.Error, "A built package with the same name already exists at the output path");
        }

        if (OutputFileLocation == string.Empty)
            Ok = false;
        else
            Ok = true;
    }
}
