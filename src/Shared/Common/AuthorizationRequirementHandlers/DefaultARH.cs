using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.Models;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class DefaultARH : AuthorizationHandler<DefaultAR>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DefaultAR requirement
    )
    {
        var isPublicClaim = context.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.IsPublic);

        if (isPublicClaim is { ValueType: ClaimValueTypes.Boolean } && isPublicClaim.Value == true.ToString())
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}