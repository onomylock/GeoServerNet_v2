namespace NodeServer.Infrastructure.Helpers;

public static class FileHelper
{
    public const string HomeDirectory = ".geoServer";
    public static readonly string HomePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), HomeDirectory);
    public static readonly string BuildsPath = Path.Join(HomePath, "Builds");
    public static readonly string ResultsPath = Path.Join(HomePath, "Results");
    public static readonly string WorkingDirectory = Path.Join(HomePath, "Working");
    
    public static void EnsureCreated()
    {
        if(!Directory.Exists(HomePath))
            Directory.CreateDirectory(HomePath);
        
        if(!Directory.Exists(BuildsPath))
            Directory.CreateDirectory(BuildsPath);
        
        if(!Directory.Exists(ResultsPath))
            Directory.CreateDirectory(ResultsPath);
        
        if(!Directory.Exists(WorkingDirectory))
            Directory.CreateDirectory(WorkingDirectory);
    }

    public static DirectoryInfo CreateSolutionResultsPath(string solutionPath) =>
        Directory.CreateDirectory(Path.Join(ResultsPath, Path.GetDirectoryName(solutionPath)));

    public static DirectoryInfo CreateResultPath(string resultsPath, Guid solutionId) =>
        Directory.CreateDirectory(Path.Combine(resultsPath,
            solutionId.ToString() + DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
    
    public static DirectoryInfo CreateWorkingPath(string workingPath) =>
        Directory.CreateDirectory(Path.Join(WorkingDirectory, workingPath + DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
}