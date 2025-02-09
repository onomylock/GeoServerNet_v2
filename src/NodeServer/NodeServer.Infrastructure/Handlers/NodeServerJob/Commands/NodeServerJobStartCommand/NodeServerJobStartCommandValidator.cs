using FluentValidation;

namespace NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;

public class NodeServerJobStartCommandValidator : AbstractValidator<NodeServerJobStartCommand>
{
    public NodeServerJobStartCommandValidator()
    {
        
    }
}