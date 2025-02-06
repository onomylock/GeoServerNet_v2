using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUnlinkNodeCommand;

public class ClusterUnlinkNodeCommandValidator : AbstractValidator<ClusterUnlinkNodeCommand>
{
    public ClusterUnlinkNodeCommandValidator()
    {
        
    }
}