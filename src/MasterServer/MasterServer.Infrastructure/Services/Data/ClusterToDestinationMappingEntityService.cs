using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class ClusterToDestinationMappingEntityService(
    IMasterServerRepository<ClusterToDestinationMapping> entityRepository
) : IClusterToDestinationMappingEntityService
{
    public Task<ClusterToDestinationMapping> AddAsync(ClusterToDestinationMapping entity,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToDestinationMapping>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<ClusterToDestinationMapping> SaveAsync(ClusterToDestinationMapping entity,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToDestinationMapping>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(ClusterToDestinationMapping entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToDestinationMapping>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<ClusterToDestinationMapping> GetByIdAsync(Guid id, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToDestinationMapping>.GetByIdAsync(entityRepository, id, asNoTracking,
            cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<ClusterToDestinationMapping>, IQueryable<ClusterToDestinationMapping>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<ClusterToDestinationMapping>, SetPropertyCalls<ClusterToDestinationMapping>>>
            setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToDestinationMapping>.BulkUpdate(entityRepository, queryTransformationFunction,
            setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(
        Func<IQueryable<ClusterToDestinationMapping>, IQueryable<ClusterToDestinationMapping>> queryTransformationFunction,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToDestinationMapping>.BulkDelete(entityRepository, queryTransformationFunction,
            cancellationToken);
    }

    public Task<IReadOnlyCollection<ClusterToDestinationMapping>> SaveAsync(IEnumerable<ClusterToDestinationMapping> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToDestinationMapping>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<ClusterToDestinationMapping> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<ClusterToDestinationMapping>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<ClusterToDestinationMapping> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<ClusterToDestinationMapping>, IQueryable<ClusterToDestinationMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToDestinationMapping>.GetCollection(entityRepository, pageModel,
            queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<ClusterToDestinationMapping> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<ClusterToDestinationMapping>, IQueryable<ClusterToDestinationMapping>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<ClusterToDestinationMapping>.GetCollection(entityRepository, cursorModel,
            queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<ClusterToDestinationMapping> GetByEntityLeftIdEntityRightIdAsync(
        Guid entityLeftId,
        Guid entityRightId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<ClusterToDestinationMapping>.GetByEntityLeftIdEntityRightIdAsync(
            entityRepository,
            entityLeftId, entityRightId, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<ClusterToDestinationMapping> entities)> GetByEntityLeftIdAsync(
        Guid entityLeftId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<ClusterToDestinationMapping>.GetByEntityLeftIdAsync(entityRepository,
            entityLeftId,
            pageModel, asNoTracking, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<ClusterToDestinationMapping> entities)> GetByEntityRightIdAsync(
        Guid entityRightId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityToEntityMappingServiceBase<ClusterToDestinationMapping>.GetByEntityRightIdAsync(entityRepository,
            entityRightId,
            pageModel, asNoTracking, cancellationToken);
    }
}