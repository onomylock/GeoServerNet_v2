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
using Shared.Common.Models.Options;

namespace Shared.Common.AuthenticationHandlers;

public class JsonWebTokenExpiredAuthenticationHandler(
    IOptionsMonitor<JsonWebTokenAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AuthServiceAuthGrpc.AuthServiceAuthGrpcClient authServiceAuthGrpcClient,
    IOptions<CommonServiceOptions> commonServiceOptions,
    IHttpContextAccessor httpContextAccessor)
    : AuthenticationHandler<JsonWebTokenAuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
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
                    var validateJwtResult = await authServiceAuthGrpcClient.ValidateJwtAsync(new AuthValidateJwtRequest
                    {
                        Token = authorizationBearerPayloadTemp,
                        AuthenticationScheme = AuthenticationSchemes.JsonWebTokenExpired
                    }, headers: new Metadata
                    {
                        { "Authorization", $"Bearer {commonServiceOptions.Value.SystemAccessToken}" }
                    }, cancellationToken: httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None);

                    claims.AddRange(validateJwtResult.Claims.Select(_ => new Claim(_.Type, _.Value, _.ValueType)));

                    authorizationBearerPayload = authorizationBearerPayloadTemp;
                    break;
                }
                catch (Exception)
                {
                    // ignored
                }

            if (!string.IsNullOrEmpty(authorizationBearerPayload)) claims.Add(new Claim(ClaimKey.JsonWebToken, authorizationBearerPayload, ClaimValueTypes.String));
        }

        var claimsIdentity = new ClaimsIdentity(claims, nameof(JsonWebTokenAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}