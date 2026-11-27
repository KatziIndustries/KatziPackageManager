using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

public static class PackageDatabase
{
    public static string DatabasePath => Path.Combine(Program.DataPath, "packages.db");
    private static readonly SqliteConnection _connection;

    static PackageDatabase()
    {
        _connection = new($"Data Source={DatabasePath}");
        _connection.Open();

        using var command = _connection.CreateCommand();
        command.CommandText =
        """
        CREATE TABLE IF NOT EXISTS packages (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL UNIQUE,
            description TEXT NOT NULL,
            version TEXT NOT NULL,
            url TEXT NOT NULL,
            type INTEGER NOT NULL
        );
        
        CREATE TABLE IF NOT EXISTS package_dependencies (
            package_id INTEGER REFERENCES packages(id),
            dependency TEXT NOT NULL
        );
        """;

        command.ExecuteNonQuery();
    }

    public static Package? GetPackage(string packageName)
    {
        long id = -1;
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
            """
            SELECT id FROM packages WHERE name = $packageName;
            """;

            command.Parameters.AddWithValue("$packageName", packageName);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    id = reader.GetInt64(0);
            }

            if (id == -1)
                return null;
        }

        List<string> dependencies = GetPackageDependencies(id);

        Package? package = null;
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
            """
            SELECT * FROM packages WHERE name = $packageName;
            """;

            command.Parameters.AddWithValue("$packageName", packageName);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    package = new()
                    {
                        Id = reader.GetInt64(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Version = reader.GetString(3),
                        Url = reader.GetString(4),
                        Type = reader.GetInt64(5),
                        Dependencies = [.. dependencies]
                    };
                }
            }
        }

        return package;
    }

    public static long AddPackage(Package package)
    {
        using var command = _connection.CreateCommand();

        command.CommandText =
        """
        INSERT INTO packages (name, description, version, url, type)
        VALUES ($name, $description, $version, $url, $type);
        """;

        command.Parameters.AddWithValue("$name", package.Name);
        command.Parameters.AddWithValue("$description", package.Description);
        command.Parameters.AddWithValue("$version", package.Version);
        command.Parameters.AddWithValue("$url", package.Url);
        command.Parameters.AddWithValue("$type", package.Type);

        long packageId = -1;
        try
        {
            packageId = (long)command.ExecuteScalar()!;
        }
        catch (SqliteException)
        {
            PrettyPrinter.Print(ErrorLevel.Error, $"Failed to add package (Name: {package.Name}, Version: {package.Version}), package already exists");
            return packageId;
        }

        SetDependencies(packageId, package.Dependencies);

        return packageId;
    }

    private static void SetDependencies(long packageId, string[] dependencies)
    {
        using var transaction = _connection.BeginTransaction();

        var command = _connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO dependencies (package_id, dependency) VALUES ($packageId, $dependency)";

        var packageParam = command.Parameters.Add("$package_id", SqliteType.Integer);
        var dependencyParam = command.Parameters.Add("$dependency", SqliteType.Text);

        packageParam.Value = packageId;
        foreach (string dependency in dependencies)
        {
            dependencyParam.Value = dependency;
            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }

    public static List<string> GetPackageDependencies(long id)
    {

        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT dependency FROM package_dependencies WHERE package_id = $id";
        command.Parameters.AddWithValue("$id", id);

        List<string> dependencies = [];

        using var reader = command.ExecuteReader();

        while (reader.Read())
            dependencies.Add(reader.GetString(0));

        return dependencies;
    }

    public static void Close()
    {
        _connection.Close();
    }
}
