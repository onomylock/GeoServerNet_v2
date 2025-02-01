using System.Security.Claims;
using System.Text;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.Auth;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Common.Helpers;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;
using Shared.Common.Models.Options;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands;

public abstract class AuthSingInHandlerBase(
    IHttpContextAccessor httpContextAccessor,
    IOptions<JsonWebTokenOptions> jsonWebTokenOptions,
    IOptions<MasterServiceOptions> masterServiceOptions,
    IRefreshTokenEntityService refreshTokenEntityService)
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;
    
    protected async Task<(AuthSignInResultBaseDto result, IEnumerable<ErrorBase> errors)> SignInBase(
        AuthSignInRequestBaseDto data, 
        Domain.Entities.User user,
        ClaimEntry[] claims, 
        CancellationToken cancellationToken = default)
    {
        var errors = new List<ErrorBase>();
        
        //GRPC does not have cookies
        var useCookies = data.UseCookies && _httpContext.Request is not { Protocol: "HTTP/2", ContentType: "application/grpc" };
        
        var customPasswordHasher = new CustomPasswordHasher();

        var dateTimeOffsetUtcNow = DateTimeOffset.UtcNow;
        
        if (!customPasswordHasher.VerifyPassword(user.PasswordHashed, data.Password))
            throw new IncorrectCredentialsException();

        //Create new RefreshToken
        var refreshTokenString = Convert.ToBase64String(CommonHelpers.GenerateRandomBytes(256));
        var refreshTokenExpiresAt = data.RefreshTokenExpireAt;
        var refreshTokenNew = await refreshTokenEntityService.SaveAsync(new RefreshToken
        {
            Token = Encoding.UTF8.GetBytes(refreshTokenString).ComputeSha256(),
            ExpiresAt = refreshTokenExpiresAt,
            UserId = user.Id
        }, cancellationToken);
        
        //Create new JsonWebToken
        var jsonWebTokenIdNew = Guid.NewGuid();
        
        var claimsToAdd = new List<Claim>
        {
            new(ClaimKey.UserId, user.Id.ToString(), ClaimValueTypes.String),
            new(ClaimKey.JsonWebTokenId, jsonWebTokenIdNew.ToString(), ClaimValueTypes.String)
        };
        
        if (claims?.Length > 0)
            claimsToAdd.AddRange(claims.Select(_ => new Claim(_.Type, _.Value, _.ValueType)));
        
        var jsonWebTokenExpiresAt = dateTimeOffsetUtcNow.AddSeconds(jsonWebTokenOptions.Value.ExpirySeconds);
        var jsonWebTokenString = JsonWebTokenHelpers.CreateWithClaims(jsonWebTokenOptions.Value.IssuerSigningKey,
            jsonWebTokenOptions.Value.Issuer, jsonWebTokenOptions.Value.Audience, claimsToAdd, jsonWebTokenExpiresAt.UtcDateTime);

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
            _httpContext.Response.Cookies.Append(CookieKey.RefreshTokenExpiresAt, refreshTokenExpiresAt.ToString("o"), new CookieOptions
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
            _httpContext.Response.Cookies.Append(CookieKey.JsonWebTokenExpiresAt, jsonWebTokenExpiresAt.ToString("o"), new CookieOptions
            {
                Expires = refreshTokenExpiresAt,
                Secure = masterServiceOptions.Value.SecureCookies,
                SameSite = SameSiteMode.Strict,
                HttpOnly = false,
                Domain = masterServiceOptions.Value.CookiesDomain
            });
            _httpContext.Response.Cookies.Append(CookieKey.UserId, user.Id.ToString(), new CookieOptions
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
            new AuthSignInResultBaseDto(userId: user.Id, jsonWebToken: !useCookies ? jsonWebTokenString : null,
                jsonWebTokenExpiresAt: !useCookies ? jsonWebTokenExpiresAt : null,
                refreshToken: !useCookies ? refreshTokenString : null,
                refreshTokenExpiresAt: !useCookies ? refreshTokenExpiresAt : null),
            errors);
    }
    
    
}