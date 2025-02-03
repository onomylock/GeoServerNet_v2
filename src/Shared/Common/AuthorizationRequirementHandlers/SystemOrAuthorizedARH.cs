using Microsoft.AspNetCore.Authorization;
using Shared.Application.Services;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.Models;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class SystemOrAuthorizedARH
{
    public class System : AuthorizationHandler<SystemOrAuthorizedAR>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SystemOrAuthorizedAR authorizationRequirement
        )
        {
            if (context.HasSucceeded)
                return Task.CompletedTask;

            if (context.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.AccessToken)?.Value == authorizationRequirement.AccessToken)
                context.Succeed(authorizationRequirement);

            return Task.CompletedTask;
        }
    }

    public class Authorized(IJsonWebTokenAdvancedService jsonWebTokenAdvancedService) : AuthorizationHandler<SystemOrAuthorizedAR>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SystemOrAuthorizedAR requirement)
        {
            if (context.HasSucceeded)
                return;

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
}