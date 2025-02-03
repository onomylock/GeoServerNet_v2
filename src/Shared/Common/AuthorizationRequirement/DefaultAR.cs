using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class DefaultAR : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.Default
    ];
}