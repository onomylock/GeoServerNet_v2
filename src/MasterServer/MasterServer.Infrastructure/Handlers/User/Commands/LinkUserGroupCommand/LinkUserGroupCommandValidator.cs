using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Commands.LinkUserGroupCommand;

public class LinkUserGroupCommandValidator : AbstractValidator<LinkUserGroupCommand>
{
    public LinkUserGroupCommandValidator()
    {
        RuleFor(_ => _.UserId).NotEmpty();
    }
}