using Katzi.Package;

public readonly struct VerifyCommand : Command
{
    public readonly string PackageDirPath;

    public readonly string[]? MissingRequiredFiles;
    public readonly string[]? MissingOptionalFiles;
    public readonly string[]? UnknownFiles;

    public bool Ok => (MissingRequiredFiles == null) && (UnknownFiles == null);

    public VerifyCommand(string packageDirPath)
    {
        PackageDirPath = packageDirPath;

        MissingRequiredFiles = KatziPackager.GetMissingRequiredFiles(packageDirPath);
        MissingOptionalFiles = KatziPackager.GetMissingOptionalFiles(packageDirPath);
        UnknownFiles = KatziPackager.GetUnknownFiles(packageDirPath);
    }

    public void Print()
    {
        if (MissingRequiredFiles != null)
        {
            PrettyPrinter.PrintWarning("Missing required files:");
            foreach (string file in MissingRequiredFiles)
                PrettyPrinter.PrintInfo($" - {file}");
        }
        else PrettyPrinter.PrintYay("All required files found");

        if (MissingOptionalFiles != null)
        {
            PrettyPrinter.PrintWarning("Missing optional files:");
            foreach (string file in MissingOptionalFiles)
                PrettyPrinter.PrintInfo($" - {file}");
        }
        else PrettyPrinter.PrintYay("All optional files found");

        if (UnknownFiles != null)
        {
            PrettyPrinter.PrintWarning("Unknown files:");
            foreach (string file in UnknownFiles)
                PrettyPrinter.PrintInfo($" - {file}");
        }

        if (Ok)
        {
            PrettyPrinter.PrintYay("Package verified succesfully");
        }
        else
        {
            PrettyPrinter.PrintError("Package could not be verified");
        }
    }
}
