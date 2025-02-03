using System.Linq.Expressions;
using System.Text;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Helpers;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class RefreshTokenEntityService(IMasterServerRepository<RefreshToken> entityRepository)
    : IRefreshTokenEntityService
{
    public Task<RefreshToken> AddAsync(RefreshToken entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<RefreshToken> SaveAsync(RefreshToken entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(RefreshToken entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<RefreshToken> GetByIdAsync(Guid id, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<RefreshToken>, IQueryable<RefreshToken>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<RefreshToken>, SetPropertyCalls<RefreshToken>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<RefreshToken>.BulkUpdate(entityRepository, queryTransformationFunction,
            setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<RefreshToken>, IQueryable<RefreshToken>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.BulkDelete(entityRepository, queryTransformationFunction,
            cancellationToken);
    }

    public Task<RefreshToken> GetByTokenAsync(string token, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return entityRepository
            .Query(asNoTracking)
            .SingleOrDefaultAsync(_ => _.Token == Encoding.UTF8.GetBytes(token).ComputeSha256(), cancellationToken);
    }

    public Task PurgeByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.BulkDelete(entityRepository,
            query => query.Where(_ => _.UserId == userId), cancellationToken);
    }

    public Task PurgeAsync(CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.BulkDelete(entityRepository,
            query => query.Where(_ => _.ExpiresAt < DateTimeOffset.UtcNow), cancellationToken);
    }

    public Task<IReadOnlyCollection<RefreshToken>> SaveAsync(IEnumerable<RefreshToken> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<RefreshToken> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<RefreshToken>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<RefreshToken> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<RefreshToken>, IQueryable<RefreshToken>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<RefreshToken>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<RefreshToken> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<RefreshToken>, IQueryable<RefreshToken>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<RefreshToken>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }
}