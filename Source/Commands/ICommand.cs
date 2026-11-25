namespace KatziPackageManager.Commands;

public interface ICommand
{
    bool Ok { get; }

    public void Execute();
}
