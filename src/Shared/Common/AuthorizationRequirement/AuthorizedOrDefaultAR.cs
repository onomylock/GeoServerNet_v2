using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class AuthorizedOrDefaultAR : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.JsonWebToken,
        Models.AuthenticationSchemes.Default
    ];
}