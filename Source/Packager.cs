using System.Diagnostics;
using System.Text;
using KatziPackageManager.Commands;

public static class Packager
{
    public const string KUR_URL = "https://kur.katzi.org";

    private static readonly HttpClient _client = new();

    public static async Task<Error> Install(string packageName)
    {
        Console.Write("Downloading KATZIBUILD... ");

        Katzibuild? katzibuild = await DownloadKatzibuild(packageName);
        if (katzibuild == null) return Error.PackageNotFound;

        Console.WriteLine("Done");

        if (katzibuild.Value.BuildInstructions != null)
        {
            Console.WriteLine("Building package...");
            BuildKatzibuild(katzibuild.Value);
        }

        return Error.Success;
    }

    public static async Task<Katzibuild?> GetKatzibuild(string packageName)
    {
        string programCachePath = Path.Combine(Program.CacheDirectory, packageName);
        string katzibuildPath = Path.Combine(programCachePath, "KATZIBUILD");

        if (Directory.Exists(programCachePath) && File.Exists(katzibuildPath))
        {
            return KatzibuildParser.Parse(File.ReadAllText(katzibuildPath));
        }

        Katzibuild? katzibuild = await DownloadKatzibuild(packageName);
        if (!katzibuild.HasValue) return null;

        Directory.CreateDirectory(programCachePath);
        using (var fs = File.Create(katzibuildPath))
        {
            fs.Write(Encoding.UTF8.GetBytes("katzibuild lel"));
        }

        return katzibuild;
    }

    private static async Task<Katzibuild?> DownloadKatzibuild(string packageName)
    {
        HttpResponseMessage response = await _client.GetAsync(GetPackageUrl(packageName));

        string responseContent = await response.Content.ReadAsStringAsync();

        if (responseContent == string.Empty)
            return null;

        return KatzibuildParser.Parse(responseContent);
    }

    private static void BuildKatzibuild(Katzibuild katzibuild)
    {
        if (katzibuild.BuildInstructions == null)
            return;

        Directory.CreateDirectory($"/tmp/{katzibuild.Name}");
        ProcessStartInfo processInfo = new()
        {
            WorkingDirectory = $"/tmp/{katzibuild.Name}",
        };

        foreach (string instruction in katzibuild.BuildInstructions)
        {
            int firstSpaceIndex = instruction.IndexOf(" ");

            string fileName = instruction[0..firstSpaceIndex];
            string arguments = instruction[firstSpaceIndex..instruction.Length];

            processInfo.FileName = fileName;
            processInfo.Arguments = arguments;

            Process process = Process.Start(processInfo) ?? throw new Exception($"Failed to run build instruction: '{instruction}'");

            process.WaitForExit();
        }
    }

    private static void InstallKatzibuild(Katzibuild katzibuild)
    {

    }

    private static string GetPackageUrl(string packageName)
    {
        return KUR_URL + $"/package/{packageName}/KATZIBUILD";
    }
}
