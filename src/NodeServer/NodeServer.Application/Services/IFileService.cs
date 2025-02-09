namespace NodeServer.Application.Services;

public interface IFileService
{
    Task<string> ExtractArchiveAsync(Stream stream, string destinationPath,
        CancellationToken cancellationToken = default);

    Task<Stream> ZipFolderAsync(string sourcePath, CancellationToken cancellationToken = default);
    Task DeleteFolderAsync(string path, CancellationToken cancellationToken = default);
}