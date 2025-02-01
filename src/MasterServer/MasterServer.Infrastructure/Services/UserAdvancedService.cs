using MasterServer.Application.Services.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Application.Services;
using Shared.Common.Exceptions;
using Shared.Common.Models;
using Shared.Common.Models.Options;

namespace MasterServer.Infrastructure.Services;

public class UserAdvancedService(
    IUserToUserGroupMappingEntityService userToUserGroupMappingEntityService,
    IHttpContextAccessor httpContextAccessor,
    IOptions<CommonServiceOptions> commonServiceOptions
) : IUserAdvancedService
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;

    public Guid GetUserIdFromHttpContext(bool throwIfNotProvided)
    {
        Guid resultUserId = default;

        if (!Guid.TryParse(_httpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.UserId)?.Value, out var userId))
        {
            //System Access Token acts as a RootUser
            if (_httpContext.User.Claims.Any(_ => _.Type == ClaimKey.AccessToken && _.Value == commonServiceOptions.Value.SystemAccessToken))
                resultUserId = Consts.RootUserId;

            if (bool.TryParse(_httpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimKey.IsPublic)?.Value, out var isPublic))
                resultUserId = Consts.PublicUserId;
        }
        else
        {
            resultUserId = userId;
        }

        if (resultUserId == default && throwIfNotProvided)
            throw new HttpContextMissingClaimsException(ClaimKey.UserId);

        return resultUserId;
    }

    public async Task<bool> IsInUserGroupByUserGroupId(Guid userId, Guid userGroupId, CancellationToken cancellationToken = default)
    {
        //Avoid potential deadlocks between AuthService and UserService (exit early)
        if (userId == Consts.RootUserId && userGroupId == Consts.RootUserGroupId)
            return true;

        return await userToUserGroupMappingEntityService.GetByEntityLeftIdEntityRightIdAsync(userId, userGroupId, true, cancellationToken) is { };
    }
}