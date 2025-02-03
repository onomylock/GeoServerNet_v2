using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class SystemOrAuthorizedAR(string accessToken) : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.AccessToken,
        Models.AuthenticationSchemes.JsonWebToken
    ];

    public string AccessToken { get; } = accessToken;
}