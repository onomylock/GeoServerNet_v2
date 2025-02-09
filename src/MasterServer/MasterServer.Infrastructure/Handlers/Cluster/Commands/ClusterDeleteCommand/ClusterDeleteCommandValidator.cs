using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterDeleteCommand;

public class ClusterDeleteCommandValidator : AbstractValidator<ClusterDeleteCommand>
{
    public ClusterDeleteCommandValidator()
    {
        
    }
}