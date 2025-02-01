using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.User;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Services;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadByEmailQuery;

public class UserReadByEmailHandler(
    IValidator<UserReadByEmailQuery> validator,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserGroupEntityService userGroupEntityService
) : IRequestHandler<UserReadByEmailQuery, ResponseBase<UserReadResultDto>>
{
    public async Task<ResponseBase<UserReadResultDto>> Handle(UserReadByEmailQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = userAdvancedService.GetUserIdFromHttpContext(true);
        
        var isRoot = await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId, cancellationToken);

        var targetUser = await userEntityService.GetByEmailAsync(request.Email, true, cancellationToken) ??
                         throw new UserNotFoundException();
        
        return new ResponseBase<UserReadResultDto>
        {
            Data = await UserMapper.ToUserReadResultDto(targetUser, userGroupEntityService, isRoot,
                cancellationToken: cancellationToken)
        };
    }
}