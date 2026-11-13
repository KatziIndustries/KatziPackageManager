using KatziPackageManager.Commands;

public static class PackageInstaller
{
    public const string KUR_URL = "https://kur.katzi.org";

    private static HttpClient _client = new();

    public static async Task Install(InstallCommand installCommand)
    {
        HttpResponseMessage response = await _client.GetAsync(GetPackageUrl(installCommand.PackageName));

        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    private static string GetPackageUrl(string packageName)
    {
        return KUR_URL + $"/package/{packageName}/KATZIBUILD";
    }
}
