using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shared.Application.Services;
using Shared.Common.AuthorizationRequirement;
using Shared.Common.Models;

namespace Shared.Common.AuthorizationRequirementHandlers;

public class AuthorizedOrDefaultARH
{
    public class Default : AuthorizationHandler<AuthorizedOrDefaultAR>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AuthorizedOrDefaultAR requirement)
        {
            if (context.HasSucceeded)
                return Task.CompletedTask;

            var isPublicClaim = context.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.IsPublic);

            if (isPublicClaim is { ValueType: ClaimValueTypes.Boolean } && isPublicClaim.Value == true.ToString())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class Authorized(IJsonWebTokenAdvancedService jsonWebTokenAdvancedService)
        : AuthorizationHandler<AuthorizedOrDefaultAR>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AuthorizedOrDefaultAR requirement)
        {
            if (context.HasSucceeded)
                return;

            /*
             * Use services, to find whether is on revoke list, has expired, or is missing in claims
             *
             * Better avoid failing the context when using multiple handlers, to invoke next handlers that might succeed
             * Failing a context, stops pipeline
             */
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