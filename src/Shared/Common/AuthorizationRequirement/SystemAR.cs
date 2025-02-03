using Microsoft.AspNetCore.Authorization;

namespace Shared.Common.AuthorizationRequirement;

public class SystemAR(string accessToken) : IAuthorizationRequirement
{
    public static readonly List<string> AuthenticationSchemes =
    [
        Models.AuthenticationSchemes.AccessToken
    ];

    public string AccessToken { get; } = accessToken;
}