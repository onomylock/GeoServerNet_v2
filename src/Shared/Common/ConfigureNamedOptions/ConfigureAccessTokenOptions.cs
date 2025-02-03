using Microsoft.Extensions.Options;
using Shared.Common.AuthenticationSchemeOptions;

namespace Shared.Common.ConfigureNamedOptions;

public class ConfigureAccessTokenOptions : IConfigureNamedOptions<AccessTokenAuthenticationSchemeOptions>
{
    public void Configure(string name, AccessTokenAuthenticationSchemeOptions options)
    {
        //ignored
    }

    public void Configure(AccessTokenAuthenticationSchemeOptions options)
    {
        Configure(string.Empty, options);
    }
}