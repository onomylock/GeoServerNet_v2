using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Application.Repository;
using Shared.Common.Extensions;
using Shared.Common.Models;
using Shared.Domain.Entity.Base;

namespace Shared.Infrastructure.Services;

public static class EntityServiceBase<TEntity> where TEntity : EntityBase
{
    public static async Task<TEntity> AddAsync(
        IRepositoryBase<TEntity> repository,
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        await repository.AddAsync(entity, cancellationToken);
        await repository.CommitAsync(cancellationToken);

        return entity;
    }

    public static async Task<TEntity> SaveAsync(
        IRepositoryBase<TEntity> repository,
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        repository.Save(entity);
        await repository.CommitAsync(cancellationToken);

        return entity;
    }

    public static async Task DeleteAsync(
        IRepositoryBase<TEntity> repository,
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        repository.Delete(entity);
        await repository.CommitAsync(cancellationToken);
    }

    public static async Task<TEntity> GetByIdAsync(
        IRepositoryBase<TEntity> repository,
        Guid id,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return await repository.Query(asNoTracking).SingleOrDefaultAsync(_ => _.Id == id, cancellationToken);
    }

    public static Task<int> BulkUpdate(
        IRepositoryBase<TEntity> repository,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return repository
            .Transform(queryTransformationFunction)
            .ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
    }

    public static Task<int> BulkDelete(
        IRepositoryBase<TEntity> repository,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        CancellationToken cancellationToken = default
    )
    {
        return repository
            .Transform(queryTransformationFunction)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public static async Task<IReadOnlyCollection<TEntity>> SaveAsync(
        IRepositoryBase<TEntity> repository,
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        var entitiesEnumerated = entities as TEntity[] ?? entities.ToArray();

        repository.Save(entitiesEnumerated);
        await repository.CommitAsync(cancellationToken);

        return entitiesEnumerated;
    }

    public static async Task DeleteAsync(
        IRepositoryBase<TEntity> repository,
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        repository.Delete(entities);
        await repository.CommitAsync(cancellationToken);
    }

    public static async Task<(int total, IReadOnlyCollection<TEntity> entities)> GetCollection(
        IRepositoryBase<TEntity> repository,
        PageModel pageModel,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var queryTransformed = repository.Transform(queryTransformationFunction, asNoTracking);

        var (total, result) = await queryTransformed.GetPage(pageModel, cancellationToken);

        return (total, result is { } ? await result.ToArrayAsync(cancellationToken) : []);
    }

    public static async Task<(string prev, IReadOnlyCollection<TEntity> entities, string next)> GetCollection(
        IRepositoryBase<TEntity> repository,
        CursorModel cursorModel,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var queryTransformed = repository.Transform(queryTransformationFunction, asNoTracking);

        var (prev, result, next) = await queryTransformed.GetPage(cursorModel, cancellationToken);

        return (prev, result is { } ? await result.ToArrayAsync(cancellationToken) : [], next);
    }
}