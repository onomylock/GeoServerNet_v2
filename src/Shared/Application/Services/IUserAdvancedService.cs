namespace Shared.Application.Services;

public interface IUserAdvancedService
{
    Guid GetUserIdFromHttpContext(bool throwIfNotProvided);
    Task<bool> IsInUserGroupByUserGroupId(Guid userGroup, Guid userGroupId, CancellationToken cancellationToken = default);
}