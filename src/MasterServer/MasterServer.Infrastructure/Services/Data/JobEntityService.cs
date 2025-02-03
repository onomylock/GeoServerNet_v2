using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class JobEntityService(IMasterServerRepository<Job> entityRepository) : IJobEntityService
{
    public Task<Job> AddAsync(Job entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Job> SaveAsync(Job entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(Job entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<Job> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<Job>, IQueryable<Job>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<Job>, SetPropertyCalls<Job>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Job>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls,
            cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<Job>, IQueryable<Job>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<Job>> SaveAsync(IEnumerable<Job> entities,
        CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<Job> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<Job>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<Job> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<Job>, IQueryable<Job>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Job>.GetCollection(entityRepository, pageModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<Job> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<Job>, IQueryable<Job>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<Job>.GetCollection(entityRepository, cursorModel, queryTransformationFunction,
            asNoTracking, cancellationToken);
    }
}