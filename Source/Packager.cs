using KatziPackageManager.Commands;

public static class Packager
{
    public const string KUR_URL = "https://kur.katzi.org";

    private static readonly HttpClient _client = new();

    public static async Task<Error> Install(string packageName)
    {
        Katzibuild? katzibuild = await GetKatzibuild(packageName);
        if (katzibuild == null) return Error.PackageNotFound;

        return Error.Success;
    }

    public static async Task<Katzibuild?> GetKatzibuild(string packageName)
    {
        HttpResponseMessage response = await _client.GetAsync(GetPackageUrl(packageName));

        string responseContent = await response.Content.ReadAsStringAsync();

        if (responseContent == string.Empty)
            return null;


        return KatzibuildParser.Parse(responseContent);
    }

    private static string GetPackageUrl(string packageName)
    {
        return KUR_URL + $"/package/{packageName}/KATZIBUILD";
    }
}
