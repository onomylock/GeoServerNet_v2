using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Shared.Common.AuthenticationSchemeOptions;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.AuthenticationHandlers;

public class MasterServerJsonWebTokenAuthenticationHandler(
    IOptionsMonitor<JsonWebTokenAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<JsonWebTokenAuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>();

        var executingEndpoint = Context.GetEndpoint();

        if (executingEndpoint == null)
            return Task.FromResult(AuthenticateResult.Fail(new NullReferenceException(nameof(executingEndpoint))));

        if (executingEndpoint.Metadata.OfType<AllowAnonymousAttribute>().Any()
            || executingEndpoint.Metadata.OfType<AllowAnonymousAttribute>().Any())
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), Scheme.Name)));

        var authorizationBearerPayloads = new[]
        {
            Context.Request.Headers[HeaderNames.Authorization].SingleOrDefault()?.Split(" ").Last(),
            Context.Request.Cookies[CookieKey.JsonWebToken],
            Context.Request.Query.SingleOrDefault(_ => _.Key == CookieKey.JsonWebToken).Value.ToString()
        };

        authorizationBearerPayloads = authorizationBearerPayloads.Where(_ => !string.IsNullOrEmpty(_)).ToArray();

        if (authorizationBearerPayloads.Length > 0)
        {
            string authorizationBearerPayload = null;

            foreach (var authorizationBearerPayloadTemp in authorizationBearerPayloads)
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(authorizationBearerPayloadTemp, Options.TokenValidationParameters, out var validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    claims.AddRange(jwtToken.Claims);

                    authorizationBearerPayload = authorizationBearerPayloadTemp;
                    break;
                }
                catch (Exception)
                {
                    // ignored
                }

            if (!string.IsNullOrEmpty(authorizationBearerPayload)) claims.Add(new Claim(ClaimKey.JsonWebToken, authorizationBearerPayload, ClaimValueTypes.String));
        }

        var claimsIdentity = new ClaimsIdentity(claims, nameof(MasterServerJsonWebTokenAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}