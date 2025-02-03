using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class JsonWebTokenRevokedEntityService(IMasterServerRepository<JsonWebTokenRevoked> entityRepository) : IJsonWebTokenRevokedEntityService
{
    public Task<JsonWebTokenRevoked> AddAsync(JsonWebTokenRevoked entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<JsonWebTokenRevoked> SaveAsync(JsonWebTokenRevoked entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(JsonWebTokenRevoked entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<JsonWebTokenRevoked> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<JsonWebTokenRevoked>, IQueryable<JsonWebTokenRevoked>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<JsonWebTokenRevoked>, SetPropertyCalls<JsonWebTokenRevoked>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<JsonWebTokenRevoked>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<JsonWebTokenRevoked>, IQueryable<JsonWebTokenRevoked>> queryTransformationFunction, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<JsonWebTokenRevoked> GetByJsonWebTokenId(Guid jsonWebTokenId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return entityRepository.Query(asNoTracking).SingleOrDefaultAsync(_ => _.JsonWebTokenId == jsonWebTokenId, cancellationToken);
    }

    public Task PurgeAsync(CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.BulkDelete(entityRepository, query => query.Where(_ => _.ExpiresAt < DateTimeOffset.UtcNow), cancellationToken);
    }

    public Task<IReadOnlyCollection<JsonWebTokenRevoked>> SaveAsync(IEnumerable<JsonWebTokenRevoked> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<JsonWebTokenRevoked> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<JsonWebTokenRevoked>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<JsonWebTokenRevoked> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<JsonWebTokenRevoked>, IQueryable<JsonWebTokenRevoked>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<JsonWebTokenRevoked>.GetCollection(entityRepository, pageModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<JsonWebTokenRevoked> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<JsonWebTokenRevoked>, IQueryable<JsonWebTokenRevoked>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<JsonWebTokenRevoked>.GetCollection(entityRepository, cursorModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }
}