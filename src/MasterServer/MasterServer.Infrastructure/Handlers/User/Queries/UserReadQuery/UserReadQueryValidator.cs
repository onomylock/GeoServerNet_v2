using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadQuery;

public class UserReadQueryValidator : AbstractValidator<UserReadQuery>
{
    public UserReadQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}