using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface IDestinationEntityService : IEntityServiceBase<Destination>
{
    Task<Destination> GetByAliasAsync(
        string alias,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}