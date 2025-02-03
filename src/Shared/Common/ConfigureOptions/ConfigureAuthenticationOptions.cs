using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Shared.Common.Models;

namespace Shared.Common.ConfigureOptions;

public class ConfigureAuthenticationOptions : IConfigureOptions<AuthenticationOptions>
{
    public void Configure(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = AuthenticationSchemes.Default;
        options.DefaultChallengeScheme = null;
    }
}