using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface ISolutionTypeEntityService : IEntityServiceBase<SolutionType>
{
    public Task<SolutionType> GetByAliasAsync(string alias, bool asNoTracking = false,
        CancellationToken cancellationToken = default);
}