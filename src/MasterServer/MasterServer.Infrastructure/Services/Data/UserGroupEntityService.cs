using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class UserGroupEntityService(
    IMasterServerRepository<UserGroup> entityRepository,
    IMasterServerRepository<UserToUserGroupMapping> userToUserGroupMappingRepository
) : IUserGroupEntityService
{
    public Task<UserGroup> AddAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<UserGroup> SaveAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<UserGroup> GetByIdAsync(Guid id, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<UserGroup>, IQueryable<UserGroup>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<UserGroup>, SetPropertyCalls<UserGroup>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserGroup>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls,
            cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<UserGroup>, IQueryable<UserGroup>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.BulkDelete(entityRepository, queryTransformationFunction,
            cancellationToken);
    }

    public Task<IReadOnlyCollection<UserGroup>> SaveAsync(IEnumerable<UserGroup> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<UserGroup> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserGroup>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserGroup> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<UserGroup>, IQueryable<UserGroup>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserGroup>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<UserGroup> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<UserGroup>, IQueryable<UserGroup>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserGroup>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<UserGroup> GetByAliasAsync(string alias, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return entityRepository.Query(asNoTracking).SingleOrDefaultAsync(_ => _.Alias == alias, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserGroup> entities)> GetByUserIdAsync(
        Guid userId,
        PageModel pageModel,
        Func<IQueryable<UserGroup>, IQueryable<UserGroup>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return GetCollection(pageModel, query =>
        {
            return queryTransformationFunction(
                query.Join(
                    userToUserGroupMappingRepository.Query(asNoTracking)
                        .Where(userToUserGroupMapping => userToUserGroupMapping.EntityLeftId == userId),
                    userGroup => userGroup.Id,
                    userToUserGroupMapping => userToUserGroupMapping.EntityRightId,
                    (userGroup, userToUserGroupMapping) => userGroup
                )
            );
        }, asNoTracking, cancellationToken);
    }
}