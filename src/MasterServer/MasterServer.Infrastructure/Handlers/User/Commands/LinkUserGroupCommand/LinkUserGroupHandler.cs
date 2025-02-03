using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Exceptions;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Commands.LinkUserGroupCommand;

public class LinkUserGroupHandler(
    IValidator<LinkUserGroupCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserGroupEntityService userGroupEntityService,
    IUserToUserGroupMappingEntityService userToUserGroupMappingEntityService
) : IRequestHandler<LinkUserGroupCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(LinkUserGroupCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var response = new ResponseBase<OkResult>();

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var userId = userAdvancedService.GetUserIdFromHttpContext(true);

            if (!(await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId,
                      cancellationToken) ||
                  await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.ManageUsersUserGroupId,
                      cancellationToken)))
                throw new InsufficientPermissionsException();

            var targetUser = await userEntityService.GetByIdAsync(request.UserId, true, cancellationToken) ??
                             throw new UserNotFoundException();

            var errors = new List<ErrorBase>();

            foreach (var userGroupId in request.UserGroupIds)
                try
                {
                    var userGroup = await userGroupEntityService.GetByIdAsync(userGroupId, true, cancellationToken)
                                    ?? throw new UserGroupNotFoundException();

                    await userToUserGroupMappingEntityService.SaveAsync(new UserToUserGroupMapping
                    {
                        EntityLeftId = targetUser.Id,
                        EntityRightId = userGroup.Id
                    }, cancellationToken);
                }
                catch (Exception)
                {
                    errors.Add(new ErrorBase
                    {
                        ErrorMessage = Localize.Keys.Warning.MappingAlreadyExists
                    });
                }

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            response.Errors = errors;

            return response;
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);

            throw;
        }
    }
}