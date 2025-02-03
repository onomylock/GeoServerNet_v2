using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.UserGroup;
using MasterServer.Application.Services.Data;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.UserGroup.Queries.UserGroupReadQuery;

public class UserGroupReadHandler(
    IValidator<UserGroupReadQuery> validator,
    IUserGroupEntityService userGroupEntityService
) : IRequestHandler<UserGroupReadQuery, ResponseBase<UserGroupReadResultDto>>
{
    public async Task<ResponseBase<UserGroupReadResultDto>> Handle(UserGroupReadQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var targetUser = await userGroupEntityService.GetByIdAsync(request.UserGroupId, true, cancellationToken) ??
                         throw new UserGroupNotFoundException();

        return new ResponseBase<UserGroupReadResultDto>
        {
            Data = new UserGroupReadResultDto
            {
                Id = targetUser.Id,
                Alias = targetUser.Alias,
                CreatedAt = targetUser.CreatedAt,
                UpdatedAt = targetUser.UpdatedAt
            }
        };
    }
}