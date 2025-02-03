using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class NodeEntityService(IMasterServerRepository<Node> entityRepository) : INodeEntityService
{
    public Task<Node> AddAsync(Node entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Node> SaveAsync(Node entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(Node entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Node> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<Node>, IQueryable<Node>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<Node>, SetPropertyCalls<Node>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Node>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls,
            cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<Node>, IQueryable<Node>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<Node>> SaveAsync(IEnumerable<Node> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<Node> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Node>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<Node> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<Node>, IQueryable<Node>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Node>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<Node> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<Node>, IQueryable<Node>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Node>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }
}