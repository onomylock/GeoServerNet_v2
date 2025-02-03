using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class SolutionEntityService(IMasterServerRepository<Solution> entityRepository) : ISolutionEntityService
{
    public Task<Solution> AddAsync(Solution entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Solution> SaveAsync(Solution entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(Solution entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Solution> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<Solution>, IQueryable<Solution>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<Solution>, SetPropertyCalls<Solution>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Solution>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<Solution>, IQueryable<Solution>> queryTransformationFunction, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<Solution>> SaveAsync(IEnumerable<Solution> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<Solution> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Solution>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<Solution> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<Solution>, IQueryable<Solution>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Solution>.GetCollection(entityRepository, pageModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<Solution> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<Solution>, IQueryable<Solution>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Solution>.GetCollection(entityRepository, cursorModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }
}