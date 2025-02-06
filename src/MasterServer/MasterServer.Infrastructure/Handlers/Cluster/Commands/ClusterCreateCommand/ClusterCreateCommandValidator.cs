using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;

public class ClusterCreateCommandValidator : AbstractValidator<ClusterCreateCommand>
{
    public ClusterCreateCommandValidator()
    {
        
    }
}