using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class AuthorizedExpiredAR : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.JsonWebTokenExpired
    ];
}