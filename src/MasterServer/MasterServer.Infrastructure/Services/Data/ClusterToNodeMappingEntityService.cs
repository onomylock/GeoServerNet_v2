using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class ClusterToNodeMappingEntityService(
    IMasterServerRepository<ClusterToNodeMapping> entityRepository
) : IClusterToNodeMappingEntityService
{
    public Task<ClusterToNodeMapping> AddAsync(ClusterToNodeMapping entity,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToNodeMapping>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<ClusterToNodeMapping> SaveAsync(ClusterToNodeMapping entity,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToNodeMapping>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(ClusterToNodeMapping entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToNodeMapping>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<ClusterToNodeMapping> GetByIdAsync(Guid id, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToNodeMapping>.GetByIdAsync(entityRepository, id, asNoTracking,
            cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<ClusterToNodeMapping>, IQueryable<ClusterToNodeMapping>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<ClusterToNodeMapping>, SetPropertyCalls<ClusterToNodeMapping>>>
            setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToNodeMapping>.BulkUpdate(entityRepository, queryTransformationFunction,
            setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(
        Func<IQueryable<ClusterToNodeMapping>, IQueryable<ClusterToNodeMapping>> queryTransformationFunction,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToNodeMapping>.BulkDelete(entityRepository, queryTransformationFunction,
            cancellationToken);
    }

    public Task<IReadOnlyCollection<ClusterToNodeMapping>> SaveAsync(IEnumerable<ClusterToNodeMapping> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToNodeMapping>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<ClusterToNodeMapping> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToNodeMapping>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<ClusterToNodeMapping> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<ClusterToNodeMapping>, IQueryable<ClusterToNodeMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToNodeMapping>.GetCollection(entityRepository, pageModel,
            queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<ClusterToNodeMapping> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<ClusterToNodeMapping>, IQueryable<ClusterToNodeMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToNodeMapping>.GetCollection(entityRepository, cursorModel,
            queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<ClusterToNodeMapping> GetByEntityLeftIdEntityRightIdAsync(
        Guid entityLeftId,
        Guid entityRightId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<ClusterToNodeMapping>.GetByEntityLeftIdEntityRightIdAsync(
            entityRepository,
            entityLeftId, entityRightId, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<ClusterToNodeMapping> entities)> GetByEntityLeftIdAsync(
        Guid entityLeftId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<ClusterToNodeMapping>.GetByEntityLeftIdAsync(entityRepository,
            entityLeftId,
            pageModel, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<ClusterToNodeMapping> entities)> GetByEntityRightIdAsync(
        Guid entityRightId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<ClusterToNodeMapping>.GetByEntityRightIdAsync(entityRepository,
            entityRightId,
            pageModel, asNoTracking, cancellationToken);
    }
}