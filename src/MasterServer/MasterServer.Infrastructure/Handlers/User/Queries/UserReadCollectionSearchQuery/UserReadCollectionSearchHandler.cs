using FluentValidation;
using MasterServer.Application.Models.Dto.User;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Services;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadCollectionSearchQuery;

public class UserReadCollectionSearchHandler(
    IValidator<UserReadCollectionSearchQuery> validator,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserGroupEntityService userGroupEntityService
) : IRequestHandler<UserReadCollectionSearchQuery, ResponseBase<UserReadCollectionResultDto>>
{
    public async Task<ResponseBase<UserReadCollectionResultDto>> Handle(UserReadCollectionSearchQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = userAdvancedService.GetUserIdFromHttpContext(true);

        var isRoot =
            await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId, cancellationToken);

        var dataTerm = request.Term.ToLowerInvariant();

#pragma warning disable CA1862
        var targetUsers = await userEntityService.GetCollection(request.PageModel, query =>
        {
            return query
                .Where(_ => string.IsNullOrEmpty(request.Term)
                            || _.Alias.ToLower().Contains(dataTerm) || dataTerm.Contains(_.Alias.ToLower())
                            || _.FirstName.ToLower().Contains(dataTerm) || dataTerm.Contains(_.FirstName.ToLower())
                            || _.LastName.ToLower().Contains(dataTerm) || dataTerm.Contains(_.LastName.ToLower())
                            || _.Patronymic.ToLower().Contains(dataTerm) || dataTerm.Contains(_.Patronymic.ToLower())
                            || _.Email.ToLower().Contains(dataTerm) || dataTerm.Contains(_.Email.ToLower()));
        }, true, cancellationToken);
#pragma warning restore CA1862

        return new ResponseBase<UserReadCollectionResultDto>
        {
            Data = await UserMapper.ToUserReadCollectionResultDto(targetUsers, userGroupEntityService, isRoot,
                cancellationToken)
        };
    }
}