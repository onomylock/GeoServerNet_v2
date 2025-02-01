using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshCommand;

public class AuthRefreshCommandValidator : AbstractValidator<AuthRefreshCommand>
{
    public AuthRefreshCommandValidator()
    {
        
    }
}