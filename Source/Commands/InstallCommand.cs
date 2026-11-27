using System.Diagnostics;
using Katzi.Package;
using KatziPackageManager.Commands;

namespace KatziPackageManager.Commands;

public struct InstallCommand(string packagePath) : ICommand
{
    public bool Ok { get; private set; } = false;

    private readonly string _packagePath = packagePath;

    public void Execute()
    {
        if (!File.Exists(_packagePath))
        {
            PrettyPrinter.Print(ErrorLevel.Error, $"The specified Katzi package does not exist '{_packagePath}'");
            Ok = false;
            return;
        }

        KatziPackage katziPackage = new(_packagePath);

        PrettyPrinter.Print(ErrorLevel.Info, $"Installing '{katziPackage.Metadata.Name} {katziPackage.Metadata.Version}'");

        if (katziPackage.PreInstallScript != null)
        {
            PrettyPrinter.Print(ErrorLevel.Info, "Running pre-install script");
            int exitCode = RunScript(katziPackage.PreInstallScript);

            if (exitCode != 0)
            {
                PrettyPrinter.Print(ErrorLevel.Error, "Failed to run pre-install script");
                return;
            }
        }

        if (katziPackage.PostInstallScript != null)
        {
            PrettyPrinter.Print(ErrorLevel.Info, "Running post-install script");
            int exitCode = RunScript(katziPackage.PostInstallScript);

            if (exitCode != 0)
            {
                PrettyPrinter.Print(ErrorLevel.Error, "Failed to run post-install script");
                return;
            }
        }

        katziPackage.Dispose();

        Ok = true;
    }

    private readonly int RunScript(string script)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = Environment.GetEnvironmentVariable("SHELL") ?? "/bin/sh",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        Process process = Process.Start(startInfo) ?? throw new Exception("Failed to start shell process for running script");
        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                PrettyPrinter.Print(ErrorLevel.Info, e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
                PrettyPrinter.Print(ErrorLevel.Error, e.Data);
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using (StreamWriter streamWriter = process.StandardInput)
        {
            streamWriter.Write(script);
        }

        process.WaitForExit();

        return process.ExitCode;
    }
}
