using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignOutCommand;

public class AuthSignOutHandler(
    IValidator<AuthSignOutCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IUserAdvancedService userAdvancedService,
    IHttpContextAccessor httpContextAccessor,
    IJsonWebTokenAdvancedService jsonWebTokenAdvancedService,
    IRefreshTokenEntityService refreshTokenEntityService,
    IJsonWebTokenRevokedEntityService jsonWebTokenRevokedEntityService,
    IOptions<MasterServerOptions> masterServerOptions
) : IRequestHandler<AuthSignOutCommand, ResponseBase<OkResult>>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;

    public async Task<ResponseBase<OkResult>> Handle(AuthSignOutCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var userId = userAdvancedService.GetUserIdFromHttpContext(true);

            //GRPC does not have cookies
            var useCookies = string.IsNullOrEmpty(request.RefreshToken) && _httpContext.Request is not
                { Protocol: "HTTP/2", ContentType: "application/grpc" };

            request.RefreshToken ??= _httpContext.Request.Cookies[CookieKey.RefreshToken];
            if (string.IsNullOrEmpty(request.RefreshToken))
                throw new RefreshTokenNotProvidedException();

            //Find old RefreshToken
            var refreshTokenOld =
                await refreshTokenEntityService.GetByTokenAsync(request.RefreshToken,
                    cancellationToken: cancellationToken);
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

            //Delete old RefreshToken
            await refreshTokenEntityService.DeleteAsync(refreshTokenOld, cancellationToken);

            if (useCookies)
            {
                _httpContext.Response.Cookies.Delete(CookieKey.JsonWebToken, new CookieOptions
                {
                    Secure = masterServerOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = true,
                    Domain = masterServerOptions.Value.CookiesDomain
                });
                _httpContext.Response.Cookies.Delete(CookieKey.JsonWebTokenExpiresAt, new CookieOptions
                {
                    Secure = masterServerOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = false,
                    Domain = masterServerOptions.Value.CookiesDomain
                });
                _httpContext.Response.Cookies.Delete(CookieKey.RefreshToken, new CookieOptions
                {
                    Secure = masterServerOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = true,
                    Domain = masterServerOptions.Value.CookiesDomain
                });
                _httpContext.Response.Cookies.Delete(CookieKey.RefreshTokenExpiresAt, new CookieOptions
                {
                    Secure = masterServerOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = false,
                    Domain = masterServerOptions.Value.CookiesDomain
                });
                _httpContext.Response.Cookies.Delete(CookieKey.UserId, new CookieOptions
                {
                    Secure = masterServerOptions.Value.SecureCookies,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = false,
                    Domain = masterServerOptions.Value.CookiesDomain
                });
            }

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<OkResult>
            {
                Data = new OkResult()
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);

            throw;
        }
    }
}