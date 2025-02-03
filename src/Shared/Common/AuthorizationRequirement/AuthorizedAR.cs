using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class AuthorizedAR : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.JsonWebToken
    ];
}