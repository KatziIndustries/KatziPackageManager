using Katzi.Package;

using KatziPackageManager.Commands;

internal static class Program
{
    public static void Main(string[] args)
    {
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

            case ErrorCommand errorCommand:
                PrettyPrinter.Print(errorCommand.ErrorLevel, errorCommand.ErrorMessage);
                break;
        }
    }
}
