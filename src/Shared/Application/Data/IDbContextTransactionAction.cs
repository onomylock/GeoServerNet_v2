using System.Data;

namespace Shared.Application.Data;

public interface IDbContextTransactionAction
{
    void BeginTransaction(bool shouldThrow = false);
    void BeginTransaction(IsolationLevel isolationLevel, bool shouldThrow = false);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default, bool shouldThrow = false);
    Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default, bool shouldThrow = false);
    void CommitTransaction(bool shouldThrow = false);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default, bool shouldThrow = false);
    void RollbackTransaction(bool shouldThrow = false);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default, bool shouldThrow = false);
}