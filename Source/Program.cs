using KatziPackageManager.Commands;

internal static class Program
{
    public static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "kpm");

    public static void Main(string[] args)
    {
        Directory.CreateDirectory(DataPath);

#if STATIC_SQLITE
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_internal());
#else
        SQLitePCL.Batteries_V2.Init();
#endif

        ICommand command = CliParser.Parse(args);

        command.Execute();

        switch (command)
        {
            case VerifyCommand verifyCommand:
                verifyCommand.Print();
                break;

            case BuildCommand buildCommand:
                if (!buildCommand.Ok)
                    PrettyPrinter.Print(ErrorLevel.Error, "Failed to build package");
                else
                {
                    PrettyPrinter.Print(ErrorLevel.Yay, "Successfully built package!");
                    PrettyPrinter.Print(ErrorLevel.Info, $"The package can be found at '{buildCommand.OutputFileLocation}'");
                }
                break;

            case InstallCommand installCommand:
                if (!installCommand.Ok)
                    PrettyPrinter.Print(ErrorLevel.Error, "Failed to install package");
                else
                    PrettyPrinter.Print(ErrorLevel.Yay, "Installed package successfully!");
                break;

            case ErrorCommand errorCommand:
                PrettyPrinter.Print(errorCommand.ErrorLevel, errorCommand.ErrorMessage);
                break;
        }

        PackageDatabase.Close();
    }
}
