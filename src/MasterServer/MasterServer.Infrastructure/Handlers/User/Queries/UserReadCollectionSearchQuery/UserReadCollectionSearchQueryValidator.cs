using FluentValidation;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadCollectionSearchQuery;

public class UserReadCollectionSearchQueryValidator : AbstractValidator<UserReadCollectionSearchQuery>
{
    public UserReadCollectionSearchQueryValidator()
    {
        RuleFor(_ => _.Term).NotEmpty().MaximumLength(64);
        RuleFor(_ => _.PageModel).NotEmpty();
    }
}