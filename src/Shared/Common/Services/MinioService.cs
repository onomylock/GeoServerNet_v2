using System.Net.Mime;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Shared.Application.Services;
using Shared.Common.Models.Options;

namespace Shared.Common.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;

    public MinioService(IMinioClient minioClient, IOptions<MinioOptions> minioOptions)
    {
        var minioOptionsValue = minioOptions.Value;

        _minioClient = minioClient
            .WithSSL(minioOptionsValue.WithSsl);
    }

    public async Task SaveAsync(Stream file, string fileName, string bucketName, long objectSize,
        CancellationToken cancellationToken = default)
    {
        // Make a bucket on the server, if not already present.
        var beArgs = new BucketExistsArgs().WithBucket(bucketName);
        if (!await _minioClient.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false))
        {
            var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
        }

        //Get file content type
        var fectp = new FileExtensionContentTypeProvider();

        if (!fectp.TryGetContentType(fileName, out var fileContentType))
            fileContentType ??= MediaTypeNames.Application.Octet;

        //Upload file
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithStreamData(file)
            .WithObject(fileName)
            .WithContentType(fileContentType)
            .WithObjectSize(objectSize);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
    }

    public Task DeleteAsync(string fileName, string bucketName, CancellationToken cancellationToken = default)
    {
        var deleteObjectsArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);

        return _minioClient.RemoveObjectAsync(deleteObjectsArgs, cancellationToken);
    }

    public Task<string> GetFileUrl(string fileName, string bucketName, CancellationToken cancellationToken = default)
    {
        var getObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry((int)TimeSpan.FromMinutes(5).TotalSeconds);

        return _minioClient.PresignedGetObjectAsync(getObjectArgs);
    }
}