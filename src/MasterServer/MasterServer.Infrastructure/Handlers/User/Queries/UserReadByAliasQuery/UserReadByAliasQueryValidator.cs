using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadByAliasQuery;

public class UserReadByAliasQueryValidator : AbstractValidator<UserReadByAliasQuery>
{
    public UserReadByAliasQueryValidator()
    {
        RuleFor(_ => _.Alias).NotEmpty();
    }
}