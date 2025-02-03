using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Common.AuthenticationSchemeOptions;
using Shared.Common.Models;

namespace Shared.Common.AuthenticationHandlers;

public class DefaultAuthenticationHandler(
    IOptionsMonitor<DefaultAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<DefaultAuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimKey.IsPublic, true.ToString(), ClaimValueTypes.Boolean)
        }, nameof(DefaultAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}