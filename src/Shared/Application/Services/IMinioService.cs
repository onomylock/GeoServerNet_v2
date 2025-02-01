namespace Shared.Application.Services;

public interface IMinioService
{
    public Task SaveAsync(Stream file, string fileName, string bucketName, long objectSize, CancellationToken cancellationToken = default);
    Task DeleteAsync(string fileName, string bucketName, CancellationToken cancellationToken = default);
    Task<string> GetFileUrl(string fileName, string bucketName, CancellationToken cancellationToken = default);
}