using Microsoft.AspNetCore.Authorization;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.Models;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class SystemARH : AuthorizationHandler<SystemAR>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SystemAR authorizationRequirement
    )
    {
        if (context.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.AccessToken)?.Value == authorizationRequirement.AccessToken)
            context.Succeed(authorizationRequirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}