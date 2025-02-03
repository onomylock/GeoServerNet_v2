using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class SystemOrAuthorizedOrDefaultAR(string accessToken) : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.AccessToken,
        Models.AuthenticationSchemes.JsonWebToken,
        Models.AuthenticationSchemes.Default
    ];

    public string AccessToken { get; } = accessToken;
}