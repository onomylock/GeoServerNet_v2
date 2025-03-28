using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Services.Data;

public class ClusterEntityService(IMasterServerRepository<Cluster> entityRepository) : IClusterEntityService
{
    public Task<Cluster> AddAsync(Cluster entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Cluster> SaveAsync(Cluster entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Cluster entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Cluster> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> BulkUpdate(Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<Cluster>, SetPropertyCalls<Cluster>>> setPropertyCalls,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> BulkDelete(Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Cluster>> SaveAsync(IEnumerable<Cluster> entities,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(IEnumerable<Cluster> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(int total, IReadOnlyCollection<Cluster> entities)> GetCollection(PageModel pageModel,
        Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(string prev, IReadOnlyCollection<Cluster> entities, string next)> GetCollection(
        CursorModel cursorModel, Func<IQueryable<Cluster>, IQueryable<Cluster>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}