using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface IRefreshTokenEntityService : IEntityServiceBase<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string token, bool asNoTracking = false, CancellationToken cancellationToken = default);
    Task PurgeByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task PurgeAsync(CancellationToken cancellationToken = default);
}