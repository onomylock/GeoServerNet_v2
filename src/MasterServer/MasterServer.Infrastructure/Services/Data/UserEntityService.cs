using System.Linq.Expressions;
using MasterServer.Application.Repository;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Common.Models;
using Shared.Infrastructure.Services;

namespace MasterServer.Infrastructure.Services.Data;

public class UserEntityService(
    IMasterServerRepository<User> entityRepository
) : IUserEntityService
{
    public Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.AddAsync(entityRepository, entity, cancellationToken);
    }

    public Task<User> SaveAsync(User entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.SaveAsync(entityRepository, entity, cancellationToken);
    }

    public Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.DeleteAsync(entityRepository, entity, cancellationToken);
    }

    public Task<User> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.GetByIdAsync(entityRepository, id, asNoTracking, cancellationToken);
    }

    public Task<int> BulkUpdate(
        Func<IQueryable<User>, IQueryable<User>> queryTransformationFunction,
        Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<User>.BulkUpdate(entityRepository, queryTransformationFunction, setPropertyCalls, cancellationToken);
    }

    public Task<int> BulkDelete(Func<IQueryable<User>, IQueryable<User>> queryTransformationFunction, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.BulkDelete(entityRepository, queryTransformationFunction, cancellationToken);
    }

    public Task<IReadOnlyCollection<User>> SaveAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.SaveAsync(entityRepository, entities, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default)
    {
        return EntityServiceBase<User>.DeleteAsync(entityRepository, entities, cancellationToken);
    }

    public Task<(int total, IReadOnlyCollection<User> entities)> GetCollection(
        PageModel pageModel,
        Func<IQueryable<User>, IQueryable<User>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<User>.GetCollection(entityRepository, pageModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<(string prev, IReadOnlyCollection<User> entities, string next)> GetCollection(
        CursorModel cursorModel,
        Func<IQueryable<User>, IQueryable<User>> queryTransformationFunction,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return EntityServiceBase<User>.GetCollection(entityRepository, cursorModel, queryTransformationFunction, asNoTracking, cancellationToken);
    }

    public Task<User> GetByEmailAsync(string email, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return entityRepository.Query(asNoTracking).SingleOrDefaultAsync(_ => _.Email == email, cancellationToken);
    }

    public Task<User> GetByAliasAsync(string alias, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return entityRepository.Query(asNoTracking).SingleOrDefaultAsync(_ => _.Alias == alias, cancellationToken);
    }
}