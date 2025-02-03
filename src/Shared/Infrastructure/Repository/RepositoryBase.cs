using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Shared.Application.Repository;
using Shared.Domain.Entity.Base;
using Shared.Infrastructure.Data;

namespace Shared.Infrastructure.Repository;

public abstract class RepositoryBase<TEntity, TDbContext> : IRepositoryBase<TEntity>
    where TEntity : EntityBase
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    private readonly DbContextAction<TDbContext> _dbContextAction;
    private readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(TDbContext dbContext, ILogger<DbContextAction<TDbContext>> logger)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
        _dbContextAction = new DbContextAction<TDbContext>(_dbContext);
    }

    public DbConnection DbConnection => _dbContext.Database.GetDbConnection();

    public void Save(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Save(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return _dbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    public Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public IQueryable<TEntity> Transform(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformationFunction,
        bool asNoTracking = false)
    {
        return queryTransformationFunction(Query(asNoTracking).OrderBy(_ => _.CreatedAt));
    }

    public IQueryable<TEntity> Query(bool asNoTracking = false)
    {
        return asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
    }

    public DbSet<TEntity> DbSet()
    {
        return _dbSet;
    }

    public EntityEntry<TEntity> Attach(TEntity entity)
    {
        return _dbContext.Attach(entity);
    }

    public void Attach(IEnumerable<TEntity> entities)
    {
        _dbContext.AttachRange(entities);
    }

    public Type GetEntityType()
    {
        return typeof(TEntity);
    }

    public void Commit()
    {
        _dbContextAction.Commit();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _dbContextAction.CommitAsync(cancellationToken);
    }

    public string GetTableName()
    {
        var model = _dbContext.Model;
        var entityTypes = model.GetEntityTypes();
        var entityType = entityTypes.First(t => t.ClrType == typeof(TEntity));
        var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
        return tableNameAnnotation.Value?.ToString();
    }
}