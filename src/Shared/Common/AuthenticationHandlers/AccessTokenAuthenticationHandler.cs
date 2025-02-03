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

namespace Shared.Common.AuthenticationHandlers;

public class AccessTokenAuthenticationHandler(
    IOptionsMonitor<AccessTokenAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AccessTokenAuthenticationSchemeOptions>(options, logger, encoder)
{
#pragma warning disable CS1998
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma warning restore CS1998
    {
        var claims = new List<Claim>();

        var executingEndpoint = Context.GetEndpoint();

        if (executingEndpoint == null)
            return AuthenticateResult.Fail(new NullReferenceException(nameof(executingEndpoint)));

        if (executingEndpoint.Metadata.OfType<AllowAnonymousAttribute>().Any()
            || executingEndpoint.Metadata.OfType<AllowAnonymousAttribute>().Any())
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), Scheme.Name));

        var authorizationBearerPayloads = new[]
        {
            Context.Request.Headers[HeaderNames.Authorization].SingleOrDefault()?.Split(" ").Last(),
            Context.Request.Cookies[CookieKey.AccessToken],
            Context.Request.Query.SingleOrDefault(_ => _.Key == CookieKey.AccessToken).Value.ToString()
        };

        authorizationBearerPayloads = authorizationBearerPayloads.Where(_ => !string.IsNullOrEmpty(_)).ToArray();

        if (authorizationBearerPayloads.Length > 0)
        {
            var accessToken = authorizationBearerPayloads.First();

            if (!string.IsNullOrEmpty(accessToken)) claims.Add(new Claim(ClaimKey.AccessToken, accessToken, ClaimValueTypes.String));
        }

        var claimsIdentity = new ClaimsIdentity(claims, nameof(AccessTokenAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}