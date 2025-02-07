using NodeServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace NodeServer.Application.Services.Data;

public interface INodeServerSolutionEntityService : IEntityServiceBase<NodeServerSolution>
{
    Task<NodeServerSolution> GetByMasterServerSolutionIdAsync(Guid masterServerSolutionId, bool asNoTracking = false,
        CancellationToken cancellationToken = default);
}