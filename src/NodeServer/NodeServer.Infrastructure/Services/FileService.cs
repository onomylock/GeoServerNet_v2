using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using NodeServer.Application.Services;
using Shared.Common.Helpers;

namespace NodeServer.Infrastructure.Services;

public class FileService : IFileService
{
    public async Task<string> ExtractArchiveAsync(Stream stream, string destinationPath, CancellationToken cancellationToken = default)
    {
        await using var gZipStream = new GZipInputStream(stream);
        await using var tarInputStream = new TarInputStream(gZipStream, Encoding.UTF8);
        
        if(string.IsNullOrEmpty(destinationPath))
            throw new ArgumentNullException(nameof(destinationPath));
        
        if(!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);
        
        while (await tarInputStream.GetNextEntryAsync(cancellationToken) is { } tarEntry)
        {
            if(tarEntry.IsDirectory)
                continue;

            await using var fs = new FileStream(Path.Combine(destinationPath, tarEntry.Name), FileMode.OpenOrCreate, FileAccess.Write);
            
            await tarInputStream.CopyEntryContentsAsync(fs, cancellationToken);
            fs.Seek(0, SeekOrigin.Begin);
        }

        return destinationPath;
    }

    public async Task<Stream> ZipFolderAsync(string sourcePath, CancellationToken cancellationToken = default)
    {
        var tempDirectory = Directory.CreateTempSubdirectory();
        
        CopyDirectory( sourcePath, tempDirectory.FullName, true);

        var memoryStream = new MemoryStream();
        var gZipOutputStream = new GZipOutputStream(memoryStream);
        gZipOutputStream.SetLevel(4);
        var tarArchive = TarArchive.CreateOutputTarArchive(gZipOutputStream);
        
        tarArchive.RootPath = tempDirectory.FullName;

        TarHelper.AddToTarManually(tarArchive, tempDirectory.FullName);

        tarArchive.IsStreamOwner = false;
        tarArchive.Close();

        gZipOutputStream.IsStreamOwner = false;
        await gZipOutputStream.FlushAsync(cancellationToken);
        await gZipOutputStream.FinishAsync(cancellationToken);

        await memoryStream.FlushAsync(cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);

        Directory.Delete(tempDirectory.FullName, true);

        return memoryStream;
    }

    public Task DeleteFolderAsync(string path, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        var dirs = dir.GetDirectories();

        // Get the files in the source directory and copy to the destination directory
        foreach (var file in dir.GetFiles())
        {
            var targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (!recursive) return;
        foreach (var subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir, true);
        }
    }
}