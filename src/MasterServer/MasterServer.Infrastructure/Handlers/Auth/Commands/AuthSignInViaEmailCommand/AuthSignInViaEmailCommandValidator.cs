using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaEmailCommand;

public class AuthSignInViaEmailCommandValidator : AbstractValidator<AuthSignInViaEmailCommand>
{
    public AuthSignInViaEmailCommandValidator()
    {
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
    }
}