using Microsoft.AspNetCore.Authorization;
using Shared.Application.Services;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.Exceptions;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class AuthorizedExpiredARH(IJsonWebTokenAdvancedService jsonWebTokenAdvancedService)
    : AuthorizationHandler<AuthorizedExpiredAR>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedExpiredAR requirement)
    {
        try
        {
            _ = await jsonWebTokenAdvancedService.GetTokenFromHttpContext(true, true);

            context.Succeed(requirement);
        }
        catch (JsonWebTokenExpiredException)
        {
            context.Succeed(requirement);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}