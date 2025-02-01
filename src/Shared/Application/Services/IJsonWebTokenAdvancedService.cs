namespace Shared.Application.Services;

public interface IJsonWebTokenAdvancedService
{
    Task<string> GetTokenFromHttpContext(bool throwIfNotProvided, bool throwIfExpiredOrRevoked,
        CancellationToken cancellationToken = default);

    DateTimeOffset GetExpiresAtFromHttpContext(bool throwIfNotProvided);
    Task<bool> GetIsRevokedFromHttpContext(bool throwIfNotProvided, CancellationToken cancellationToken = default);
    public Guid GetIdFromHttpContext(bool throwIfNotProvided);
}