using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class ClusterEntityService(IMasterServerRepository<Cluster> entityRepository) : IClusterEntityService
{
    public Task<Cluster> AddAsync(Cluster entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Cluster> SaveAsync(Cluster entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(Cluster entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Cluster> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<Cluster>, SetPropertyCalls<Cluster>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Cluster>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls,
            cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<Cluster>> SaveAsync(IEnumerable<Cluster> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<Cluster> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Cluster>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<Cluster> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Cluster>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<Cluster> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Cluster>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<Cluster> GetByAliasAsync(string alias, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}