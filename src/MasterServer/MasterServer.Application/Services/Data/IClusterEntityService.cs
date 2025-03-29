using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface IClusterEntityService : IEntityServiceBase<Cluster>
{
    Task<Cluster> GetByAliasAsync(
        string alias,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}