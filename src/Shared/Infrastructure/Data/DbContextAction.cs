using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Application.Data;
using Shared.Common.Exceptions;
using Shared.Domain.Entity.Base;

namespace Shared.Infrastructure.Data;

public class DbContextAction<TDbContext>(TDbContext dbContext) : IDbContextAction<TDbContext>, IDbContextEntityAction,
    IDbContextTransactionAction
    where TDbContext : DbContext
{
    private bool TransactionInProgress { get; set; }

    public TDbContext DbContext { get; } = dbContext;

    public void Commit()
    {
        var entityEntries = DbContext.ChangeTracker.Entries().ToArray();

        UpdateTimestamps(entityEntries);

        DbContext.SaveChanges();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var entityEntries = DbContext.ChangeTracker.Entries().ToArray();

        UpdateTimestamps(entityEntries);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public void BeginTransaction(bool shouldThrow = false)
    {
        if (TransactionInProgress)
        {
            if (shouldThrow)
                throw new AnotherTransactionInProgressException();
        }
        else
        {
            DbContext.Database.BeginTransaction();
            TransactionInProgress = true;
        }
    }

    public void BeginTransaction(IsolationLevel isolationLevel, bool shouldThrow = false)
    {
        if (TransactionInProgress)
        {
            if (shouldThrow) throw new AnotherTransactionInProgressException();
        }
        else
        {
            DbContext.Database.BeginTransaction(isolationLevel);
            TransactionInProgress = true;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default, bool shouldThrow = false)
    {
        if (TransactionInProgress)
        {
            if (shouldThrow)
                throw new AnotherTransactionInProgressException();
        }
        else
        {
            await DbContext.Database.BeginTransactionAsync(cancellationToken);
            TransactionInProgress = true;
        }
    }

    Task IDbContextTransactionAction.BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken,
        bool shouldThrow)
    {
        return BeginTransactionAsync(isolationLevel, cancellationToken, shouldThrow);
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default, bool shouldThrow = false)
    {
        if (TransactionInProgress)
        {
            if (shouldThrow) throw new AnotherTransactionInProgressException();
        }
        else
        {
            await DbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            TransactionInProgress = true;
        }
    }

    public void CommitTransaction(bool shouldThrow = false)
    {
        if (!TransactionInProgress)
        {
            if (shouldThrow)
                throw new NoTransactionInProgressException();
        }
        else
        {
            DbContext.Database.CommitTransaction();
            TransactionInProgress = false;
        }
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default, bool shouldThrow = false)
    {
        if (!TransactionInProgress)
        {
            if (shouldThrow)
                throw new NoTransactionInProgressException();
        }
        else
        {
            await DbContext.Database.CommitTransactionAsync(cancellationToken);
            TransactionInProgress = false;
        }
    }

    public void RollbackTransaction(bool shouldThrow = false)
    {
        if (TransactionInProgress)
        {
            DbContext.Database.RollbackTransaction();
        }
        else
        {
            if (shouldThrow)
                throw new NoTransactionInProgressException();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default, bool shouldThrow = false)
    {
        if (TransactionInProgress)
        {
            await DbContext.Database.RollbackTransactionAsync(cancellationToken);
        }
        else
        {
            if (shouldThrow)
                throw new NoTransactionInProgressException();
        }
    }

    private void UpdateTimestamps(IEnumerable<EntityEntry> entityEntries)
    {
        foreach (var entityEntry in entityEntries.Where(_ => _.State is EntityState.Modified or EntityState.Added))
        {
            var dateTimeOffsetUtcNow = DateTimeOffset.UtcNow;

            var entity = entityEntry.Entity;

            if (entity is EntityBase entityAsEntityBase)
            {
                if (entityEntry.State == EntityState.Added)
                    entityAsEntityBase.CreatedAt = dateTimeOffsetUtcNow;
                entityAsEntityBase.UpdatedAt = dateTimeOffsetUtcNow;
            }
        }
    }

    public class AnotherTransactionInProgressException : LocalizedException;

    public class NoTransactionInProgressException : LocalizedException;
}