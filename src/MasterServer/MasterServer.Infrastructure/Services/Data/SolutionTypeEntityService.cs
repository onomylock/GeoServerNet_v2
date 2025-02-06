using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class SolutionTypeEntityService(IMasterServerRepository<SolutionType> entityRepository) : ISolutionTypeEntityService
{
    public Task<SolutionType> AddAsync(SolutionType entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<SolutionType> SaveAsync(SolutionType entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(SolutionType entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<SolutionType> GetByIdAsync(Guid id, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<SolutionType>, IQueryable<SolutionType>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<SolutionType>, SetPropertyCalls<SolutionType>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<SolutionType>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls,
            cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<SolutionType>, IQueryable<SolutionType>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<SolutionType>> SaveAsync(IEnumerable<SolutionType> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<SolutionType> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<SolutionType>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<SolutionType> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<SolutionType>, IQueryable<SolutionType>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<SolutionType>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<SolutionType> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<SolutionType>, IQueryable<SolutionType>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<SolutionType>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<SolutionType> GetByAliasAsync(string alias, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return entityRepository.Query(asNoTracking).SingleOrDefaultAsync(_ => _.Alias == alias, cancellationToken);
    }
}