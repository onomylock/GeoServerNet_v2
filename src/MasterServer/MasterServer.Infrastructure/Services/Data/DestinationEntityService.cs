using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class DestinationEntityService(IMasterServerRepository<Destination> entityRepository) : IDestinationEntityService
{
    public Task<Destination> AddAsync(Destination entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Destination> SaveAsync(Destination entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(Destination entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Destination> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<Destination>, IQueryable<Destination>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<Destination>, SetPropertyCalls<Destination>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Destination>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls,
            cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<Destination>, IQueryable<Destination>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<Destination>> SaveAsync(IEnumerable<Destination> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<Destination> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Destination>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<Destination> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<Destination>, IQueryable<Destination>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Destination>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<Destination> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<Destination>, IQueryable<Destination>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Destination>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<Destination> GetByAliasAsync(string alias, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}