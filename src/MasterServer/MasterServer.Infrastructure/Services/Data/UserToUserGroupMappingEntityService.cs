using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Extensions;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class UserToUserGroupMappingEntityService(
    IMasterServerRepository<UserToUserGroupMapping> entityRepository
) : IUserToUserGroupMappingEntityService
{
    public Task<UserToUserGroupMapping> AddAsync(UserToUserGroupMapping entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToUserGroupMapping>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<UserToUserGroupMapping> SaveAsync(UserToUserGroupMapping entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToUserGroupMapping>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(UserToUserGroupMapping entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToUserGroupMapping>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<UserToUserGroupMapping> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToUserGroupMapping>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<UserToUserGroupMapping>, IQueryable<UserToUserGroupMapping>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<UserToUserGroupMapping>, SetPropertyCalls<UserToUserGroupMapping>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToUserGroupMapping>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(
        Func<IQueryable<UserToUserGroupMapping>, IQueryable<UserToUserGroupMapping>> queryTransformationFunction,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToUserGroupMapping>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<UserToUserGroupMapping>> SaveAsync(IEnumerable<UserToUserGroupMapping> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToUserGroupMapping>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<UserToUserGroupMapping> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToUserGroupMapping>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserToUserGroupMapping> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<UserToUserGroupMapping>, IQueryable<UserToUserGroupMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToUserGroupMapping>.GetCollection(entityRepository, pageModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<UserToUserGroupMapping> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<UserToUserGroupMapping>, IQueryable<UserToUserGroupMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToUserGroupMapping>.GetCollection(entityRepository, cursorModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<UserToUserGroupMapping> GetByEntityLeftIdEntityRightIdAsync(
        Guid entityLeftId,
        Guid entityRightId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<UserToUserGroupMapping>.GetByEntityLeftIdEntityRightIdAsync(entityRepository,
            entityLeftId, entityRightId, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserToUserGroupMapping> entities)> GetByEntityLeftIdAsync(
        Guid entityLeftId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<UserToUserGroupMapping>.GetByEntityLeftIdAsync(entityRepository, entityLeftId,
            pageModel, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserToUserGroupMapping> entities)> GetByEntityRightIdAsync(
        Guid entityRightId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<UserToUserGroupMapping>.GetByEntityRightIdAsync(entityRepository, entityRightId,
            pageModel, asNoTracking, cancellationToken);
    }
}