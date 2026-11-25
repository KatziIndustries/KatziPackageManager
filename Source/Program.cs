using Katzi.Package;

using KatziPackageManager.Commands;

internal static class Program
{
    public static void Main(string[] args)
    {
        BuildCommand build = new("/home/katzi/KatziPackage");

        if (build.Ok)
        {
            PrettyPrinter.PrintYay("Succesfully built package");
            PrettyPrinter.PrintInfo($"The package can be found at '{build.OutputFileLocation}'");
        }
        else
        {
            PrettyPrinter.PrintError("Failed to build package");
        }

    }
}
