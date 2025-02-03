using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserCreateCommand;

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(_ => _.Alias).NotEmpty();
        RuleFor(_ => _.FirstName).NotEmpty();
    }
}