namespace NodeServer.Infrastructure.Helpers;

public static class FileHelper
{
    public const string HomeDirectory = ".geoServer";
    public static readonly string HomePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), HomeDirectory);
    public static readonly string BuildsPath = Path.Join(HomePath, "Builds");
    public static readonly string ResultsPath = Path.Join(HomePath, "Results");

    public static void EnsureCreated()
    {
        if(!Directory.Exists(HomePath))
            Directory.CreateDirectory(HomePath);
        
        if(!Directory.Exists(BuildsPath))
            Directory.CreateDirectory(BuildsPath);
    }

    public static DirectoryInfo CreateSolutionResultsPath(string solutionPath) =>
        Directory.CreateDirectory(Path.Join(ResultsPath, Path.GetDirectoryName(solutionPath)));

    public static DirectoryInfo CreateTmpDirectory(string resultsPath, Guid solutionId) =>
        Directory.CreateDirectory(Path.Combine(resultsPath,
            solutionId.ToString() + DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
}