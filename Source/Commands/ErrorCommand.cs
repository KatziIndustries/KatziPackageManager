namespace KatziPackageManager.Commands;

public class ErrorCommand(Error error, string? description = "") : Command
{
    public readonly Error Error = error;
    public readonly string? Description = description;
}
