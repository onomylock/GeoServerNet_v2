using Microsoft.AspNetCore.Authorization;
using Shared.Application.Services;
using Shared.Common.AuthorizationRequirement;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class AuthorizedARH(IJsonWebTokenAdvancedService jsonWebTokenAdvancedService) : AuthorizationHandler<AuthorizedAR>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedAR requirement)
    {
        try
        {
            _ = await jsonWebTokenAdvancedService.GetTokenFromHttpContext(true, true);

            context.Succeed(requirement);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}