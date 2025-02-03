using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserDeleteCommand;

public class UserDeleteCommandValidator : AbstractValidator<UserDeleteCommand>
{
    public UserDeleteCommandValidator()
    {
        RuleFor(_ => _.UserId).NotEmpty();
    }
}