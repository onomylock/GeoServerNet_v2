using System.Security.Claims;
using System.Text;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.Auth;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Application.Services;
using Shared.Common.Helpers;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;
using Shared.Common.Models.Options;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands;

public abstract class AuthRefreshHandlerBase(
    IHttpContextAccessor httpContextAccessor,
    IOptions<JsonWebTokenOptions> jsonWebTokenOptions,
    IOptions<MasterServerOptions> masterServiceOptions,
    IUserAdvancedService userAdvancedService,
    IJsonWebTokenAdvancedService jsonWebTokenAdvancedService,
    IJsonWebTokenRevokedEntityService jsonWebTokenRevokedEntityService,
    IRefreshTokenEntityService refreshTokenEntityService)
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;

    protected async Task<(AuthSignInResultBaseDto result, IEnumerable<ErrorBase> errors)> RefreshBase(
        AuthRefreshRequestBaseDto data, ClaimEntry[] claims,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<ErrorBase>();

        var userId = userAdvancedService.GetUserIdFromHttpContext(true);

        var dateTimeOffsetUtcNow = DateTimeOffset.UtcNow;

        //GRPC does not have cookies
        var useCookies = string.IsNullOrEmpty(data.RefreshToken) && _httpContext.Request is not
            { Protocol: "HTTP/2", ContentType: "application/grpc" };

        data.RefreshToken ??= _httpContext.Request.Cookies[CookieKey.RefreshToken];
        if (string.IsNullOrEmpty(data.RefreshToken))
            throw new RefreshTokenNotProvidedException();

        //Find old RefreshToken
        var refreshTokenOld =
            await refreshTokenEntityService.GetByTokenAsync(data.RefreshToken, cancellationToken: cancellationToken);
        if (refreshTokenOld == null)
            throw new RefreshTokenNotFoundException();

        if (refreshTokenOld.ExpiresAt < DateTimeOffset.UtcNow)
            throw new RefreshTokenExpiredException();

        //Find old JsonWebTokenId
        var jsonWebTokenIdOld = jsonWebTokenAdvancedService.GetIdFromHttpContext(true);

        //Revoke old JsonWebTokenId by creating JsonWebTokenRevoked
        await jsonWebTokenRevokedEntityService.SaveAsync(new JsonWebTokenRevoked
        {
            JsonWebTokenId = jsonWebTokenIdOld,
            ExpiresAt = jsonWebTokenAdvancedService.GetExpiresAtFromHttpContext(true)
        }, cancellationToken);

        //Create new RefreshToken
        var refreshTokenString = Convert.ToBase64String(CommonHelpers.GenerateRandomBytes(256));
        var refreshTokenExpiresAt = refreshTokenOld.ExpiresAt;
        var refreshTokenNew = await refreshTokenEntityService.SaveAsync(new RefreshToken
        {
            Token = Encoding.UTF8.GetBytes(refreshTokenString).ComputeSha256(),
            ExpiresAt = refreshTokenExpiresAt,
            UserId = userId
        }, cancellationToken);

        //Create new JsonWebToken
        var jsonWebTokenIdNew = Guid.NewGuid();

        var claimsToAdd = new List<Claim>
        {
            new(ClaimKey.UserId, userId.ToString(), ClaimValueTypes.String),
            new(ClaimKey.JsonWebTokenId, jsonWebTokenIdNew.ToString(), ClaimValueTypes.String)
        };

        if (claims.Length > 0)
            claimsToAdd.AddRange(claims.Select(_ => new Claim(_.Type, _.Value, _.ValueType)));

        var jsonWebTokenExpiresAt = dateTimeOffsetUtcNow.AddSeconds(jsonWebTokenOptions.Value.ExpirySeconds);
        var jsonWebTokenString = JsonWebTokenHelpers.CreateWithClaims(jsonWebTokenOptions.Value.IssuerSigningKey,
            jsonWebTokenOptions.Value.Issuer, jsonWebTokenOptions.Value.Audience, claimsToAdd,
            jsonWebTokenExpiresAt.UtcDateTime);

        //Delete old RefreshToken
        await refreshTokenEntityService.DeleteAsync(refreshTokenOld, cancellationToken);

        if (useCookies)
        {
            _httpContext.Response.Cookies.Append(CookieKey.RefreshToken, refreshTokenString, new CookieOptions
            {
                Expires = refreshTokenExpiresAt,
                Secure = masterServiceOptions.Value.SecureCookies,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Domain = masterServiceOptions.Value.CookiesDomain
            });
            _httpContext.Response.Cookies.Append(CookieKey.RefreshTokenExpiresAt, refreshTokenExpiresAt.ToString("o"),
                new CookieOptions
                {
                    Expires = refreshTokenExpiresAt,
                    Secure = masterServiceOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = false,
                    Domain = masterServiceOptions.Value.CookiesDomain
                });
            _httpContext.Response.Cookies.Append(CookieKey.JsonWebToken, jsonWebTokenString, new CookieOptions
            {
                Expires = refreshTokenExpiresAt,
                Secure = masterServiceOptions.Value.SecureCookies,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Domain = masterServiceOptions.Value.CookiesDomain
            });
            _httpContext.Response.Cookies.Append(CookieKey.JsonWebTokenExpiresAt, jsonWebTokenExpiresAt.ToString("o"),
                new CookieOptions
                {
                    Expires = refreshTokenExpiresAt,
                    Secure = masterServiceOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = false,
                    Domain = masterServiceOptions.Value.CookiesDomain
                });
            _httpContext.Response.Cookies.Append(CookieKey.UserId, userId.ToString(), new CookieOptions
            {
                Expires = refreshTokenExpiresAt,
                Secure = masterServiceOptions.Value.SecureCookies,
                SameSite = SameSiteMode.Strict,
                HttpOnly = false,
                Domain = masterServiceOptions.Value.CookiesDomain
            });
        }
        else
        {
            errors.Add(new ErrorBase
            {
                ErrorMessage = Localize.Keys.Warning.XssVulnerable
            });
        }

        return (
            new AuthSignInResultBaseDto
            {
                UserId = userId,
                JsonWebToken = !useCookies ? jsonWebTokenString : null,
                JsonWebTokenExpiresAt = !useCookies ? jsonWebTokenExpiresAt : null,
                RefreshToken = !useCookies ? refreshTokenString : null,
                RefreshTokenExpiresAt = !useCookies ? refreshTokenExpiresAt : null
            },
            errors);
    }
}