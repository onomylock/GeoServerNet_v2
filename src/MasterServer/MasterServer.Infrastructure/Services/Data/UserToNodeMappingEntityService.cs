using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Extensions;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class UserToNodeMappingEntityService(
    IMasterServerRepository<UserToNodeMapping> entityRepository
) : IUserToNodeMappingEntityService
{
    public Task<UserToNodeMapping> AddAsync(UserToNodeMapping entity,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToNodeMapping>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<UserToNodeMapping> SaveAsync(UserToNodeMapping entity,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToNodeMapping>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(UserToNodeMapping entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToNodeMapping>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<UserToNodeMapping> GetByIdAsync(Guid id, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToNodeMapping>.GetByIdAsync(entityRepository, id, asNoTracking,
            cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<UserToNodeMapping>, IQueryable<UserToNodeMapping>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<UserToNodeMapping>, SetPropertyCalls<UserToNodeMapping>>>
            setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToNodeMapping>.BulkUpdate(entityRepository, queryTransformationFunction,
            setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(
        Func<IQueryable<UserToNodeMapping>, IQueryable<UserToNodeMapping>> queryTransformationFunction,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToNodeMapping>.BulkDelete(entityRepository, queryTransformationFunction,
            cancellationToken);
    }

    public Task<IReadOnlyCollection<UserToNodeMapping>> SaveAsync(IEnumerable<UserToNodeMapping> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToNodeMapping>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<UserToNodeMapping> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<UserToNodeMapping>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserToNodeMapping> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<UserToNodeMapping>, IQueryable<UserToNodeMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToNodeMapping>.GetCollection(entityRepository, pageModel,
            queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<UserToNodeMapping> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<UserToNodeMapping>, IQueryable<UserToNodeMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<UserToNodeMapping>.GetCollection(entityRepository, cursorModel,
            queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<UserToNodeMapping> GetByEntityLeftIdEntityRightIdAsync(
        Guid entityLeftId,
        Guid entityRightId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<UserToNodeMapping>.GetByEntityLeftIdEntityRightIdAsync(
            entityRepository,
            entityLeftId, entityRightId, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserToNodeMapping> entities)> GetByEntityLeftIdAsync(
        Guid entityLeftId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<UserToNodeMapping>.GetByEntityLeftIdAsync(entityRepository,
            entityLeftId,
            pageModel, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<UserToNodeMapping> entities)> GetByEntityRightIdAsync(
        Guid entityRightId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<UserToNodeMapping>.GetByEntityRightIdAsync(entityRepository,
            entityRightId,
            pageModel, asNoTracking, cancellationToken);
    }
}