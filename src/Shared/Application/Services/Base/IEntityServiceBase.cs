using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Domain.Entity.Base;
using PageModel = Shared.Common.Models.PageModel;

namespace Shared.Application.Services.Base;

public interface IEntityServiceBase<TEntity> where TEntity : EntityBase
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> GetByIdAsync(
        Guid id,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<int> BulkUpdate(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    );

    Task<int> BulkDelete(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<TEntity>> SaveAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    );

    Task DeleteAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    );

    Task<(int total, IReadOnlyCollection<TEntity> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<(string prev, IReadOnlyCollection<TEntity> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}