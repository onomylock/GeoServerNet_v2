using Microsoft.Extensions.Options;
using Shared.Common.AuthenticationSchemeOptions;

namespace Shared.Common.ConfigureNamedOptions;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JsonWebTokenAuthenticationSchemeOptions>
{
    public void Configure(string name, JsonWebTokenAuthenticationSchemeOptions options)
    {
        options.TokenValidationParameters = null!;
    }

    public void Configure(JsonWebTokenAuthenticationSchemeOptions options)
    {
        Configure(string.Empty, options);
    }
}