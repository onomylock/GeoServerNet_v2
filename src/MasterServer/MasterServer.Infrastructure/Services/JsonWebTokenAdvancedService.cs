using MasterServer.Application.Services.Data;
using Microsoft.AspNetCore.Http;
using Shared.Application.Services;
using Shared.Common.Exceptions;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Services;

public class JsonWebTokenAdvancedService(
    IHttpContextAccessor httpContextAccessor,
    IJsonWebTokenRevokedEntityService jsonWebTokenRevokedEntityService
) : IJsonWebTokenAdvancedService
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;

    public async Task<string> GetTokenFromHttpContext(bool throwIfNotProvided, bool throwIfExpiredOrRevoked, CancellationToken cancellationToken = default)
    {
        if (_httpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.JsonWebToken)?.Value is var jsonWebTokenString &&
            string.IsNullOrEmpty(jsonWebTokenString))
            return throwIfNotProvided
                ? throw new JsonWebTokenNotFoundOrHttpContextMissingClaims()
                : null;

        if (throwIfExpiredOrRevoked)
        {
            var expiresAt = GetExpiresAtFromHttpContext(throwIfNotProvided);
            var isRevoked = await GetIsRevokedFromHttpContext(throwIfNotProvided, cancellationToken);

            if (expiresAt < DateTimeOffset.UtcNow || isRevoked)
                throw new JsonWebTokenExpiredException();

            if (isRevoked)
                throw new JsonWebTokenRevokedException();
        }

        return jsonWebTokenString;
    }

    public DateTimeOffset GetExpiresAtFromHttpContext(bool throwIfNotProvided)
    {
        if (!int.TryParse(_httpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.ExpiresAt)?.Value, out var expiresAt))
            return throwIfNotProvided
                ? throw new JsonWebTokenNotFoundOrHttpContextMissingClaims()
                : default;

        return DateTimeOffset.FromUnixTimeSeconds(expiresAt);
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<bool> GetIsRevokedFromHttpContext(bool throwIfNotProvided, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        var jsonWebTokenId = GetIdFromHttpContext(true);
        
        return await jsonWebTokenRevokedEntityService.GetByJsonWebTokenId(jsonWebTokenId, true, cancellationToken) is { };
    }

    public Guid GetIdFromHttpContext(bool throwIfNotProvided)
    {
        if (!Guid.TryParse(_httpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.JsonWebTokenId)?.Value, out var jsonWebTokenId))
            return throwIfNotProvided
                ? throw new JsonWebTokenNotFoundOrHttpContextMissingClaims()
                : Guid.Empty;

        return jsonWebTokenId;
    }
}