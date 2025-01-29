namespace Shared.Application.Data;

public interface IDbContextEntityAction
{
    void Commit();
    Task CommitAsync(CancellationToken cancellationToken = default);
}