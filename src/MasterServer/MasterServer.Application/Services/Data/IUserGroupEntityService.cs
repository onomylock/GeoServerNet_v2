using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;
using Shared.Common.Models;

namespace MasterServer.Application.Services.Data;

public interface IUserGroupEntityService : IEntityServiceBase<UserGroup>
{
    Task<UserGroup> GetByAliasAsync(string alias, bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<(int total, IReadOnlyCollection<UserGroup> entities)> GetByUserIdAsync(
        Guid userId,
        PageModel pageModel,
        Func<IQueryable<UserGroup>, IQueryable<UserGroup>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}