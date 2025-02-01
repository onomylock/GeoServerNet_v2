using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Exceptions;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UnlinkUserGroupCommand;

public class UnlinkUserGroupHandler(
    IValidator<UnlinkUserGroupCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserToUserGroupMappingEntityService userToUserGroupMappingEntityService)
    : IRequestHandler<UnlinkUserGroupCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(UnlinkUserGroupCommand request,
        CancellationToken cancellationToken)
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
                    var userToUserGroupMapping =
                        await userToUserGroupMappingEntityService.GetByEntityLeftIdEntityRightIdAsync(targetUser.Id,
                            userGroupId, cancellationToken: cancellationToken);

                    await userToUserGroupMappingEntityService.DeleteAsync(userToUserGroupMapping, cancellationToken);
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