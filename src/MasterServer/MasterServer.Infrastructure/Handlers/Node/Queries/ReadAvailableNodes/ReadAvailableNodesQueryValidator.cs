using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.Node.Queries.ReadAvailableNodes;

public class ReadAvailableNodesQueryValidator : AbstractValidator<ReadAvailableNodesQuery>
{
    public ReadAvailableNodesQueryValidator()
    {
        
    }
}