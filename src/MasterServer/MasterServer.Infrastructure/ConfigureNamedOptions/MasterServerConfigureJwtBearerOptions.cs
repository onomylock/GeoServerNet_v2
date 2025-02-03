using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.AuthenticationSchemeOptions;
using Shared.Common.Models;
using Shared.Common.Models.Options;

namespace MasterServer.Infrastructure.ConfigureNamedOptions;

public class MasterServerConfigureJwtBearerOptions(JsonWebTokenOptions jsonWebTokenOptions)
    : IConfigureNamedOptions<JsonWebTokenAuthenticationSchemeOptions>
{
    public void Configure(string name, JsonWebTokenAuthenticationSchemeOptions options)
    {
        options.TokenValidationParameters = name switch
        {
            AuthenticationSchemes.JsonWebToken => new TokenValidationParameters
            {
                ValidateIssuer = jsonWebTokenOptions.ValidateIssuer,
                ValidateAudience = jsonWebTokenOptions.ValidateAudience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = jsonWebTokenOptions.ValidateIssuerSigningKey,
                ValidIssuer = jsonWebTokenOptions.Issuer,
                ValidAudience = jsonWebTokenOptions.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokenOptions.IssuerSigningKey))
            },
            AuthenticationSchemes.JsonWebTokenExpired => new TokenValidationParameters
            {
                ValidateIssuer = jsonWebTokenOptions.ValidateIssuer,
                ValidateAudience = jsonWebTokenOptions.ValidateAudience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = jsonWebTokenOptions.ValidateIssuerSigningKey,
                ValidIssuer = jsonWebTokenOptions.Issuer,
                ValidAudience = jsonWebTokenOptions.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokenOptions.IssuerSigningKey))
            },
            _ => options.TokenValidationParameters
        };
    }

    public void Configure(JsonWebTokenAuthenticationSchemeOptions options)
    {
        Configure(string.Empty, options);
    }
}