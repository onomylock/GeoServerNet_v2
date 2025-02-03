using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserUpdateCommand;

public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
        RuleFor(x => x.Alias).NotEmpty();
    }
}