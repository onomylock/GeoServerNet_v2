using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface IRouteEntityService : IEntityServiceBase<Route>
{
    Task<Route> GetByAliasAsync(
        string alias,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}