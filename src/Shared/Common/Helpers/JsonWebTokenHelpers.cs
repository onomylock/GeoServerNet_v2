using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Common.Helpers;

public static class JsonWebTokenHelpers
{
    public static string CreateWithClaims(
        string issuerSigningKey,
        string issuer,
        string audience,
        IEnumerable<Claim> claims,
        DateTime expires
    )
    {
        var symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(issuer, audience,
            claims ?? new List<Claim>(), expires: expires, signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}