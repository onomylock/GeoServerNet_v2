using System.Linq.Expressions;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Services.Data;

public class RouteEntityService : IRouteEntityService
{
    public Task<Route> AddAsync(Route entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Route> SaveAsync(Route entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Route entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Route> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> BulkUpdate(Func<IQueryable<Route>, IQueryable<Route>> queryTransformationFunction, Expression<Func<SetPropertyCalls<Route>, SetPropertyCalls<Route>>> setPropertyCalls,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> BulkDelete(Func<IQueryable<Route>, IQueryable<Route>> queryTransformationFunction, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Route>> SaveAsync(IEnumerable<Route> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(IEnumerable<Route> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(int total, IReadOnlyCollection<Route> entities)> GetCollection(PageModel pageModel, Func<IQueryable<Route>, IQueryable<Route>> queryTransformationFunction, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(string prev, IReadOnlyCollection<Route> entities, string next)> GetCollection(CursorModel cursorModel, Func<IQueryable<Route>, IQueryable<Route>> queryTransformationFunction, bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Route> GetByAliasAsync(string alias, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}