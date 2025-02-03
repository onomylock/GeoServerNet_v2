using System.Data.Entity;
using Shared.Application.Repository;
using Shared.Common.Models;
using Shared.Domain.Entity.Base;

namespace Shared.Common.Extensions;

public static class EntityToEntityMappingServiceBase<TEntity> where TEntity : EntityToEntityMappingBase
{
    public static Task<TEntity> GetByEntityLeftIdEntityRightIdAsync(
        IRepositoryBase<TEntity> repository,
        Guid entityLeftId,
        Guid entityRightId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return repository
            .Query(asNoTracking)
            .SingleOrDefaultAsync(_ => _.EntityLeftId == entityLeftId && _.EntityRightId == entityRightId,
                cancellationToken);
    }

    public static async Task<(int total, IReadOnlyCollection<TEntity> entities)> GetByEntityLeftIdAsync(
        IRepositoryBase<TEntity> repository,
        Guid entityLeftId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = repository
            .Query(asNoTracking)
            .Where(_ => _.EntityLeftId == entityLeftId)
            .OrderBy(_ => _.CreatedAt);

        var (total, result) = await query.GetPage(pageModel, cancellationToken);

        return (total, result is not null ? await result.ToArrayAsync(cancellationToken) : []);
    }

    public static async Task<(int total, IReadOnlyCollection<TEntity> entities)> GetByEntityRightIdAsync(
        IRepositoryBase<TEntity> repository,
        Guid entityRightId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = repository
            .Query(asNoTracking)
            .Where(_ => _.EntityRightId == entityRightId)
            .OrderBy(_ => _.CreatedAt);

        var (total, result) = await query.GetPage(pageModel, cancellationToken);

        return (total, result is not null ? await result.ToArrayAsync(cancellationToken) : []);
    }

    public static async Task<(string prev, IReadOnlyCollection<TEntity> entities, string next)> GetByEntityLeftIdAsync(
        IRepositoryBase<TEntity> repository,
        Guid entityLeftId,
        CursorModel cursorModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = repository
            .Query(asNoTracking)
            .Where(_ => _.EntityLeftId == entityLeftId)
            .OrderBy(_ => _.CreatedAt);

        var (prev, result, next) = await query.GetPage(cursorModel, cancellationToken);

        return (prev, result is not null ? await result.ToArrayAsync(cancellationToken) : [], next);
    }

    public static async Task<(string prev, IReadOnlyCollection<TEntity> entities, string next)> GetByEntityRightIdAsync(
        IRepositoryBase<TEntity> repository,
        Guid entityRightId,
        CursorModel cursorModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = repository
            .Query(asNoTracking)
            .Where(_ => _.EntityRightId == entityRightId)
            .OrderBy(_ => _.CreatedAt);

        var (prev, result, next) = await query.GetPage(cursorModel, cancellationToken);

        return (prev, result is not null ? await result.ToArrayAsync(cancellationToken) : [], next);
    }
}