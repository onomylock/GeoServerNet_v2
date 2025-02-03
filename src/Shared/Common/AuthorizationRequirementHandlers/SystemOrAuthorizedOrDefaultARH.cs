using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shared.Application.Services;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.Models;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class SystemOrAuthorizedOrDefaultARH
{
    public class System : AuthorizationHandler<SystemOrAuthorizedOrDefaultAR>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SystemOrAuthorizedOrDefaultAR authorizationRequirement
        )
        {
            if (context.HasSucceeded)
                return Task.CompletedTask;

            if (context.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.AccessToken)?.Value == authorizationRequirement.AccessToken)
                context.Succeed(authorizationRequirement);

            return Task.CompletedTask;
        }
    }

    public class Default : AuthorizationHandler<SystemOrAuthorizedOrDefaultAR>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SystemOrAuthorizedOrDefaultAR requirement)
        {
            if (context.HasSucceeded)
                return Task.CompletedTask;

            var isPublicClaim = context.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.IsPublic);

            if (isPublicClaim is { ValueType: ClaimValueTypes.Boolean } && isPublicClaim.Value == true.ToString()) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class Authorized(IJsonWebTokenAdvancedService jsonWebTokenAdvancedService) : AuthorizationHandler<SystemOrAuthorizedOrDefaultAR>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SystemOrAuthorizedOrDefaultAR requirement)
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