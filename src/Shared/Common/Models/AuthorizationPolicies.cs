namespace Shared.Common.Models;

public static class AuthorizationPolicies
{
    public const string Default = "Default";
    public const string System = "System";
    public const string Authorized = "Authorized";
    public const string AuthorizedExpired = "AuthorizedExpired";
    public const string AuthorizedOrDefault = "AuthorizedOrDefault";
    public const string SystemOrAuthorized = "SystemOrAuthorized";
    public const string SystemOrAuthorizedOrDefault = "SystemOrAuthorizedOrDefault";
}