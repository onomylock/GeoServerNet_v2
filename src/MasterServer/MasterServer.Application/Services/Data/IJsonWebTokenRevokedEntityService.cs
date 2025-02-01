using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface IJsonWebTokenRevokedEntityService : IEntityServiceBase<JsonWebTokenRevoked>
{
    Task<JsonWebTokenRevoked> GetByJsonWebTokenId(Guid jsonWebTokenId, bool asNoTracking = false, CancellationToken cancellationToken = default);
    Task PurgeAsync(CancellationToken cancellationToken = default);
}