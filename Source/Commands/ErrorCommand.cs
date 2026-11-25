using KatziPackageManager.Commands;

public struct ErrorCommand(ErrorLevel errorLevel, string errorMessage) : ICommand
{
    public bool Ok { get; private set; }
    public readonly string ErrorMessage = errorMessage;
    public readonly ErrorLevel ErrorLevel = errorLevel;

    public void Execute() { }
}
