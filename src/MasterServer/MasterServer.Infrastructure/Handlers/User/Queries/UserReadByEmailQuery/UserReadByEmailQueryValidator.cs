using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadByEmailQuery;

public class UserReadByEmailQueryValidator : AbstractValidator<UserReadByEmailQuery>
{
    public UserReadByEmailQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
    }
}