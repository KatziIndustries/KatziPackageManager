using Katzi.Package;

namespace KatziPackageManager.Commands;

public struct VerifyCommand(string packageDirPath) : ICommand
{
    public readonly string PackageDirPath = packageDirPath;

    public string[] MissingRequiredFiles { get; private set; } = [];
    public string[] MissingOptionalFiles { get; private set; } = [];
    public string[] UnknownFiles { get; private set; } = [];

    public readonly bool Ok => (MissingRequiredFiles.Length == 0) && (UnknownFiles.Length == 0);

    public void Execute()
    {
        Console.WriteLine(PackageDirPath);
        MissingRequiredFiles = KatziPackager.GetMissingRequiredFiles(PackageDirPath);
        MissingOptionalFiles = KatziPackager.GetMissingOptionalFiles(PackageDirPath);
        UnknownFiles = KatziPackager.GetUnknownFiles(PackageDirPath);
    }

    public readonly void Print()
    {
        if (MissingRequiredFiles.Length > 0)
        {
            PrettyPrinter.Print(ErrorLevel.Error, "Missing required files:");
            foreach (string file in MissingRequiredFiles)
                PrettyPrinter.Print(ErrorLevel.Info, $" - {file}");
        }
        else PrettyPrinter.Print(ErrorLevel.Yay, "All required files found");

        if (MissingOptionalFiles.Length > 0)
        {
            PrettyPrinter.Print(ErrorLevel.Warning, "Missing optional files:");
            foreach (string file in MissingOptionalFiles)
                PrettyPrinter.Print(ErrorLevel.Info, $" - {file}");
        }
        else PrettyPrinter.Print(ErrorLevel.Yay, "All optional files found");

        if (UnknownFiles.Length > 0)
        {
            PrettyPrinter.Print(ErrorLevel.Error, "Unknown files:");
            foreach (string file in UnknownFiles)
                PrettyPrinter.Print(ErrorLevel.Info, $" - {file}");
        }

        if (Ok)
        {
            PrettyPrinter.Print(ErrorLevel.Yay, "Package verified succesfully");
        }
        else
        {
            PrettyPrinter.Print(ErrorLevel.Error, "Package could not be verified");
        }
    }
}
